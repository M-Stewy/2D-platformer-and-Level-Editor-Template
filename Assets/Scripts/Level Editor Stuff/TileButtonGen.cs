using LevelEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Automatically generates buttons for the level editor based off the tileScriptableObjects in the "CustomTileScritObjs" folder
/// </summary>
public class TileButtonGen : MonoBehaviour
{
    [SerializeField] GameObject buttonPreFab;


    List <CustomTileScritObj> tileScritList = new List<CustomTileScritObj>();
    void Awake()
    {
        string[] fileNames = AssetDatabase.FindAssets("Tile", new[] { "Assets/Scripts/Level Editor Stuff/CustomTileScritObjs" });
        Debug.Log(fileNames.Length);

        foreach (string file in fileNames)
        {
            Debug.Log("Found asset: " + file);
            var path = AssetDatabase.GUIDToAssetPath(file);
            var tile = AssetDatabase.LoadAssetAtPath<CustomTileScritObj>(path);
            tileScritList.Add(tile);
        }
    }

    private void Start()
    {
        foreach (var tile in tileScritList)
        {
            GameObject button = Instantiate(buttonPreFab, transform);
            button.GetComponent<Button>().onClick.AddListener(() =>
            FindAnyObjectByType<LevelEditorBase>().SelectTile(tile.numID)
            );
            button.GetComponentsInChildren<Image>()[1].sprite = tile.image; //gets the image of the child obj (for some reason the first child is itself?? not sure why
           EventTrigger trig = button.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            EventTrigger.Entry entryexit = new EventTrigger.Entry();


            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => FindAnyObjectByType<LevelEditorBase>().CanPlace(false) );
            trig.triggers.Add(entry);

            entryexit.eventID = EventTriggerType.PointerExit;
            entryexit.callback.AddListener((eventData) => FindAnyObjectByType<LevelEditorBase>().CanPlace(true));
            trig.triggers.Add(entryexit);

        }

    }

}
