using UnityEngine;

//상자 창 
public class ChestWindowView : MonoBehaviour
{
    [SerializeField] private Transform chestGrid; //상자 칸
    [SerializeField] private Transform bagGrid;   // 가방 칸
    public SlotView[] ChestSlots => chestGrid.GetComponentsInChildren<SlotView>(true);
    public SlotView[] BagSlots => bagGrid.GetComponentsInChildren<SlotView>(true);

    //상자 창 켜기/ 끄기
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(37, -75);
    }
}
