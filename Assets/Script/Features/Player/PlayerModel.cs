using UnityEngine;
using UniRx;
using UnityEngine.UIElements.Experimental;
using Zenject.SpaceFighter;
using UnityEngine.UIElements;

// HealthModel을 상속해서 HP, MaxHP, OnDead, TakeDamage를 그대로 물려받음
// 상속받아서 플레이어는 맞을 수 있는 대상이 된다
// HealthModel이 IDamageble을 구현하고 있어서 EnemyPresenter는 상태가 플레이어인지 몰라도 됨
public class PlayerModel : HealthModel
{

  

    //플레이어의 위치를 보관한다. 값이 바뀌면 바로 알림
    public ReactiveProperty<Vector2> Position
        = new ReactiveProperty<Vector2>(Vector2.zero);


    public float MoveSpeed = 12f; //플레이어 이동 스피드


    Rigidbody2D rb;

    // 생성자보단 Awake가 맞지 않나
    public PlayerModel() 
    {
        MaxHP = 100;
        HP.Value = 100;
    }

    // 첫 프레임 전에 한 번. 같은 오브젝트의 Rigidbody2D를 찾아둔다.
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    public void PlayerMove(Vector2 v)
    {
        Vector2 newpos = rb.position + v * MoveSpeed * Time.deltaTime;
        rb.MovePosition(newpos);
    }

}
