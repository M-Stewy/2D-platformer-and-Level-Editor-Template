using UnityEngine;
using TMPro;

public class abilityDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] Player _player;

    private void Update()
    {
        _text.text = _player.CurrentAbility.name;
    }
}
