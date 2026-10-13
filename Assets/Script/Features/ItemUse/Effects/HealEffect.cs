using UnityEngine;
//회복량만 다르게 해서 '작은 표선', '큰 포션' 에셋을 여러 개 만들 수 있음
[CreateAssetMenu(menuName = "Item/Effect/Heal")]
public class HealEffect :  ItemUseEffectS0
{
    //이 효과가 회복시키는 양
    [SerializeField] private int healAmount = 20;

    public override bool TryUse(ItemUseContext context)
    {
        //컨텍스트에서 회복 대상 체력 꺼내기
        var health = context.Health;

        //죽었거나 HP가 가득이면 실패-> 포션 소모xx
        if (health == null ||health.IsDead || health.IsFull) return false;

        health.Heal(healAmount);
        return true;
    }
}
