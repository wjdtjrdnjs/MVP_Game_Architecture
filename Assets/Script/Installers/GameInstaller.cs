using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] GameObject enemyPrefab; //Factory가 복제할 몬스터 프리팹

    public override void InstallBindings()
    {
        Container.Bind<InventoryModel>()
            .AsSingle()
            .WithArguments(20);
        Container.Bind<InventoryWindowModel>()
            .AsSingle();

        Container.Bind<InventoryView>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<HPBarView>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<Dragicon>()
           .FromComponentInHierarchy()
           .AsSingle();
        

        Container.Bind<HotbarView>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<HotbarModel>()
            .AsSingle()
            .WithArguments(10);

        Container.BindInterfacesAndSelfTo<HotbarPresenter>()
            .AsSingle();

        Container.Bind<ChestSession>().AsSingle();

        Container.Bind<ChestWindowView>()
            .FromComponentInHierarchy()
            .AsSingle();

        //아이템 사용
        Container.BindInterfacesAndSelfTo<ItemUsePresenter>()
            .AsSingle();

        //몬스터 생성기: EnemyFactory.Create()->enemyPrefab 복제 -> 주입
        Container.BindFactory<EnemyModel, EnemyFactory>().FromComponentInNewPrefab(enemyPrefab);
    }
}
 
