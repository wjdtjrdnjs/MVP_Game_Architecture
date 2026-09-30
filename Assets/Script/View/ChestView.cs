using UnityEngine;
using Zenject;

// 상자 오브젝트마다 1개 자기만의 창고를 만들어 들고 있다

public class ChestView : MonoBehaviour
{

    //이 상자의 칸 수 (
    [SerializeField] private int capacity = 16;

    public InventoryModel Storage {  get; private set; }

   

    private void Awake()
    {
        Storage = new InventoryModel(capacity);
        Debug.Log($"[Chest] {name} 창고 생성: {Storage.Capacity}칸");
    }
}
