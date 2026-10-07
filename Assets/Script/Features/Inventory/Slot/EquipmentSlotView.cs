using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotView : MonoBehaviour, IItemSlot,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public EquipmentSlotType slotType;              // Inspector에서 부위 지정
    public Item currentItem { get; private set; }
    public Image iconImage;

    // 내 주소 = 장비 저장소의 (int)slotType번 칸
    // SlotView는 이 줄만 다름: new SlotAddress(ContainerType.Inventory, index)
    public SlotAddress Address => new SlotAddress(ContainerType.Equipment, (int)slotType);

    // ───── 여기부터 SlotView에도 똑같이 들어가는 부분 ─────

    Subject<(SlotAddress From, SlotAddress To)> _onDropped = new Subject<(SlotAddress From, SlotAddress To)>();
    public IObservable<(SlotAddress From, SlotAddress To)> OnDropped => _onDropped.AsObservable();

    Subject<Unit> _onDragStarted = new Subject<Unit>();
    public IObservable<Unit> OnDragStarted => _onDragStarted.AsObservable();

    Subject<Unit> _onDragEnded = new Subject<Unit>();
    public IObservable<Unit> OnDragEnded => _onDragEnded.AsObservable();

    public void OnBeginDrag(PointerEventData eventData) => _onDragStarted.OnNext(Unit.Default);

    // 비어 있어도 지우면 안 됨 — IDragHandler가 있어야 Unity가 이 칸을 "끌 수 있는 것"으로 인식함
    public void OnDrag(PointerEventData eventData) { }

    // 칸 위든 밖이든 놓으면 발행 → 드래그 아이콘 숨김용
    public void OnEndDrag(PointerEventData eventData) => _onDragEnded.OnNext(Unit.Default);

    // 출발 칸이 가방인지 장비인지 모르고 IItemSlot인지만 확인 (IDamageable과 같은 패턴)
    public void OnDrop(PointerEventData eventData)
    {
        IItemSlot fromSlot = eventData.pointerDrag?.GetComponent<IItemSlot>();
        if (fromSlot == null || fromSlot.currentItem == null) return;
        _onDropped.OnNext((fromSlot.Address, Address));
    }

    // ───── 여기까지 ─────

    // 클릭 한 번으로 해제 (기존 기능 유지)
    Subject<Unit> _onUnequipClicked = new Subject<Unit>();
    public IObservable<Unit> OnUnequipClicked => _onUnequipClicked.AsObservable();

    public void OnPointerClick(PointerEventData eventData)
    {
        // 드래그했다가 제자리에 놓으면 Unity가 클릭으로도 처리해서 장비가 빠짐 → 막기
        if (eventData.dragging) return;
        if (currentItem == null) return;
        _onUnequipClicked.OnNext(Unit.Default);
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;
        iconImage.color = new Color(1, 1, 1, 1);
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1, 1, 1, 0f);
    }
}
