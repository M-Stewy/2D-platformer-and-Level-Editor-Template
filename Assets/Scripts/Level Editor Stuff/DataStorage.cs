using UnityEngine;

/// <summary>
/// use this to store data between all scenes
/// later I will probably add data that is saved between shutoff and startup but Ill get to that later
/// </summary>
public class DataStorage : MonoBehaviour
{
   public static DataStorage Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else Destroy(this);
    }

    public string levelToLoad { get; private set; }
    public bool isEditing;
    public void SetLevelToLoad(string ltl)
    {
        Debug.Log("What is going on?");
        levelToLoad = ltl;
    }
}
