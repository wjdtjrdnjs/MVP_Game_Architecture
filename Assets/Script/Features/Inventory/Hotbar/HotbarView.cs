using UnityEngine;

// 핫바 view 칸 목록을 알려주고, 하이라이트만 그림
public class HotbarView : MonoBehaviour
{
    [SerializeField] private Transform slotGrid; // 칸들을 바로품은 푸보
    [SerializeField] private RectTransform selectframe; //선택 테두리

    //부모 기준으로만 찾아서 다른 UI가 섞이지 않게함
    public SlotView[] Slots => slotGrid.GetComponentsInChildren<SlotView>(true);

    public void Highlight(int index)
    {
        var slots = Slots;
        if ( index < 0 || index >= slots.Length) return;

        selectframe.SetParent(slots[index].transform, false); //f false = 부모 기준으로 크기 위치 맞춤
        selectframe.SetAsFirstSibling(); // 아이콘보다 위에 그림
    }
}
