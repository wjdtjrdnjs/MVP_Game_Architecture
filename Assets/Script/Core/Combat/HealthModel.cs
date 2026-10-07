using UnityEngine;
using UniRx;
public class HealthModel : MonoBehaviour, IDamageable
{
    //HP, 스태미나, TakeDamage 로직을 구현예정

    //ReactiveProperty는 값의 변경을 관찰할 수 있게 해주는 객체이다 
    //데이터가 발생할 때마다 자동으로 이벤트를 발생시켜 UI나 게임 로직을 갱신하는 데 사용한다고 한다. 

    //Presenter에 구독을 해야하니깐 protected대신 public를 사용한다.
    [HideInInspector]
    public ReactiveProperty<int> HP 
           = new ReactiveProperty<int>(); //0 이하로 내려가면 OnDead 발생

    public int MaxHP { get; protected set; } //최대 체력
    //스태미나
    
    //사망 
    public Subject<Unit>OnDead
             = new Subject<Unit>();
    //TakeDmanage
    public virtual void TakeDamage(int damage)
    {
        HP.Value -= damage;
        if (HP.Value <= 0)
            OnDead.OnNext(Unit.Default);
    }

}
