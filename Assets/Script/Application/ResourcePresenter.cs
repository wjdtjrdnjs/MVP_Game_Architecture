using UniRx;
using UnityEngine;
using Zenject;

public class ResourcePresenter : MonoBehaviour
{
    [Inject] InventoryModel inventoryModel;
    ResourceModel resourceModel;

    [Inject]
    public void Initialize()
    {
        resourceModel = GetComponent<ResourceModel>();
        //플레이어 체력
        //resourceModel.HP
        //    .Subscribe(hp => hpBarView.UpdateHP(hp, playerModel.MaxHP)) //MaxHP Public 변경 해야함
        //    .AddTo(this);

        //오브젝트 사망
        resourceModel.OnDead
            .Subscribe(_ =>
            {
                Debug.Log("OnDead 발행!");
                inventoryModel.AddItem(resourceModel.DropItem.ToItem());
                Destroy(gameObject);
            })
            .AddTo(this);
    }
}
