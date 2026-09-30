using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerModel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerInputHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerPresenter>()
            .FromComponentInHierarchy().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<InventoryPresenter>()
            .FromComponentInHierarchy().AsSingle().NonLazy();

        // 칸 전담 Presenter (EquipmentPresenter 자리를 대신함)
        Container.BindInterfacesAndSelfTo<SlotPresenter>()
            .FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<TooltipView>().FromComponentInHierarchy().AsSingle();

        // 획득 범위 View (씬에 하나 → 단수 + AsSingle)
        Container.Bind<PickupRangeView>()
            .FromComponentInHierarchy()
            .AsSingle();

        // 장비 칸 View (여러 개 → 복수형 + AsCached, 주입은 List<T>)
        Container.Bind<EquipmentSlotView>()
            .FromComponentsInHierarchy()
            .AsCached();

        // 장비 Model (순수 C# 클래스라 FromComponent 계열 금지)
        Container.Bind<EquipmentModel>()
            .AsSingle();

        // DI 스왑 지점: 공격 방식 교체
        Container.Bind<IAttackService>()
            .To<NormalAttackService>()
            .AsSingle();

        // 몬스터가 플레이어 위치만 최소 의존으로 주입받는 통로
        Container.Bind<Transform>()
            .WithId("Player")
            .FromMethod(ctx => ctx.Container.Resolve<PlayerView>().transform);
    }
}
