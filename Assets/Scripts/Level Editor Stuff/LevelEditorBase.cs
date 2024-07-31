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
        Vector3Int PrevPos;
        [SerializeField] Camera cam;
        public int tileIndex;

        bool _isPlacing;
        bool _isDeleting;


        private bool canPlace;
        public void CanPlace(bool y)
        {
            canPlace = y;
        }
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
            if (!canPlace) return;
            currentTilemap.SetTile(pos, currentTile);
        }

        private void DeleteTile(Vector3Int pos)
        {
            if (!canPlace) return;
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
            if (context.performed)
            {
                PlaceTile(Pos);
                _isPlacing = true;
            }else if(context.canceled)
            {
                _isPlacing = false;
            }
        }

        public void OnDelete(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DeleteTile(Pos);
                _isDeleting = true;
            }else if (context.canceled)
            {
                _isDeleting= false;
            }
        }

        public void OnMoveMouse(InputAction.CallbackContext context)
        {
            if(!canPlace) return;
            Pos = currentTilemap.WorldToCell(context.ReadValue<Vector2>());
            Pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Pos));
            
        }


        private void Update()
        {
            if (!_isDeleting && !_isPlacing) return;
            if(Pos != PrevPos)
            {
                if(_isPlacing)
                    PlaceTile(Pos);
                
                if(_isDeleting)
                    DeleteTile(Pos);
            }
        }

    }
}