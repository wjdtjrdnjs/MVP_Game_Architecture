using UnityEngine;
public class ResourceModel : HealthModel
{
    [SerializeField] ResourceData data;
    public ItemData DropItem => data.dorpItem;
    public ResourceType Type => data.type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void OnValidate()
    {
        if(data == null) return;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }
    void Start()
    {
        if (data == null) return;
        MaxHP = data.MaxHP;
        HP.Value = data.MaxHP;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    
}
