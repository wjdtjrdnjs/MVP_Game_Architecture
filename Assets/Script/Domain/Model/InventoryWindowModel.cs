using UniRx;
using UnityEngine;

//가방 창의 열림/닫힘 상태 관리
public class InventoryWindowModel
{
    //초기값 false 게임 시작 시 닫힌 상태
    private readonly ReactiveProperty<bool> isOpen = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsOpen => isOpen;

    /// <summary>
    /// 열기
    /// </summary>
    public void Open() => isOpen.Value = true;
    /// <summary>
    /// 닫기
    /// </summary>
    public void Close() => isOpen.Value = false;
    /// <summary>
    /// 열려 있으면 닫고, 닫혀 있으면 연다
    /// </summary>
    public void Toggle() => isOpen.Value = !isOpen.Value;


}
