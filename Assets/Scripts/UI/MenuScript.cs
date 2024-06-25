using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject LevelSelectScreen;
    [SerializeField]
    GameObject MainMenuScreen;

    [SerializeField] GameObject scrollSecetionContainer;
    [SerializeField] GameObject ButtonPreFab;

    bool editing;
    private void Awake()
    {
        if (!SceneManager.GetSceneByName("AlwaysActive").isLoaded)
            SceneManager.LoadSceneAsync("AlwaysActive",LoadSceneMode.Additive);
    }


    public void SelectLevel(string levelId)
    {
        Debug.Log(levelId);
        DataStorage.Instance.SetLevelToLoad(levelId);
        SceneManager.UnloadSceneAsync("MainMenu");

        if (!editing)
            SceneManager.LoadSceneAsync("EmptyScene", LoadSceneMode.Additive);
        else
            SceneManager.LoadSceneAsync("Level Editor", LoadSceneMode.Additive);
        
    }

    public void LevelSelector()
    {
        editing = false;
        DataStorage.Instance.isEditing = false;
        MainMenuScreen.SetActive(false);
        LevelSelectScreen.SetActive(true);

        loadLevelButtons();
    }

    public void EnterLevelEditor()
    {
        editing = true;
        DataStorage.Instance.isEditing = true;
        MainMenuScreen.SetActive(false);
        LevelSelectScreen.SetActive(true);

        loadLevelButtons();
    }

    public void OpenSettings()
    {
        Debug.Log("No, not yet");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        LevelSelectScreen.SetActive(false);
        MainMenuScreen.SetActive(true);
    }

    void loadLevelButtons()
    {
        foreach (string str in Directory.GetFiles(Application.dataPath + "/SaveFileFolder", "*.json"))
        {
            char[] chars = { '.', 'j', 's', 'o', 'n' };
            string tempString = str.Remove(0, (Application.dataPath + "/SaveFileFolder/").Length);
            tempString = tempString.TrimEnd(chars);

            GameObject levelSelectB = Instantiate(ButtonPreFab, scrollSecetionContainer.transform);
            levelSelectB.name = tempString;
            levelSelectB.GetComponentInChildren<TMP_Text>().text = tempString;

            levelSelectB.GetComponent<Button>().onClick.AddListener(() => SelectLevel(tempString));

            //create the buttons here
        }

    }
}
