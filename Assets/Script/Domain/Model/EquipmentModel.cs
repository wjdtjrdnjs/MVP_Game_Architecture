using System;
using System.Collections.Generic;
using UniRx;

public class EquipmentModel : IItemContainer
{
    // 부위별 착용 아이템. 비어 있으면 null.
    // 알림을 OnSlotChanged 하나로 통일해서 부위별 ReactiveProperty는 필요 없어졌다.
    // 바꿀 땐 반드시 SetEquipped를 거친다 → 변경과 알림이 항상 한 세트.
    readonly Dictionary<EquipmentSlotType, Item> equipped = new Dictionary<EquipmentSlotType, Item>
    {
        { EquipmentSlotType.Head,  null },
        { EquipmentSlotType.Chest, null },
        { EquipmentSlotType.Legs,  null },
        { EquipmentSlotType.Feet,  null },
    };

    // "어느 부위가 바뀌었다" — 칸 표시 갱신, (6주차) 스탯 재계산이 이걸 구독
    Subject<EquipmentSlotType> _onSlotChanged = new Subject<EquipmentSlotType>();
    public IObservable<EquipmentSlotType> OnSlotChanged => _onSlotChanged.AsObservable();

    // 이 아이템을 이 부위에 장착할 수 있는가
    // (판정을 Model에 둬야 드래그·클릭·우클릭 등 경로가 늘어도 규칙이 한 벌로 유지된다)
    public bool CanEquip(Item item, EquipmentSlotType slot)
    {
        return item != null && item.type == ItemType.Armor && item.slotType == slot;
    }

    public Item GetEquipped(EquipmentSlotType slot)
    {
        return equipped.TryGetValue(slot, out Item item) ? item : null;
    }

    // 장착과 해제 둘 다 이걸로 (해제 = null 넣기)
    public void SetEquipped(EquipmentSlotType slot, Item item)
    {
        if (!equipped.ContainsKey(slot)) return;   // None 같은 없는 부위는 무시
        equipped[slot] = item;
        _onSlotChanged.OnNext(slot);
    }


    public Item GetItem(int index) 
        => GetEquipped((EquipmentSlotType)index);
    public bool CanPlace(int index, Item item) 
        => CanEquip(item, (EquipmentSlotType)index);
    public void SetItem(int index, Item item)
        => SetEquipped((EquipmentSlotType)index, item);
}
