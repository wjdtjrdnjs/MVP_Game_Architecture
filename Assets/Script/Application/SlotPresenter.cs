using System.Collections.Generic;
using System.Data;
using System.Linq;
using UniRx;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Zenject;

// 아이템 칸에 관한 일은 전부 여기서 한다.
//
//  [흐름]  드롭 / 장비칸 클릭 → Move() → Model 변경 → Model 알림 → Refresh() → 칸 표시
//          줍기(AddItem) ─────────────────────↗
//
//  - 아이템이 엉뚱한 데로 간다       → Move()
//  - 칸 그림이 데이터와 다르게 보인다 → Refresh()
public class SlotPresenter : MonoBehaviour
{
    [Inject] InventoryModel inventoryModel;
    [Inject] EquipmentModel equipmentModel;
    [Inject] InventoryView inventoryView;
    [Inject] Dragicon dragicon;
    [Inject] List<EquipmentSlotView> equipmentSlots;
    [Inject] ChestSession chestSession;
    [Inject] ChestWindowView chestWindow;

    // 주소 → 칸 View
    readonly Dictionary<(ContainerType, int), List<IItemSlot>> slotByAddress
        = new Dictionary<(ContainerType, int), List<IItemSlot>>();

    // 창고 종류 -> 창고. 셋 다 게임 내내 안바뀌어서 한 번만 등록
    Dictionary<ContainerType, IItemContainer> containers;
    
    SlotView[] chestSlots;
    readonly SerialDisposable chestSubscription = new SerialDisposable();

