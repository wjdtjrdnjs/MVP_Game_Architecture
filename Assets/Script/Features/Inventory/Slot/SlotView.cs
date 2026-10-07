using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour, IItemSlot,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public int index;                               // 가방 몇 번 칸인지 (SlotPresenter가 Initialize로 매김)
    public Item currentItem { get; private set; }
    public Image iconImage;

    // 이 칸의 주소 .어느 창고의 몇 번 칸
    private SlotAddress address;

    // 
    public SlotAddress Address => address;

    // ───── 드래그 / 드롭 (EquipmentSlotView와 똑같은 부분) ─────

    Subject<(SlotAddress From, SlotAddress To)> _onDropped = new Subject<(SlotAddress From, SlotAddress To)>();
    public IObservable<(SlotAddress From, SlotAddress To)> OnDropped => _onDropped.AsObservable();

    Subject<Unit> _onDragStarted = new Subject<Unit>();
    public IObservable<Unit> OnDragStarted => _onDragStarted.AsObservable();

    Subject<Unit> _onDragEnded = new Subject<Unit>();
    public IObservable<Unit> OnDragEnded => _onDragEnded.AsObservable();

    public void OnBeginDrag(PointerEventData eventData) => _onDragStarted.OnNext(Unit.Default);

    // 비어 있어도 지우면 안 됨 — IDragHandler가 있어야 Unity가 이 칸을 "끌 수 있는 것"으로 인식함
    public void OnDrag(PointerEventData eventData) { }

    // 칸 위든 밖이든 놓으면 발행 → 드래그 아이콘 숨김용
    public void OnEndDrag(PointerEventData eventData) => _onDragEnded.OnNext(Unit.Default);

    // 출발 칸이 가방인지 장비인지 모르고 IItemSlot인지만 확인 (IDamageable과 같은 패턴)
    public void OnDrop(PointerEventData eventData)
    {
        IItemSlot fromSlot = eventData.pointerDrag?.GetComponent<IItemSlot>();
        if (fromSlot == null || fromSlot.currentItem == null) return;
        _onDropped.OnNext((fromSlot.Address, Address));
    }

    // ───── 툴팁 (가방 칸 전용, InventoryPresenter가 구독) ─────

    Subject<(Item, Vector2)> _onHoverEnter = new Subject<(Item, Vector2)>();
    public IObservable<(Item, Vector2)> OnHoverEnter => _onHoverEnter.AsObservable();

    Subject<Unit> _onHoverExit = new Subject<Unit>();
    public IObservable<Unit> OnHoverExit => _onHoverExit.AsObservable();

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem == null) return;
        _onHoverEnter.OnNext((currentItem, eventData.position));
    }

    public void OnPointerExit(PointerEventData eventData) => _onHoverExit.OnNext(Unit.Default);

    // ───── 표시 (SlotPresenter.Refresh만 부른다) ─────

    public void Initialize(ContainerType container, int index)
    {
        address = new SlotAddress(container, index);
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;
        iconImage.color = new Color(1, 1, 1, 1);
    }

    public void ClearSlot()   // 예전 이름: clearSlot
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1, 1, 1, 0);
    }
}
