using UnityEngine;
using UniRx;
using Zenject;
using System;
using System.Collections.Generic;
// 플레이어 사망 후 처리 아이템 회수 -> 사망 상자 / 바닥 드롭 리스폰
public class PlayerDeathPresenter : MonoBehaviour
{
    [Inject] PlayerModel playerModel;
    [Inject] InventoryModel inventoryModel;
    [Inject] EquipmentModel equipmentModel;
    [Inject] ChestSession chestSession;
    [Inject] InventoryWindowModel window;

    [SerializeField] ChestView deathChestPrefab;            //부서지지 않는 20칸 상자
    [SerializeField] DroppedItemView droppedItemPrefab;     //넘친 아이템
    [SerializeField] Vector2 respawnPoint = Vector2.zero;   //시작 위치
    [SerializeField] float respawnDelay = 1f;               //죽고 나서 리스폰
    [SerializeField] float scatterRadius = 0.7f;            //바닥 드롭 흩뿌리는 반경

    // 장비를 꺼내는 순서 = 상자에 들어가는 순서 
    static readonly EquipmentSlotType[] EquipOrder =
    {
        EquipmentSlotType.Head, EquipmentSlotType.Chest,
        EquipmentSlotType.Legs, EquipmentSlotType.Feet,
    };

    [Inject]

    public void Initialize()
    {
        playerModel.OnDead
            .Subscribe(_ => HandleDeath())
            .AddTo(this);
    }

    void HandleDeath()
    {
        Vector3 deathPos = playerModel.transform.position; // 이동 전에 죽은 자리 기억

        // 열린 창 닫기
        chestSession.Close();
        window.Close();

        //장비 가방 순서로 전부 꺼내고 원래칸은 비움
        List<Item> lost = TakeAllItems();

        if (lost.Count > 0)
            SpawnDeathChest(deathPos, lost);

        //respawnDelay초 뒤 한 번 리스폰
        Observable.Timer(TimeSpan.FromSeconds(respawnDelay))
            .Subscribe(_ => Respawn())
            .AddTo(this);
    }

    //장비 4칸 -> 가방 순서로 모으고, 원래 칸은 비운다.
    // SetItem/ SetEquipped를 거치니 칸 화면 핫바는 알림으로 자동 갱신
    List<Item> TakeAllItems()
    {
        List<Item> lost = new List<Item>();

        foreach (EquipmentSlotType slot in EquipOrder)
        {
            Item item = equipmentModel.GetEquipped(slot);
            if (item == null) continue;
            lost.Add(item);
            equipmentModel.SetEquipped(slot, null);
        }

        for (int i = 0; i < inventoryModel.Capacity; i++)
        {
            Item item = inventoryModel.GetItem(i);
            if (item == null) continue;
            lost.Add(item);
            inventoryModel.SetItem(i, null);
        }
        return lost;
    }

    // 상자를 만들고 앞에서부터 채운다.  상자가 꽉 차면 나머지는 바닥으로
    void SpawnDeathChest(Vector3 pos, List<Item> items)
    {
        ChestView chest = Instantiate(deathChestPrefab, pos, Quaternion.identity);

        foreach (Item item in items)
        {
            if (chest.Storage.AddItem(item)) continue;  //상자에 들어감
            DropNear(pos, item);                        // 상자 가득 --> 바닥으로
        }
    }
    // 같은 좌표에 겹치지 않게 주변에 흩뿌려서 떨어트림
    void DropNear(Vector3 center, Item item)
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * scatterRadius;
        DroppedItemView dropped = Instantiate(droppedItemPrefab,
            center + (Vector3)offset, Quaternion.identity);
        dropped.Setup(item);
    }

    void Respawn()
    {
        playerModel.Teleport(respawnPoint);     //이동 먼저, 부활은 나중에
        playerModel.Revive();                   //Hp바는 구독으로 자동 갱신
    }
}
