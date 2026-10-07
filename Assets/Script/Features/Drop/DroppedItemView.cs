using UnityEngine;

public class DroppedItemView : MonoBehaviour
{

    public Item Item { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 생성 직후 호출. 담을 아이템을 넘겨받음
    /// </summary>
    /// <param name="item"></param>
    public void Setup(Item item)
    {
        Item = item;
        spriteRenderer.sprite = item.icon;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
