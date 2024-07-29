using UnityEngine;
using UnityEngine.Tilemaps;

public class SetRespawnPoints : MonoBehaviour
{
    [SerializeField] Tilemap spawnPointMap;
    [SerializeField] GameObject SpawnerPrefab;
    bool hasFoundSpawnPoint;

    private void Start()
    {
        CheckForTiles();
    }

    private void CheckForTiles()
    {
        BoundsInt bounds = spawnPointMap.cellBounds;
        TileBase[] alltiles = spawnPointMap.GetTilesBlock(bounds);


        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = alltiles[x + y * bounds.size.x];
                if(tile != null)
                {
                    PlaceSpawnerOBJ(new Vector3(x, y));
                    Debug.Log("Found SP at X: " + x + "  ,  Y: " + y);
                    hasFoundSpawnPoint = true;
                }
            }
        }
        if (!hasFoundSpawnPoint)
        {
            PlaceSpawnerOBJ(Vector2.zero);
        }
    }


    void PlaceSpawnerOBJ(Vector3 pos) 
    {
        Instantiate(SpawnerPrefab, pos,Quaternion.identity);
    }
}
