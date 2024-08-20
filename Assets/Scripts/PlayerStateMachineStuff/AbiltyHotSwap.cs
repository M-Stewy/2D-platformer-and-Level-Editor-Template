using UnityEngine;
using UnityEngine.EventSystems;

public class AbiltyHotSwap : MonoBehaviour
{
    [SerializeField] string AbilName;
    Player _player;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();

        EventTrigger trig = GetComponentInChildren<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entryexit = new EventTrigger.Entry();


        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => _player.HotSwapAbilty(AbilName));
        trig.triggers.Add(entry);

        entryexit.eventID = EventTriggerType.PointerExit;
        entryexit.callback.AddListener((eventData) => _player.HotSwapAbilty("None"));
        trig.triggers.Add(entryexit);
    }

}
