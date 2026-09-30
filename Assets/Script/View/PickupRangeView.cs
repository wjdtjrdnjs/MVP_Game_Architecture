using System;
using UniRx;
using UnityEngine;

public class PickupRangeView : MonoBehaviour
{

     private Subject<Collider2D> onItemDetectedSubject
        = new Subject<Collider2D>();
    public IObservable<Collider2D> OnItemDetected
        => onItemDetectedSubject.AsObservable();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"PickupRange 충돌: {collision.name}");
        onItemDetectedSubject.OnNext(collision);
    }
 
}
