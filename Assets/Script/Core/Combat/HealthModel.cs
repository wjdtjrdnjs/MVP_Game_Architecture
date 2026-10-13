using UnityEngine;
using UniRx;
using System;
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


    //사망 알림
    private readonly Subject<Unit> onDead
        = new Subject<Unit>();
    public IObservable<Unit> OnDead
        => onDead.AsObservable();

    //현재 HP가 최대 HP 이상인지 확인
    public bool IsFull => HP.Value >= MaxHP;

    // 죽었는지 판단하는 기준은 여기 한 곳
    public bool IsDead => HP.Value <= 0;
    //체력 회복
    public void Heal(int amount)
    {
        if (IsDead) return; //죽은 뒤에 회복불가
        if (amount <= 0) return; //0이하 회복량 무시

        //최대 HP를 넘지 않도록 잘라서 반영
        //ReactiveProperty 값이 바뀌면 HP UI 등 구독자에게 자동 전달
        HP.Value = Mathf.Min(HP.Value + amount, MaxHP);
    }

    //TakeDmanage
    public virtual void TakeDamage(int damage)
    {
        if (IsDead) return; //이미 죽었으면 무시
        HP.Value = Mathf.Max(HP.Value - damage, 0); // 0아래로 안내려감
        if (IsDead)
            onDead.OnNext(Unit.Default);
    }
    //부활 전용 
    public void Revive() => HP.Value = MaxHP;
}
