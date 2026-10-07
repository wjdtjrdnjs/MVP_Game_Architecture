using UniRx;
using UnityEngine;
using Zenject;

// E로 상자를 열고, 열린 상자에 맞춰 상자 창을 보이거나 숨긴다
public class ChestInteractionPresenter : MonoBehaviour
{
    [Inject] PlayerInputHandler input;
    [Inject] ChestSession chestSession;
    [Inject] InventoryWindowModel window;
    [Inject] ChestWindowView chestWindow;
    [Inject(Id = "Player")] Transform player;   // 몬스터처럼 위치만 받는다 (규칙 7)

    [SerializeField] float interactRange = 2f;  // 상자를 열 수 있는 거리 (월드 단위)
    [SerializeField] LayerMask chestMask;       // 상자 레이어만 검사

    [Inject]
    public void Initialize()
    {
        Debug.Log("[Chest] Prester 시작");

        // E → 그 좌표에 상자가 있으면 열기 시도
        input.OnInteract.Subscribe(TryOpen).AddTo(this);

        // 상자 창을 켜고 끄는 곳은 여기 한 곳뿐
        // 구독 즉시 null이 들어와서 Hide → 닫힌 채 시작
        chestSession.Current.Subscribe(storage =>
        {
            if (storage != null) chestWindow.Show();
            else chestWindow.Hide();
        }).AddTo(this);

        input.OnCancel.Subscribe(_ =>
        {
            if(chestSession.Current.Value != null) chestSession.Close();
            else if (window.IsOpen.Value) window.Close();
            else Application.Quit();
        })
        .AddTo(this);
        window.IsOpen
            .Where(open => open)
            .Subscribe(_ => chestSession.Close())
             .AddTo(this);

    }

    void TryOpen(Vector2 mouseWorld)
    {
        // 커서 위치에 상자가 있나 (상자 레이어만 → PickupRange 같은 트리거가 안 잡힘)
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld, chestMask);
        if (hit == null || !hit.TryGetComponent(out ChestView chest)) return;

        // 플레이어와 가깝나
        float dist = Vector2.Distance(player.position, chest.transform.position);
        if (dist > interactRange) return;

        window.Close();                    // I창은 닫는다 (상자 창에 가방이 있으니까)
        chestSession.Open(chest.Storage);  // 이 상자를 연다 → 상자 창 Show + 칸 갱신
    }
}