using UnityEngine;

public class AbilHSMenuUI : MonoBehaviour
{
    [SerializeField] GameObject daMenu;

    public void OpenMenu()
    {
        daMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        daMenu.SetActive(false);
    }

}
