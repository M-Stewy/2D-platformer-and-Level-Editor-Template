using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New CustomeTile",menuName = "LevelEditor/Tile")]
public class CustomTileScritObj : ScriptableObject
{
    public string id;
    public TileBase tile;
    public Sprite image;

    //    [Space]

        public LevelEditor.LevelEditorLoadAndSave.TileMaps tilemap;
        public LevelEditor.LevelEditorLoadAndSave.Categories drawCategory;
}
