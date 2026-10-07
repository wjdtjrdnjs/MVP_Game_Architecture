using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UniRx;
using System;
using Zenject;

public class InventoryView : MonoBehaviour
{

    private Subject<Unit> onIventorySubject
        = new Subject<Unit>();

    public IObservable<Unit> OnInventory
        => onIventorySubject.AsObservable();


    private void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2( 37, -46);
    }
    /// <summary>
    /// 인벤토리 열기 
    /// </summary>
    public void Show()
    {
        Debug.Log("인벤토리 열림");
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 인벤토리 닫기 
    /// </summary>
    public void Hide()
    {
        Debug.Log("인벤토리 닫힘");
        //여기서 툴팁이 꺼지게 해야함
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 아이템 추가 
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item) 
    {

    }
    /// <summary>
    /// 슬롯 전체 제거 
    /// </summary>
    public void ClearSlots()
    {

    }


   
}


