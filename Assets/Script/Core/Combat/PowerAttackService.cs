using UnityEngine;

public class PowerAttackService : IAttackService
{
    public void Attack(IDamageable target)
    {
        target.TakeDamage(20);
    }
}
