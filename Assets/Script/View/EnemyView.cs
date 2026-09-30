using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UniRx;
using System;
using Zenject;

public class EnemyView : MonoBehaviour
{
    private Subject<Collider2D> onColliderSubject //범위 안으로 들어옴
           = new Subject<Collider2D>();
    public IObservable<Collider2D> OnCollider 
        => onColliderSubject.AsObservable();

    private Subject<Collider2D> onColliderExitSubject //범위 밖으로 나감
           = new Subject<Collider2D>();
    public IObservable<Collider2D> OnColliderExit
        => onColliderExitSubject.AsObservable();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("충돌");
        onColliderSubject.OnNext(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onColliderExitSubject.OnNext(collision);
    }

}
