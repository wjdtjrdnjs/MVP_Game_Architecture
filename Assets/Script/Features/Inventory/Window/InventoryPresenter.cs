using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

// 인벤토리 "창" 담당: 열고 닫기 + 툴팁
// 칸(드래그, 이동, 표시)은 전부 SlotPresenter가 한다
public class InventoryPresenter : MonoBehaviour
{
    [Inject] InventoryModel inventoryModel;
    [Inject] InventoryWindowModel window;
    [Inject] InventoryView inventoryView;
    [Inject] PlayerInputHandler playerInputHandler;
    [Inject] TooltipView tooltip;


    [SerializeField] ResourceData rockData;
    [SerializeField] ResourceData treeData;

    [Inject]
    public void Initialize()
    {
        SlotView[] slots = inventoryView.GetComponentsInChildren<SlotView>(true);

        // ── 툴팁 ──
        slots.Select(s => s.OnHoverEnter)
            .Merge()
            .Subscribe(tuple => tooltip.Show(tuple.Item1, tuple.Item2))
            .AddTo(this);

        slots.Select(s => s.OnHoverExit)
            .Merge()
            .Subscribe(_ => tooltip.Hide())
            .AddTo(this);

        //열고 닫기
        playerInputHandler.OnInventory
            .Subscribe(_ => window.Toggle())
            .AddTo(this);

        window.IsOpen
            .Subscribe(open =>
            {
                if (open) inventoryView.Show();
                else 
                { 
                    inventoryView.Hide(); 
                    tooltip.Hide();
                }
            })
            .AddTo(this);

        playerInputHandler.OnInteract
            .Subscribe(world => Debug.Log($"E: {world}"))
            .AddTo(this);
    }

   
}
