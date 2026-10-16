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

                DropItem(); //연결이 비어도 아래 Destroy은 반드시 실행
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

    //드롭 프리팹과 드롭 아이템이 둘 다 연결돼 있을 때만 바닥에 떨어뜨린다
    void DropItem()
    {
        if(droppedItemPrefab == null || enemyModel.DropItem == null)
        {
            // 두 번째 인자 this: Console에서 이 로그를 클릭하면 해당 몬스터가 선택됨
            Debug.LogWarning($"{name}: 드롭 연결이 비어 있어 드롭을 건너뜀", this);
            return;
        }

        DroppedItemView dropped = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity);
        dropped.Setup(enemyModel.DropItem.ToItem());
    }
}
