using NUnit.Framework.Constraints;
using System;
using System.Security.Cryptography.X509Certificates;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    //플레이어 클릭 처리
    private Subject<Vector2> onClickSubject
        = new Subject<Vector2> ();

    public IObservable<Vector2> PlayerOnClick
        => onClickSubject.AsObservable();

    //플레이어 움직임 처리 
    private Subject<Vector2> moveSubject
        = new Subject<Vector2>();

    public IObservable<Vector2> OnMove
        => moveSubject.AsObservable();

    //플레이어 인벤토리 활성화 처리 
    private Subject<Unit> onInventorySubject
        = new Subject<Unit>();

    public IObservable<Unit> OnInventory
        => onInventorySubject.AsObservable();

    // E 키 누른 순간의 마우스 월드 좌표를 보냄
    private readonly Subject<Vector2> onInteract = new Subject<Vector2> ();
    private readonly Subject<Unit> onCancel = new Subject<Unit> ();
    public IObservable<Unit> OnCancel => onCancel;
    public IObservable<Vector2> OnInteract => onInteract;
  
    


    void Update()
    {
        //사용자 좌클릭 조건문
        if (Input.GetMouseButtonDown(0))
        {
           
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            onClickSubject.OnNext(worldPos);
        }

        //인벤토리 활성화 
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("InputHandler : 인벤토리 키 입력");
            onInventorySubject.OnNext(Unit.Default);
        }

        //마우스 좌표 전달
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 world = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            onInteract.OnNext(world);
        }        
        
        ////아이템 추가
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Debug.Log("InputHandler : 아이템 추가");
        //    onInventorySubject.OnNext(Unit.Default);
        //}

        //플레이어 움직임
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
            );

        if(input != Vector2.zero)
        {
            moveSubject.OnNext(input); //방향값 전달
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
           onCancel.OnNext(Unit.Default);
        }
    }
    
    public IObservable<int> HotbarKey => Observable.EveryUpdate()
        .Select(_ => GetPressedNumber())
        .Where(i => i >= 0);

    int GetPressedNumber()
    {
        for(int i = 0; i < 10; i++)
        {
            //Alpha1 - Alpha9 ->
            KeyCode key = 1 == 9 ? KeyCode.Alpha0 : KeyCode.Alpha1 + 1;
            if (Input.GetKeyDown(key)) return i;
        }
        return -1; //안눌림
    }
    public IObservable<int> HotbarScroll => Observable.EveryUpdate()
        .Select(_ => Input.mouseScrollDelta.y)
        .Where(y => y != 0)
        .Select(y => y > 0 ? -1 : 1);
}
