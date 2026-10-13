using UnityEngine;

public class Item
{
    // 사용효과, 아이코, 최대 스택 등 변하지 않는 정보는 여기서 꺼냄
    public ItemData Data { get; }

    public string Name;     //아이템 이름
    public ItemType type;   //아이템 타입
    public EquipmentSlotType slotType;
    public Sprite icon;      //아이템 스프라이트
    public string description; //설명


    public Item(ItemData data)
    {
        Data = data;
    }
}
