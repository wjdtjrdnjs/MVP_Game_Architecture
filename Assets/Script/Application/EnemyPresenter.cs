using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
public class EnemyPresenter : MonoBehaviour
{
    [Inject] EnemyModel enemyModel; //몬스터 모델 등록
    [Inject] EnemyView enemyView;  //몬스터 뷰 등록
    [Inject] EnemyBodyView enemyBodyView; //몬스터 몸 뷰


    [Inject(Id = "Player")] Transform playerTransform;
    [SerializeField] private DroppedItemView droppedItemPrefab; // 바닥 아이템 프리팹
    private Rigidbody2D rb;
    [Inject]
    public void Initialize()
    {
        enemyModel.OnDead
            .Subscribe(_ =>
            {
                Vector3 dropPosition = transform.position; // 삭제 전에 좌표 기억

                DroppedItemView dropped = Instantiate(droppedItemPrefab, dropPosition, Quaternion.identity);
                dropped.Setup(enemyModel.DropItem.ToItem());

                Debug.Log("몬스터 사망");
                Destroy(gameObject);
            })
            .AddTo(this);

        //몬스터의 상태를 출력한다.
        enemyModel.CurrentState
            .Subscribe(state => Debug.Log($"EnemyState: {state}"))
            .AddTo(this);

        // other이 Player태그면 통과, 아니면 무시
        enemyView.OnCollider  
            .Where(other => other.CompareTag("Player"))
            .Subscribe(other => enemyModel.OnPlayerDetected())
            .AddTo(this);


        enemyView.OnColliderExit
            .Where(other => other.CompareTag("Player"))
            .Subscribe(other => enemyModel.OnPlayerLost())
            .AddTo(this);


        //enemyBodyView.OnBodyContact
        //    .Where(other => other.CompareTag("Player"))
        //    .Subscribe(other =>
        //    {
        //        //Debug.Log("플레이어와 충돌");
        //        if (other.TryGetComponent<IDamageable>(out var damageable))
        //        {
        //            //Debug.Log("플레이어 데미지 입음");
        //            damageable.TakeDamage(enemyModel.ContactDamage);
        //        }
        //    })
        //    .AddTo(this);


        //겹쳐있으면 데미지가 들어온다.
        enemyBodyView.OnBodyContactStay
            .Where(other => other.CompareTag("Player"))
            .Subscribe(other =>
            {
                    if (other.TryGetComponent<IDamageable>(out var damageable) 
                    && enemyModel.CanDealDamage())
                    {
                        //Debug.Log("플레이어 데미지 입음");
                        damageable.TakeDamage(enemyModel.ContactDamage);
                    }
            })
            .AddTo (this);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        enemyModel.UpdateTimer(Time.deltaTime);
        if (enemyModel.CurrentState.Value == EnemyState.Chase)
        {
            //Debug.Log("몬스터가 플레이어를 추격");
            Vector2 direction = ((Vector2)playerTransform.position - rb.position).normalized;
            Vector2 newPos = rb.position + direction * enemyModel.MoveSpeed * Time.deltaTime;
            rb.MovePosition(newPos);
        }
    }
}
