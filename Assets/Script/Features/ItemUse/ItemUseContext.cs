using UnityEngine;
//아이템 효과가 영향을 줄 대상을 한 번에 담아 넘기는 클래스
//새 효과(음식, 설치)가 다른 대상이 필요하면 여기에 필드 추가
public class ItemUseContext 
{
    //회복 효과가 체력을 바꿀 대상
   public HealthModel Health { get; }

    //생성할 때 대상들을 받아서 저장
   public ItemUseContext(HealthModel health)
    {
        Health = health;
    }
}
