using UnityEngine;

public class NormalAttackService : IAttackService
{
    public void Attack(IDamageable target)
    {
        target.TakeDamage(10);
    }

}
