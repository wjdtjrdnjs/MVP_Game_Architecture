using UnityEngine;

[CreateAssetMenu(fileName = " ResourceData", menuName = "Data/ResourceData")]
public class ResourceData : ScriptableObject
{
    //리소스가 필요한 이유는 파괴됐을 때 어떤 아이템이 드롭할 지 결정
    public ResourceType type;
    //오브젝트 시각적 표현
    //Rock이면 바위..
    //Tree이면 나무..
    public Sprite sprite;
    //각자 다른 체력을 위해 변수 선언
    public int MaxHP;
    public ItemData dorpItem;

}
