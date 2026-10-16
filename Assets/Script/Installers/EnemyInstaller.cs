using UnityEngine;
using Zenject;
//몬스터 한 마리 전용 조립표ㅕ
public class EnemyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<EnemyModel>().FromComponentInHierarchy().AsSingle(); //루트
        Container.Bind<EnemyView>().FromComponentInHierarchy().AsSingle();  // 루트(탐지 원)
        Container.Bind<EnemyBodyView>().FromComponentInHierarchy().AsSingle(); // 자식 Body



    }
        
}
