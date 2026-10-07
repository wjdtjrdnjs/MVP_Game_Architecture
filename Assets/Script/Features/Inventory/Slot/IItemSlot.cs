using System;
using UniRx;

// 아이템 칸의 공통 계약. 받는 쪽은 상대가 가방 칸인지 장비 칸인지 몰라도 된다.
public interface IItemSlot
{
    SlotAddress Address { get; }   // 나는 어느 저장소의 몇 번 칸인가
    Item currentItem { get; }      // 지금 표시 중인 아이템

    // View → Presenter (입력)
    IObservable<(SlotAddress From, SlotAddress To)> OnDropped { get; }
    IObservable<Unit> OnDragStarted { get; }
    IObservable<Unit> OnDragEnded { get; }

    // Presenter → View (표시). SlotPresenter.Refresh()만 부른다
    void SetItem(Item item);
    void ClearSlot();
}