    [Inject]
    public void Initialize()
    {
        containers = new Dictionary<ContainerType, IItemContainer>
        {
             { ContainerType.Inventory, inventoryModel},
             { ContainerType.Equipment, equipmentModel },
             { ContainerType.Chest,     chestSession },
        };

        // ── 0. 칸 준비 ─────────────────────────────────
        // 가방 칸 번호부터 매긴다. 주소표를 만들기 전에 번호가 있어야
        // 20칸이 전부 0번으로 등록되는 일이 없다. (true = 창이 꺼져 있어도 찾음)
        SlotView[] inventorySlots = inventoryView.GetComponentsInChildren<SlotView>(true);
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].Initialize(ContainerType.Inventory, i);

        }

        Debug.Assert(inventorySlots.Length == inventoryModel.Capacity,
             $"가방 칸 수 불일치: View {inventorySlots.Length} / Model {inventoryModel.Capacity}");

       

        //ChestWindow 안의 가방 칸 = 원래 가방과 같은 창고
        SlotView[] chestBagSlots = chestWindow.BagSlots;
        for (int i = 0; i < chestBagSlots.Length; i++)
            chestBagSlots[i].Initialize(ContainerType.Inventory, i);

        Debug.Assert(chestBagSlots.Length == inventoryModel.Capacity,
        $"상자 창 가방 칸 수 불일치: View {chestBagSlots.Length} / Model {inventoryModel.Capacity}");

        chestSlots = chestWindow.ChestSlots;
        for (int i = 0; i < chestSlots.Length; i++)
            chestSlots[i].Initialize(ContainerType.Chest, i);

        // 가방 칸 + 장비 칸을 한 목록으로
        List<IItemSlot> slots = new List<IItemSlot>();
        slots.AddRange(inventorySlots);
        slots.AddRange(equipmentSlots);
        slots.AddRange(chestBagSlots);
        slots.AddRange(chestSlots);


        foreach (var slot in slots)
           Register(slot);

        // ── 1. 입력 → Move ─────────────────────────────
        slots.Select(s => s.OnDropped)
             .Merge()
             .Subscribe(drop => Move(drop.From, drop.To))
             .AddTo(this);

        foreach (var slot in equipmentSlots)
        {
            slot.OnUnequipClicked
                .Subscribe(_ => UnequipToBag(slot.Address))
                .AddTo(this);
        }

        // ── 2. Model 알림 → Refresh ────────────────────
        inventoryModel.OnSlotChanged
            .Subscribe(index => Refresh(new SlotAddress(ContainerType.Inventory, index)))
            .AddTo(this);

        equipmentModel.OnSlotChanged
            .Subscribe(type => Refresh(new SlotAddress(ContainerType.Equipment, (int)type)))
            .AddTo(this);

        foreach (var slot in slots)
            Refresh(slot.Address);   // 시작할 때 화면을 데이터에 한 번 맞춤

        // ── 3. 드래그 아이콘 ───────────────────────────
        foreach (var slot in slots)
        {
            slot.OnDragStarted
                .Where(_ => slot.currentItem != null)
                .Subscribe(_ => dragicon.Show(slot.currentItem.icon))
                .AddTo(this);

            slot.OnDragEnded
                .Subscribe(_ => dragicon.Hide())
                .AddTo(this);
        }
        // Initialize 맨 끝 (Refresh 반복문 뒤)
        chestSubscription.AddTo(this);   // SlotPresenter가 사라지면 같이 정리

        chestSession.Current.Subscribe(storage =>
        {
            // 새 상자의 칸 변경 구독. 대입하는 순간 이전 상자 구독은 자동으로 끊김 (null이면 끊기만)
            chestSubscription.Disposable = storage?.OnSlotChanged
                .Subscribe(i => Refresh(new SlotAddress(ContainerType.Chest, i)));

            // 상자가 바뀌었으니 상자 칸 전부 다시 그리기 (닫혔으면 전부 빈 칸)
            for (int i = 0; i < chestSlots.Length; i++)
                Refresh(new SlotAddress(ContainerType.Chest, i));
        }).AddTo(this);
    }

    // ───────── 이동: 모든 이동이 여기를 지난다 ─────────

    void Move(SlotAddress from, SlotAddress to)
    {
        Debug.Log($"[Move] {from} → {to}");   // 추적용. 안정되면 지워도 됨

        if (from.IsSame(to)) return;

        Item moving = GetItem(from);   // 옮기는 아이템
        Item target = GetItem(to);     // 도착 칸에 원래 있던 아이템 (없으면 null)
        if (moving == null) return;

        if (!CanPlace(to, moving)) return;                       // 도착 칸이 받아주나
        if (target != null && !CanPlace(from, target)) return;   // 스왑이면 출발 칸도 받아주나

        SetItem(from, target);   // target이 null이면 출발 칸이 비워짐
        SetItem(to, moving);
    }

    // 장비칸 클릭 = 가방 첫 빈 칸으로 Move
    void UnequipToBag(SlotAddress from)
    {
        int empty = inventoryModel.FindEmptyIndex();
        if (empty < 0) return;   // 가방이 꽉 차면 해제 안 함 (아이템 증발 방지)
        Move(from, new SlotAddress(ContainerType.Inventory, empty));
    }

    // ───────── 표시: 모든 칸 갱신이 여기를 지난다 ─────────

    void Refresh(SlotAddress a)
    {
        if (!slotByAddress.TryGetValue((a.Container, a.Index), out var views))
            return;   // 화면에 칸이 없는 부위 (예: Legs)

        Item item = GetItem(a);
        foreach(var view in views)
        {
            if (item != null) view.SetItem(item);
            else view.ClearSlot();
        }
    }

    void Register(IItemSlot slot)
    {
        var key = (slot.Address.Container, slot.Address.Index);
        if(!slotByAddress.TryGetValue(key, out var list))
        {
            list = new List<IItemSlot>();
            slotByAddress[key] = list;
        }
        list.Add(slot);
    }

    //등록표에서 창고 찾기
    IItemContainer Resolve(ContainerType c) =>
        containers.TryGetValue(c, out var container) ? container : null;

    //세 함수는 창고로 보냄
    Item GetItem(SlotAddress a)
        => Resolve(a.Container)?.GetItem(a.Index);
    bool CanPlace(SlotAddress a, Item it)
        => Resolve(a.Container)?.CanPlace(a.Index, it) ?? false;
    void SetItem(SlotAddress a, Item it)
        =>Resolve(a.Container)?.SetItem(a.Index, it);

   
}
