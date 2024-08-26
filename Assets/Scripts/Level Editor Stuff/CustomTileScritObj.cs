using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// The main fileType for tiles in the tilemap
/// </summary>
[CreateAssetMenu(fileName = "New CustomeTile",menuName = "LevelEditor/Tile")]
public class CustomTileScritObj : ScriptableObject
{
    public string id;
    public TileBase tile;
    public Sprite image;

    public LevelEditor.LevelEditorLoadAndSave.TileMaps tilemap;
    public LevelEditor.LevelEditorLoadAndSave.Categories drawCategory;
    
    public int numID;
}
