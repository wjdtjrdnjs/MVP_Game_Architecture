using System;
using UniRx;
using UnityEngine;
using Zenject;


//정해 둔 마리 수를 유지하는 스포너
public class EnemySpawn : MonoBehaviour
{
    [Inject] EnemyFactory enemyFactory;          // 컨테이너 전체가 아니라 "몬스터 만들기"만 받음

    [SerializeField] int maxAlive = 3;           // 이 스포너가 유지할 최대 마리 수
    [SerializeField] float spawnInterval = 5f;   //보충 확인 간격(초)
    [SerializeField] float  spawnRadius = 3f;    // 스포너 중심에서 생성 반경

    int aliveCount; //이 스포너가 만든 몬스터 중 살아 있는 수

    private void Start()
    {
        // 시작 시 한 번에 채우기
        for (int i = 0; i < maxAlive; i++)
            Spawn();

        Observable.Interval(TimeSpan.FromSeconds(spawnInterval))
            .Where(_ => aliveCount < maxAlive)
            .Subscribe(_ => Spawn())
            .AddTo(this);
    }

    void Spawn()
    {
        EnemyModel enemy = enemyFactory.Create(); // 복제

        // 스포너 주변 무작위 위치로 옮기기
        Vector2 offset = UnityEngine.Random.insideUnitCircle * spawnRadius;
        enemy.transform.position = transform.position + (Vector3)offset;

        aliveCount++;
        // 죽으면 수를 줄인다 -> 다음 확인 때 보충
        enemy.OnDead
            .Subscribe(_ => aliveCount--)
            .AddTo(this);
       
    }

    // 에디터에서 스포너를 선택하면 생성 반경을 빨간 원으로 보여줌
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

    }
}
