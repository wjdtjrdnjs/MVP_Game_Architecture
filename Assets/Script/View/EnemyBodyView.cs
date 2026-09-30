using System;
using UniRx;
using UnityEngine;

public class EnemyBodyView : MonoBehaviour
{
    private Subject<Collider2D> onBodyContactSubject //범위 안으로 들어옴
           = new Subject<Collider2D>();
    public IObservable<Collider2D> OnBodyContact
        => onBodyContactSubject.AsObservable();


    private Subject<Collider2D> onBodyContactStaySubject 
           = new Subject<Collider2D>();
    public IObservable<Collider2D> OnBodyContactStay
        => onBodyContactStaySubject.AsObservable();

    /// <summary>
    /// 충돌이 되면 호출
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onBodyContactSubject.OnNext(collision);
    }

    /// <summary>
    /// 겹쳐 있는 동안 매 프레임 호출
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        onBodyContactStaySubject.OnNext(collision);
        //Debug.Log($"닿음: {collision.name}");
    }

}
