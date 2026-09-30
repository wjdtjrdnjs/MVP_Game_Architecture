using UnityEngine;
using UniRx;
public class EnemyModel : HealthModel
{
    void Awake()
    {
        MaxHP = 50;
        HP.Value = 50;
    }


    public float MoveSpeed = 0.5f; //몬스터 이동 속도

    public ReactiveProperty<EnemyState> CurrentState
        = new ReactiveProperty<EnemyState>();



    //탐지 범위, 공격범위
    public float DetectRange = 5f;
    public float AttackRange = 1.5f;

    
    private float StateTimer = 0.0f; //상태 전환 시간을 담당하는 

    public float DamageInterval = 5f; //데미지 간격(Inspector에서 조정 가능하게)
    private float lastDamageTime;     //마지막으로 데미지를 준 시각 기록
    public int ContactDamage = 1;

    public ItemData DropItem; //이 몬스터가 죽을 때 떨어뜨릴 아이템
    /// <summary>
    /// 탐지 범위에 플레이어 들어옴
    /// </summary>
    public void OnPlayerDetected()
    {
        //몬스터 상태를 추적으로 변경
        if(CurrentState.Value == EnemyState.Idle || CurrentState.Value == EnemyState.Wander)
        {
            CurrentState.Value = EnemyState.Chase;
            StateTimer = 0.0f;
        }
    }
    /// <summary>
    /// 탐지 범위에서 나감
    /// </summary>
    public void OnPlayerLost()
    {
        CurrentState.Value = EnemyState.Wander;
        StateTimer = 0.0f;
    }

    /// <summary>
    /// 시간 기반 전환
    /// </summary>
    /// <param name="deltatime"></param>
    public void UpdateTimer(float deltatime) 
    {
        StateTimer += deltatime;
        if(CurrentState.Value == EnemyState.Idle && StateTimer > 3.0f)
        {
            CurrentState.Value = EnemyState.Wander;
        }
        else if (CurrentState.Value == EnemyState.Wander && StateTimer > 3.0f)
        {
            CurrentState.Value = EnemyState.Idle;
        }
        StateTimer = 0.0f;
    }

    public bool CanDealDamage()
    {
        if(Time.time - lastDamageTime > DamageInterval)
        {
            lastDamageTime = Time.time; //마지막 데미지를 준 시각을 저장
            return true;

        }
        else return false;
    }
    /// <summary>
    /// 플레이어가 공격 범위에 안에 있음
    /// </summary>    
    public void OnPlayerInAttackRange()
    {

    }

    /// <summary>
    /// 플레이어가 공격 범위 밖에 있음
    /// </summary>
    public void OnPlayerOutOfAttackRange()
    {

    }

}
