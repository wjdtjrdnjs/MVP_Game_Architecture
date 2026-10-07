using System;
using UniRx;

// 가방 데이터.
// 배열은 private이라 밖에서 직접 못 바꾸고, 바꿀 땐 반드시 SetItem을 거친다
// → 데이터 변경과 알림(OnSlotChanged)이 항상 한 세트로 나간다. (유령 아이템 원천 차단)
public class InventoryModel : IItemContainer
{
    //Item[] items = new Item[20];
    private readonly Item[] items;
    public int Capacity => items.Length;

    // "몇 번 칸이 바뀌었다" — 칸 표시 갱신은 이 알림 하나로 전부 처리
    Subject<int> _onSlotChanged = new Subject<int>();
    public IObservable<int> OnSlotChanged => _onSlotChanged.AsObservable();


    public InventoryModel(int capacity)
    {
        items = new Item[capacity];
    }


    public Item GetItem(int index) => items[index];

    // 모든 배열 변경이 지나가는 유일한 통로
    public void SetItem(int index, Item item)
    {
        items[index] = item;
        _onSlotChanged.OnNext(index);
    }

    public int FindEmptyIndex()
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i] == null) return i;
        return -1;   // 가득 참
    }

    // 빈 칸에 넣기. 가득 차서 못 넣으면 false
    public bool AddItem(Item item)
    {
        int index = FindEmptyIndex();
        if (index < 0) return false;
        SetItem(index, item);
        return true;
    }

    public void RemoveItem(int index)
    {
        if (items[index] == null) return;
        SetItem(index, null);
    }

    /// <summary>
    /// 가방, 상자 실제로 있는 칸이면
    /// </summary>
    public bool CanPlace(int index, Item item) => index >= 0 && index < Capacity;
}
