using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu instance;
    private void Awake()
    {
        if(instance == null ) instance = this;
        else Destroy(this);
    }

    public bool inSettings;
    [SerializeField]
    private GameObject FullSettingsMenu;
    [SerializeField]
    GameObject[] Menus;
    [SerializeField] GameObject editorButtn;


    public void OpenSettingsMenu() {
        if (!DataStorage.Instance.isEditing)
             editorButtn.SetActive(false);
        else editorButtn.SetActive(true);

        inSettings = true;
        FullSettingsMenu.SetActive(true); 
    }
    public void CloseSettingsMenu() 
    { 
        inSettings = false;
        FullSettingsMenu.SetActive(false); 
    }


    public void ChangeSettingMenu(GameObject settingsMenu)
    {
        foreach (var menu in Menus)
            menu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
