using UnityEngine;
using UniRx;
using UnityEngine.UIElements.Experimental;
using Zenject.SpaceFighter;
using UnityEngine.UIElements;

public class PlayerModel : HealthModel
{

    //플레이어모델
    //HP
    //스태미나 상태관리
    //TakeDamage()
    //RecoverStamina()
    //OnDead 이벤트

    //위치
    public ReactiveProperty<Vector2> Position
        = new ReactiveProperty<Vector2>(Vector2.zero);

    public float MoveSpeed = 12f;

    //이동완료 이벤트
    public Subject<Unit> OnMoveComplete
        = new Subject<Unit>();


    Rigidbody2D rb;

    public PlayerModel()
    {
        MaxHP = 100;
        HP.Value = 100;
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// 
    /// </summary>
 
    public void PlayerMove(Vector2 v)
    {
        Vector2 newpos = rb.position + v * MoveSpeed * Time.deltaTime;
        rb.MovePosition(newpos);
    }

}
