using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// a script for making a tile a spawn point for the player
/// there might be better ways to do this, need to try some other things later
/// </summary>
public class SetRespawnPoints : GeneralFunctionTile
{
    Tilemap spawnPointMap;
    [SerializeField] GameObject SpawnerPrefab;

    private void Start()
    {
        spawnPointMap = GetComponent<Tilemap>();
        CheckForTiles(spawnPointMap);
    }

    public override void CheckForTiles(Tilemap tileM)
    {
        base.CheckForTiles(tileM);
    }

    public override void CustomTileFunc(Vector3 vect)
    {
        Instantiate(SpawnerPrefab, vect, Quaternion.identity, transform);
    }
    
}
