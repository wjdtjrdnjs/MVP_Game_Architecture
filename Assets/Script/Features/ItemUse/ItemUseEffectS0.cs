using UnityEngine;
//인터페이스는 인스펙터에 표시가 안 되므로, 인퍼에티스를 구현한 SO를 다리로 둠
//ItemData는 이 타입의 필드를 갖고, 인스펙터에서 효과 에셋을 드래그해 넣음
public abstract class ItemUseEffectS0  : ScriptableObject, IItemUseEffect
{
   //실제 동작은 상속받는 각 효과 클래스가 구현
    public abstract bool TryUse(ItemUseContext context);
}
