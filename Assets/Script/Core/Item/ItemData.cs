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

    //이 아이템을 우클릭했을 때 실행할 효과
    //비워두면(Null) 사용할 수 없는 아이템(우클릭해도 뭐 없음)
    [SerializeField] private ItemUseEffectS0 useEffect;

    //useEffect != null 은 Unuty 방식 비교 -> 가짜 Null도 null판정
    //비어 있으면 진짜 Null을 넘겨서, 바깥쪽 bull 검사가 정확히 동작하게 함
    public IItemUseEffect UseEffect => useEffect != null ? useEffect : null;

    //new Item(this) 생성자로 원본 정의 전달
    //뒤의 {} 기존처럼 나머지 필드 값 복사
    public Item ToItem()
    {
        return new Item(this)
        {
            Name = this.Name,               //표시 이름
            type = this.type,               //아이템 종류
            slotType = this.slotType,       //장착 부위
            icon = this.icon,               //아이콘
            description = this.description, //설명
        };

    }
}


