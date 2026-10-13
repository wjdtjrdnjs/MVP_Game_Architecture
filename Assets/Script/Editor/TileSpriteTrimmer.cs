using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

/// <summary>
/// AI로 만든 타일시트의 "타일 사이 흰 줄"을 없애는 에디터 도구.
/// - 각 스프라이트 영역(Rect)을 가운데 기준 TileSize×TileSize로 줄여서 반투명 테두리를 잘라낸다.
/// - PPU를 TileSize로 맞춰서 모든 타일이 정확히 1칸(1 unit)이 되게 한다.
/// - Alpha Source를 None으로 바꿔서 타일 안쪽의 반투명 점까지 불투명하게 만든다.
/// 원본 PNG는 건드리지 않고 .meta(임포트 설정)만 바꾼다. 스프라이트 ID가 유지되므로 이미 칠한 맵은 그대로 동작한다.
/// 사용법: Project 창에서 타일시트 PNG 우클릭 → CoreKeeper → 타일 스프라이트 정리
/// </summary>
public static class TileSpriteTrimmer
{
    // 잘라낸 뒤 타일 한 장의 픽셀 크기 (= PPU). 줄이 남으면 90, 88로 낮춰서 다시 실행
    const int TileSize = 92;

    const string MenuPath = "Assets/CoreKeeper/타일 스프라이트 정리 (테두리 제거)";

    [MenuItem(MenuPath)]
    static void Trim()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer == null || importer.spriteImportMode != SpriteImportMode.Multiple)
        {
            EditorUtility.DisplayDialog("타일 정리", "Sprite Mode가 Multiple인 텍스처를 선택해야 합니다.", "확인");
            return;
        }

        bool ok = EditorUtility.DisplayDialog(
            "타일 정리",
            $"{path}\n\n모든 스프라이트를 {TileSize}×{TileSize}로 줄이고 PPU = {TileSize}, Alpha Source = None으로 바꿉니다.\n되돌리기(Ctrl+Z)가 안 되니 커밋해두고 진행하세요.",
            "진행", "취소");
        if (!ok) return;

        // 스프라이트 영역 데이터를 다루는 공식 API (Sprite Editor 창이 쓰는 것과 같다)
        var factory = new SpriteDataProviderFactories();
        factory.Init();
        var provider = factory.GetSpriteEditorDataProviderFromObject(importer);
        provider.InitSpriteEditorDataProvider();

        SpriteRect[] rects = provider.GetSpriteRects();
        int changed = 0;

        foreach (SpriteRect sr in rects)
        {
            Rect r = sr.rect;

            // 이미 TileSize보다 작은 스프라이트는 건드리지 않음
            if (r.width < TileSize || r.height < TileSize)
            {
                Debug.LogWarning($"[TileSpriteTrimmer] {sr.name}: {r.width}x{r.height} — {TileSize}보다 작아서 건너뜀");
                continue;
            }

            // 원래 영역의 중심은 그대로 두고, 크기만 TileSize로 줄인다 → 사방 테두리가 잘려나감
            float cx = r.x + r.width * 0.5f;
            float cy = r.y + r.height * 0.5f;
            sr.rect = new Rect(
                Mathf.Round(cx - TileSize * 0.5f),
                Mathf.Round(cy - TileSize * 0.5f),
                TileSize, TileSize);

            // 피벗 가운데 → Tilemap 칸 중앙에 딱 맞게 놓인다
            sr.alignment = SpriteAlignment.Center;
            sr.pivot = new Vector2(0.5f, 0.5f);
            changed++;
        }

        provider.SetSpriteRects(rects);
        provider.Apply();

        importer.spritePixelsPerUnit = TileSize;                       // 92px = 1칸
        importer.alphaSource = TextureImporterAlphaSource.None;        // 반투명 무시 → 완전 불투명
        importer.SaveAndReimport();

        Debug.Log($"[TileSpriteTrimmer] {path}: 스프라이트 {changed}개 정리 완료 (Size {TileSize}, PPU {TileSize}, Alpha None)");
    }

    // 텍스처를 선택했을 때만 메뉴가 활성화되게
    [MenuItem(MenuPath, true)]
    static bool Validate() => Selection.activeObject is Texture2D;
}
