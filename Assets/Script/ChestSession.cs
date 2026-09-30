using NUnit.Framework.Constraints;
using UniRx;
using UnityEngine;

// 상자는 한 번에 하나만 열린다.
public class ChestSession : IItemContainer
{
    //상태라서 ReactiveProperty (바뀔 때 SlotPresenter가 칸을 갈아끼움)
    private readonly ReactiveProperty<InventoryModel>
         current = new ReactiveProperty<InventoryModel>(null);
    public IReadOnlyReactiveProperty<InventoryModel> Current => current;

    /// <summary>
    /// 창고 열기
    /// </summary>
    /// <param name="storage"></param>
    public void Open(InventoryModel storage) => current.Value = storage;

    /// <summary>
    /// 닫기
    /// </summary>
    public void Close() => current.Value = null;

    //받은 일을 지금 열린 상자에게 넘김. 닫혀있으면 빈칸/못놓음/무시함
    public Item GetItem(int index)
        => current.Value?.GetItem(index);
    public bool CanPlace(int index, Item item)
        => current.Value?.CanPlace(index, item) ?? false;
    public void SetItem(int index, Item item)
        => current.Value?.SetItem(index, item);
}
