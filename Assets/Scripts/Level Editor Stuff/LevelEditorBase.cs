using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace LevelEditor 
{ 
    public class LevelEditorBase : MonoBehaviour
    {
        [SerializeField] Tilemap DefaultTileMap;
        Tilemap currentTilemap
        {
            get
            {
                if (LevelEditorLoadAndSave.Instance.tilelayers.TryGetValue((int)LevelEditorLoadAndSave.Instance.tileScritList[tileIndex].tilemap, out Tilemap tilemap))
                {
                    return tilemap;
                }
                else
                {
                    return DefaultTileMap;
                }
            }
        }
        TileBase currentTile
        {
            get
            {
                return LevelEditorLoadAndSave.Instance.tileScritList[tileIndex].tile;
            }
        }

        Vector3Int Pos;
        [SerializeField] Camera cam;
        public int tileIndex;

        public void SelectTile(int tileNum)
        {
            if (tileNum >= LevelEditorLoadAndSave.Instance.tileScritList.Count || tileNum < 0)
            {
                Debug.LogWarning("tile Index out of Bounds");
            }
            else
            tileIndex = tileNum;
        }

        private void NextTile()
        {
            Debug.Log("ASD");
            tileIndex++;
            if (tileIndex >= LevelEditorLoadAndSave.Instance.tileScritList.Count)
                tileIndex = 0;
        }
        private void PrevTile()
        {
            tileIndex--;
            if(tileIndex < 0 )
                tileIndex = LevelEditorLoadAndSave.Instance.tileScritList.Count - 1;
        }

        private void PlaceTile(Vector3Int pos)
        {
            currentTilemap.SetTile(pos, currentTile);
            //need to figure out how to drag and place....
        }

        private void DeleteTile(Vector3Int pos)
        {
            currentTilemap.SetTile(pos, null);
        }

        public void OnSelectNext(InputAction.CallbackContext context)
        {
            if (context.started)
                NextTile();
        }

        public void OnSelectPrev(InputAction.CallbackContext context)
        {
            if(context.started)
                PrevTile();
        }

        public void OnPlace(InputAction.CallbackContext context)
        {
            if (context.started)
                PlaceTile(Pos);
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
                DeleteTile(Pos); 
        }

        public void OnMoveMouse(InputAction.CallbackContext context)
        {
            Pos = currentTilemap.WorldToCell(context.ReadValue<Vector2>());
            Pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Pos));
        }

    }
}