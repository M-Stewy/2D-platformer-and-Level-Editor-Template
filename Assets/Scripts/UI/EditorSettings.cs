using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LevelEditor { 
    public class EditorSettings : MonoBehaviour
    {
        LevelEditorLoadAndSave lvlEdtor;
        [SerializeField] GameObject buttonPrefab;
        [SerializeField] GameObject BackGroundButtonHolder;


        void OnEnable()
        {
            if(!DataStorage.Instance.isEditing)  return; 
            lvlEdtor = FindAnyObjectByType<LevelEditorLoadAndSave>();
            Debug.Log("Editor settings started!");
            GenerateBackGroundButtons();
        }

        private void OnDisable()
        {
            if (!DataStorage.Instance.isEditing) return;
            foreach(BG_ButtonRemoveSelf child in GetComponentsInChildren<BG_ButtonRemoveSelf>() ) // this is scuffed as hell man
            {
                Destroy(child.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!DataStorage.Instance.isEditing)  return; 
        }



        void GenerateBackGroundButtons()
        {
            foreach (var backgrounds in lvlEdtor.BackGroundSprites)
            {
                Debug.Log(backgrounds.Value.name);
                GameObject button = Instantiate(buttonPrefab, BackGroundButtonHolder.transform);
                button.GetComponent<Button>().onClick.AddListener(() => lvlEdtor.setBackGroundImage(backgrounds.Key));
                button.GetComponentsInChildren<Image>()[1].sprite = backgrounds.Value;//gets the image of the child obj (for some reason the first child is itself?? not sure why
                EventTrigger trig = button.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                EventTrigger.Entry entryexit = new EventTrigger.Entry();

                /*
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) =>  );
                trig.triggers.Add(entry);

                entryexit.eventID = EventTriggerType.PointerExit;
                entryexit.callback.AddListener((eventData) => );
                trig.triggers.Add(entryexit);
                */
            }
        }



    }
}
