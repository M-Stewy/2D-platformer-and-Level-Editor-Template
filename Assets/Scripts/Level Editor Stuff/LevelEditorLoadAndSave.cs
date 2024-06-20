using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.InputSystem;

namespace LevelEditor { 
    public class LevelEditorLoadAndSave : MonoBehaviour
    {
        public static LevelEditorLoadAndSave Instance;
        private void Awake()
        { 
            if (Instance == null) Instance = this;
            else Destroy(this);

            foreach (Tilemap tilemap in tilemaps)
            {
                foreach (TileMaps num in System.Enum.GetValues(typeof(TileMaps)))
                {
                    if (tilemap.name == num.ToString())
                    {
                        if (!tilelayers.ContainsKey((int)num)) tilelayers.Add((int)num, tilemap);
                    }
                }
            }
        }

        public List<CustomTileScritObj> tileScritList = new List<CustomTileScritObj>();
        [SerializeField] List<Tilemap> tilemaps = new List<Tilemap>();
        public Dictionary<int,Tilemap> tilelayers = new Dictionary<int,Tilemap>();



        public enum TileMaps //These need to the exact same name as the ones in the scene view
        {
            FarBackground = -20,
            Background = -10,
            Floor = 10,
            Grapple1 = 20,
            Grapple2 = 22,

        }

        public enum Categories
        {
            Background = -20,
            Ground = 0,
            Foreground = 20
        }

        public void OnQSave(InputAction.CallbackContext context)
        {
            if (context.started)
                SaveLevel();
        }
        public void OnLoad(InputAction.CallbackContext context)
        {
            if (context.started)
                LoadLevel();
        }

        void SaveLevel()
        {
            //create a new leveldata
            LevelData levelData = new LevelData();

            //set up the layers in the leveldata
            foreach (var item in tilelayers.Keys)
            {
                levelData.layers.Add(new LayerData(item));
            }

            foreach (var layerData in levelData.layers)
            {
                if (!tilelayers.TryGetValue(layerData.layer_id, out Tilemap tilemap)) break;

                //get the bounds of the tilemap
                BoundsInt bounds = tilemap.cellBounds;

                //loop trougth the bounds of the tilemap
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        //get the tile on the position
                        TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));
                        //find the temp tile in the custom tiles list
                        CustomTileScritObj temptile = tileScritList.Find(t => t.tile == temp);

                        //if there's a customtile associated with the tile
                        if (temptile != null)
                        {
                            //add the values to the leveldata
                            layerData.tiles.Add(temptile.id);
                            layerData.xPos.Add(x);
                            layerData.yPos.Add(y);
                        }
                    }
                }

            }

            //save the data as a json
            string json = JsonUtility.ToJson(levelData, true);
            File.WriteAllText(Application.dataPath + "/testLevel.json", json);//TODO get it to have a custom name for the file

            //debug
            Debug.Log("Level was saved");
        }

        void LoadLevel()
        {
            //load the json file to a leveldata


            string json = File.ReadAllText(Application.dataPath + "/testLevel.json");
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            foreach (var data in levelData.layers)
            {
                if (!tilelayers.TryGetValue(data.layer_id, out Tilemap tilemap)) break;

                //clear the tilemap
                tilemap.ClearAllTiles();

                //place the tiles
                for (int i = 0; i < data.tiles.Count; i++)
                {
                    TileBase tile = tileScritList.Find(t => t.id == data.tiles[i]).tile;
                    if (tile) tilemap.SetTile(new Vector3Int(data.xPos[i], data.yPos[i], 0), tile);
                }
            }

            //debug
            Debug.Log("Level was loaded");
        }

    }

    [System.Serializable]
    public class LevelData
    {
        public List<LayerData> layers = new List<LayerData>();
    }

    [System.Serializable]
    public class LayerData
    {
        public int layer_id;
        public List<string> tiles = new List<string>();
        public List<int> xPos = new List<int>();
        public List<int> yPos = new List<int>();

        public LayerData(int id)
        {
            layer_id = id;
        }
    }
}
