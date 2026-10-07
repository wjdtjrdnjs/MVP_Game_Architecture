using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/IteaData")]

public class ItemData : ScriptableObject
{
    //Item 클래스를 사용하기엔 너무 많은 길을 와버려서 새로 만듦
    public string Name;
    public ItemType type;
    public EquipmentSlotType slotType;
    public Sprite icon;
    public string description;

    public Item ToItem()
    {
        return new Item
        {
            Name = this.Name,
            type = this.type,
            slotType = this.slotType,
            icon = this.icon,
            description = this.description,
        };

    }
}


