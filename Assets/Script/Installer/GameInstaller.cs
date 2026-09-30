using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InventoryModel>().AsSingle().WithArguments(20);
        Container.Bind<InventoryWindowModel>().AsSingle();

        Container.Bind<InventoryView>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<HPBarView>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<Dragicon>()
           .FromComponentInHierarchy()
           .AsSingle();


        Container.Bind<EnemyView>()
           .FromComponentInHierarchy()
           .AsSingle();

        Container.Bind<EnemyModel>()
           .FromComponentInHierarchy()
           .AsSingle();

        Container.Bind<EnemyPresenter>()
           .FromComponentInHierarchy()
           .AsSingle();


        Container.Bind<EnemyBodyView>()
           .FromComponentInHierarchy()
           .AsSingle();


        Container.Bind<ChestSession>().AsSingle();

        Container.Bind<ChestWindowView>()
            .FromComponentInHierarchy()
            .AsSingle();

    }
}
 
