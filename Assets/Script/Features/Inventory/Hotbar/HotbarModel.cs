using UniRx;
//핫바 고유 상태: 선택된 칸 번호. 아이템 데이터는 InventoryModel이 갖고 있음
public class HotbarModel
{
    private readonly int slotCount;
    private readonly ReactiveProperty<int> selectedIndex = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> SelectedIndex => selectedIndex;

    public HotbarModel(int slotCount) => this.slotCount = slotCount;
    //숫자키 : 범위 밖이면 무시
    public void Select(int index)
    {
        if (index < 0 || index >= slotCount) return;
        selectedIndex.Value = index;
    }

    // 휠 +1/-1 끝에서 반대편으로 넘어감
    public void Scroll(int delta)
    {
        selectedIndex.Value = (selectedIndex.Value + delta + slotCount) % slotCount;
    }
}
