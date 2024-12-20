using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEditor;

namespace LevelEditor {
    /// <summary>
    /// Probably the most important part of the project
    /// if you want to add your own tiles, this script will need a bit of modification (not too much tho)
    /// 
    /// much of this script is from: https://www.youtube.com/watch?v=snUe2oa_iM0
    /// </summary>
    public class LevelEditorLoadAndSave : MonoBehaviour
    {
        [SerializeField] public bool LoadOnly;

        GameObject Grid;

        public static LevelEditorLoadAndSave Instance;
        private void Awake()
        { 
            if (Instance == null) Instance = this;
            else Destroy(this);

            Grid = GameObject.FindGameObjectWithTag("GridHolder");
            GenTileScritList();
            GenTileMapList();
            GetBackgroundImgs();

            foreach (Tilemap tilemap in tilemaps)
            {
                foreach (TileMaps num in Enum.GetValues(typeof(TileMaps)))
                {
                    if (tilemap.name == num.ToString())
                    {
                        if (!tilelayers.ContainsKey((int)num)) tilelayers.Add((int)num, tilemap);
                    }
                }
            }

          

            if (DataStorage.Instance != null)
                foreach (string str in Directory.GetFiles(Application.dataPath + "/SaveFileFolder", "*.json"))
                {
                    string tempString = str.Remove(0, (Application.dataPath + "/SaveFileFolder/").Length);
                    tempString = tempString.TrimEnd(chars);
                    if (DataStorage.Instance.levelToLoad == tempString)
                    {
                        fileSaveName = DataStorage.Instance.levelToLoad;
                        LoadLevel();
                    }
                }
            else
                LoadLevel();

        }
        char[] chars = { '.', 'j', 's', 'o', 'n' };
        public List<CustomTileScritObj> tileScritList = new List<CustomTileScritObj>();
        [SerializeField] List<Tilemap> tilemaps = new List<Tilemap>();
        public Dictionary<int,Tilemap> tilelayers = new Dictionary<int,Tilemap>();
        [Header("---TEMP BACKGROUND TESTING---")]
        public Dictionary<string,Sprite> BackGroundSprites = new Dictionary<string,Sprite>();
        [SerializeField] string choosenBackGround;
        [SerializeField] BackGroundSetter backSetter;

        string fileSaveName = "Default";
        [SerializeField] TMP_InputField saveFileNameInput;

        
        private void GenTileScritList()
        {
            string[] fileNames = AssetDatabase.FindAssets("Tile", new[] { "Assets/Scripts/Level Editor Stuff/CustomTileScritObjs" });
            //Debug.Log(fileNames.Length);

            foreach (string file in fileNames)
            {
                //Debug.Log("Found TileScript asset: " + file);
                var path = AssetDatabase.GUIDToAssetPath(file);
                var tile = AssetDatabase.LoadAssetAtPath<CustomTileScritObj>(path);
                tileScritList.Add(tile);
            }
        }

        private void GenTileMapList()
        {
            foreach(var tilemap in Grid.GetComponentsInChildren<Tilemap>())
            {
                tilemaps.Add(tilemap);

            }

        }

        private void GetBackgroundImgs()
        {
            string[] fileNames = AssetDatabase.FindAssets("BG", new[] { "Assets/Sprites/BGs" });
            Debug.Log(fileNames.Length + " imgs found");

            foreach (string file in fileNames)
            {
                Debug.Log("Found IMG asset: " + file);
                var path = AssetDatabase.GUIDToAssetPath(file);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                Debug.Log("Found the sprite : " + sprite.name + "");
                BackGroundSprites.Add(sprite.name, sprite);
            }
            Debug.Log(BackGroundSprites.Count);
        }

        /// <summary>
        /// If you are adding custom tiles, make sure you add a corresponding enum to this
        /// </summary>
        public enum TileMaps //These need to the exact same name as the ones in the scene view
        {
            FarBackground = -20,
            Background = -10,
            Floor = 10,
            BadTile = 15,
            Grapple1 = 20,
            Grapple2 = 22,
            AntiGrapp = 23,
            RespawnTile = 100,
            EndTile = 101

        }
        /// <summary>
        /// This enum is unsued, but ideally it will help organize the tile buttons in the editor later
        /// </summary>
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

        public void SaveAs()
        {
            saveFileNameInput.gameObject.SetActive(true);
            
        }
        
        public void SavingAs()
        {
            fileSaveName = saveFileNameInput.text;
            Debug.Log(fileSaveName);
            SaveLevel();
            saveFileNameInput.gameObject.SetActive(false);
        }

        public void CanceledSaveAs()
        {
            saveFileNameInput.gameObject.SetActive(false);
        }

        public void SaveLevel()
        {
            if(LoadOnly)
            {
                Debug.Log("This should not appear :( ");
                return;
            }

            //create a new leveldata
            LevelData levelData = new LevelData();

            //set up the layers in the leveldata
            foreach (var item in tilelayers.Keys)
            {
                levelData.layers.Add(new LayerData(item));
            }

            levelData.backGround.Background = choosenBackGround;


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
            File.WriteAllText(Application.dataPath + "/SaveFileFolder/" + fileSaveName + ".json", json);
            if(!Directory.Exists(Application.dataPath + "/SaveFileFolder") )
            {
                Directory.CreateDirectory(Application.dataPath + "/SaveFileFolder");
            }
            //debug
            Debug.Log("Level was saved as " + fileSaveName);
        }

        public void LoadLevel()
        {
            //load the json file to a leveldata


            string json = File.ReadAllText(Application.dataPath + "/SaveFileFolder/" + fileSaveName + ".json");
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            GetBackGroundFromLevelData(levelData);

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

        public void GetLoadOptions()
        {
            foreach (string str in Directory.GetFiles(Application.dataPath + "/SaveFileFolder", "*.json")) 
            {
                char[] chars  = {'.','j','s','o','n' };
                string tempString = str.Remove(0,(Application.dataPath + "/SaveFileFolder/").Length);
                tempString = tempString.TrimEnd(chars);
                Debug.Log(tempString);
            }
        }

        public void GetBackGroundFromLevelData(LevelData lvdt)
        {
            if(lvdt.backGround.Background == null)
            {
                Debug.Log("No data to read Background from!");
                return;
            }
            if (BackGroundSprites.TryGetValue(lvdt.backGround.Background, out Sprite backGroundSprite))    
                backSetter.SetBackGround(backGroundSprite);
            else
            {
                backSetter.SetBackGround(null);
                Debug.LogError("NO BACKGROUND SET");
            }
        }

        public void setBackGroundImage(string bg)
        {
            choosenBackGround = bg;
        }

    }

   




    [Serializable]
    public class LevelData
    {
        public List<LayerData> layers = new List<LayerData>();
        public BackGroundData backGround = new BackGroundData();
    }

    [Serializable]
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

    [Serializable]
    public class BackGroundData
    {
        public string Background;
        public List<string> Foregrounds;
    }
}
