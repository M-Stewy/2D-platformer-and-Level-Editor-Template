using UnityEngine;
using TMPro;

public class abilityDisplay : MonoBehaviour
{
    private TMP_Text _text;
    Player _player;
    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        _text.text = _player.CurrentAbility.name;
    }
}
