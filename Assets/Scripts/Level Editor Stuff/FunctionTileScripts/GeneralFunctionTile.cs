using UnityEngine;
using UnityEngine.Tilemaps;

public class GeneralFunctionTile : MonoBehaviour
{

    bool hasFoundTile;
    public virtual void CheckForTiles(Tilemap tileM)
    {
        BoundsInt bounds = tileM.cellBounds;
        TileBase[] alltiles = tileM.GetTilesBlock(bounds);
        Vector2 start = new Vector2(bounds.min.x, bounds.min.y);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = alltiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    CustomTileFunc(new Vector3(start.x + x, start.y + y, 0));
                    Debug.Log("Found SP at X: " + start.x + x + "  ,  Y: " + start.y + y);
                    hasFoundTile = true;
                }
            }
        }
        if (!hasFoundTile)
        {
            CustomTileFunc(Vector3.zero);
        }
    }

    public virtual void CustomTileFunc()
    {
        //This should be overriden in other functions
        Debug.LogWarning("This shouldnt be here...");
    }

    public virtual void CustomTileFunc(Vector3 vect)
    {
        //This should be overriden in other functions
        Debug.LogWarning("This shouldnt be here...");
    }

}
