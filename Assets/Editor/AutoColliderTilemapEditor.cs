using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class AutoColliderTilemapEditor : EditorWindow
{
    Tilemap mainTilemap; // 기본 타일맵
    Tilemap colliderTilemap; // 콜라이더를 추가할 타일맵
    TileBase colliderTile; // 콜라이더에 사용할 타일
    MonoScript selectedColliderScript;  // 콜라이더 컨트롤 스크립트

    [MenuItem("Tools/Tilemap/Auto Collider Tilemap")]
    public static void ShowWindow()
    {
        var window = GetWindow<AutoColliderTilemapEditor>("Auto Collider Tilemap");
        window.minSize = new Vector2(300, 120);
        window.maxSize = new Vector2(300, 120);
    }

    void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        mainTilemap = EditorGUILayout.ObjectField("Main Tilemap", mainTilemap, typeof(Tilemap), true) as Tilemap;
        colliderTilemap = EditorGUILayout.ObjectField("Collider Tilemap", colliderTilemap, typeof(Tilemap), true) as Tilemap;
        colliderTile = EditorGUILayout.ObjectField("Collider Tile", colliderTile, typeof(TileBase), false) as TileBase;
        selectedColliderScript = EditorGUILayout.ObjectField("Collider Controller Script", selectedColliderScript, typeof(MonoScript), false) as MonoScript;

        if (GUILayout.Button("Generate Collider Tilemap"))
        {
            GenerateColliderTilemap();
        }
    }

    void GenerateColliderTilemap()
    {
        if (mainTilemap == null || colliderTilemap == null || colliderTile == null)
        {
            Debug.LogWarning("Please assign main tilemap, collider tilemap, and collider tile.");
            return;
        }

        // Collider Tilemap에 Tilemap Collider 2D, Composite Collider 2D 및 Rigidbody 2D 컴포넌트 추가
        SetColliderComponents();

        colliderTilemap.ClearAllTiles(); // 콜라이더 타일맵 초기화

        BoundsInt bounds = mainTilemap.cellBounds;
        TileBase[] allTiles = mainTilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    // 현재 타일이 존재하면 꼭지점과 모서리를 포함하여 주변에 콜라이더 타일을 배치
                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        for (int yOffset = -1; yOffset <= 1; yOffset++)
                        {
                            TryPlaceCollider(x + bounds.xMin + xOffset, y + bounds.yMin + yOffset);
                        }
                    }
                }
            }
        }
    }

    void TryPlaceCollider(int x, int y)
    {
        Vector3Int tilePosition = new Vector3Int(x, y, 0);
        if (mainTilemap.GetTile(tilePosition) == null && colliderTilemap.GetTile(tilePosition) == null)
        {
            // 메인 타일맵과 콜라이더 타일맵 양쪽에 타일이 없는 경우에만 콜라이더 타일 배치
            colliderTilemap.SetTile(tilePosition, colliderTile);
        }
    }

    void SetColliderComponents()
    {
        // 콜라이더 컨트롤러 스트립트 추가
        if (colliderTilemap != null && selectedColliderScript != null)
        {
            if (colliderTilemap.gameObject.GetComponent(selectedColliderScript.GetClass()) == null)
            {
                colliderTilemap.gameObject.AddComponent(selectedColliderScript.GetClass());
            }
        }

        // TilemapRenderer의 order in layer 값 수정
        var tilemapRenderer = colliderTilemap.GetComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = 2;

        // Tilemap Collider 2D 추가
        if (colliderTilemap.GetComponent<TilemapCollider2D>() == null)
        {
            var tilemapCollider = colliderTilemap.gameObject.AddComponent<TilemapCollider2D>();
            tilemapCollider.usedByComposite = true;
        }

        // Rigidbody 2D 추가
        if (colliderTilemap.GetComponent<Rigidbody2D>() == null)
        {
            var rigidbody = colliderTilemap.gameObject.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Static;
        }

        // Composite Collider 2D 추가
        if (colliderTilemap.GetComponent<CompositeCollider2D>() == null)
        {
            var compositeCollider = colliderTilemap.gameObject.AddComponent<CompositeCollider2D>();
            compositeCollider.usedByComposite = true;
            compositeCollider.offset = new Vector2(0, 0.125f);
        }
    }
}
