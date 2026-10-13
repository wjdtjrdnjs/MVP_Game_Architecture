using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

//아이템 사용의 공통 입구
//우클릭 -> 핫바 선택 칸 아이템 -> 효과TryUse -> 성공 시 1개 소모
public class ItemUsePresenter : Zenject.IInitializable, IDisposable
{
    [Inject] private PlayerInputHandler input;      //우클릭 스트팀
    [Inject] private HotbarModel hotbar;            // 지금 선택된 칸 번호
    [Inject] private InventoryModel inventory;      //가방 = 핫바 칸의 실제 데이터
    [Inject] private PlayerModel player;            // 회복 대상 채력

    // 구독을 여기에 모았다가 Dispose에서 한 번에 해제
    private readonly CompositeDisposable disposables = new CompositeDisposable();

    //효과에 넘길 대상 묶음, 플레이어는 바뀌지 않으니 시작할 때 한 번만 만듦
    private ItemUseContext context;

    public void Initialize()
    {
        // 대상 묶음 생성
        context = new ItemUseContext(player);

        //우클릭이 올 때마다 선택 칸 아이템 사용 시도 
        input.OnUse
            .Subscribe(_ => TryUseSelected())
            .AddTo(disposables);
    }

    private void TryUseSelected()
    {
        if (player.IsDead) return; //죽어 있으면 어떤 아이템도 사용 안함
        //우클릭한 그 순간의 선택 번호
        int index = hotbar.SelectedIndex.Value;

        //그 칸의 아이템
        Item item = inventory.GetItem(index);

        if (item == null) return;

        //원본 정의에서 효과 꺼내기. 정의가 없거나 효과가ㅏ 비어있으면 사용 불가
        if(item.Data == null) return;
        IItemUseEffect effect = item.Data.UseEffect;
        if(effect == null) return;

        bool used = effect.TryUse(context);

        if (used)
            inventory.ConsumOne(index);

    }
    //씬 종료 시 구독 해제
    public void Dispose() => disposables.Dispose();
}
