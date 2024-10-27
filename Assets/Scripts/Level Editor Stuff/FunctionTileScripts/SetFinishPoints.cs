using UnityEngine;
using UnityEngine.Tilemaps;

public class SetFinishPoints : GeneralFunctionTile
{
    Tilemap spawnPointMap;
    [SerializeField] GameObject finishLine;
    
    private void Start()
    {
        spawnPointMap = GetComponent<Tilemap>();
        base.CheckForTiles(spawnPointMap);
    }
    public override void CheckForTiles(Tilemap tileM)
    {
        base.CheckForTiles(tileM);
    }

    public override void CustomTileFunc(Vector3 vect)
    {
        if (vect == Vector3.zero) Debug.Log("No End Goal Set");
        else Instantiate(finishLine, vect, Quaternion.identity, transform);
    }
}
