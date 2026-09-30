using UniRx;
using UnityEngine;
using Zenject;
public class PlayerPresenter : MonoBehaviour
{
    [Inject] PlayerModel playerModel; //플레이어모델 등록
    [Inject] PlayerView playerView;  //플레이어 뷰 등록
    [Inject] IAttackService  attackService;  

    [Inject] HPBarView hpBarView; //플레이어 피통

    [Inject] InventoryModel inventoryModel;
    [Inject] PickupRangeView pickupRangeView;

    [Inject]
    public void Initialize()
    {

        //플레이어 체력
        playerModel.HP
            .Subscribe(hp => hpBarView.UpdateHP(hp, playerModel.MaxHP)) //MaxHP Public 변경 해야함
            .AddTo(this);

        //플레이어 사망
        playerModel.OnDead
            .Subscribe(_ => Debug.Log("사망"))
            .AddTo(this);


        //플레이어 좌클릭 처리  
        playerView.Onclick
            .Subscribe(worldPos =>
            {

                //"EnemyDetection" Layer를 제외한 모든 Layer를 대상으로 삼는 마스크
                // ~를 써서 "이 Layer만 빼고 다 포함" 으로 만듦
                int hitLayerMask = ~LayerMask.GetMask("EnemyDetection");
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, hitLayerMask);

                //뭔가 맞았으면
                if (hit.collider != null)
                {
                    IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
                    if (target != null)
                    {
                        attackService.Attack(target);
                        Debug.Log($" {target}이 피해를 받음");
                    }
                }
            })
            .AddTo(this);

        //플레이어 우클릭 처리


        //플레이어 이동 처리
        playerView.Onmove
           .Subscribe(input => playerModel.PlayerMove(input))
            .AddTo(this);

        //위치가 바뀌면 호출
        playerModel.Position
           .Subscribe(pos => playerView.SetPosition(pos))
           .AddTo(this);

        pickupRangeView.OnItemDetected
            .Subscribe(collision =>
            {
                DroppedItemView dropped = collision.GetComponent<DroppedItemView>();
                if (dropped != null)
                {
                    inventoryModel.AddItem(dropped.Item);
                    Destroy(dropped.gameObject);
                }
            })
            .AddTo(this);

    }
 
}
