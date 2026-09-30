// "어느 저장소의 몇 번째 칸"을 담는 값.
// View와 Presenter가 주고받는 칸 주소 — Model 참조 없이 값만 들고 다닌다.
public struct SlotAddress
{
    public ContainerType Container;  // 어느 저장소
    public int Index;                // 그 안의 몇 번째 칸 (장비는 (int)EquipmentSlotType)

    public SlotAddress(ContainerType container, int index)
    {
        Container = container;
        Index = index;
    }

    // 구조체는 == 를 바로 못 써서 비교 메서드를 따로 둠
    public bool IsSame(SlotAddress other)
    {
        return Container == other.Container && Index == other.Index;
    }

    // Debug.Log(address) 찍으면 "Equipment[1]"처럼 보임
    public override string ToString()
    {
        return $"{Container}[{Index}]";
    }
}
