using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UniRx;
using System;
using Zenject;
public class PlayerView : MonoBehaviour
{
    [Inject] PlayerInputHandler playerinput;

    //좌클릭 발행
    private Subject<Vector2> onClickSubject
        = new Subject<Vector2>();

    public IObservable<Vector2> Onclick
        => onClickSubject.AsObservable();

    //움직임 처리 발행
    private Subject<Vector2> moveSubject
        = new Subject<Vector2>();

    public IObservable<Vector2> Onmove
        => moveSubject.AsObservable();
    
    [Inject]
    public void Initialize()
    {

        //좌클릭 처리   
        playerinput.PlayerOnClick
            .Subscribe(worldPos => onClickSubject.OnNext(worldPos))
            .AddTo(this);

        //움직임 처리   
        playerinput.OnMove
            .Subscribe(input => moveSubject.OnNext(input))
            .AddTo(this);
    }
  
    public void SetPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
