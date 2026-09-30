using TMPro;
using UnityEngine;

public class TooltipView : MonoBehaviour
{
    public TextMeshProUGUI itemNameText; //아이템 이름
    public TextMeshProUGUI itemDescriptionText; //아이템 설명

    public void Show(Item item, Vector2 vector)
    {
        transform.position = vector + new Vector2(12, 0);
        itemNameText.text = item.Name;
        itemDescriptionText.text = item.description;
        gameObject.SetActive(true);

    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
