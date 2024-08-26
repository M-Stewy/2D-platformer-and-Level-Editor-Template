using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings
{
    /// <summary>
    /// simply allows for controlling the volume via a slider in the menu
    /// should only need the music and SFX channels I think but its easy to add more
    /// </summary>
    public class AudioVolumeSliders : MonoBehaviour
    {
        [SerializeField] AudioMixerGroup mixerGroup;
        [SerializeField] AudioMixer MainMixer;
        [SerializeField ] Slider slider;

        void Start()
        {
            if(PlayerPrefs.HasKey(mixerGroup.name))  
                LoadVolume();
            SetMixerToSlider();
        }

        public void SetMixerToSlider()
        {
            float volume = slider.value;
            MainMixer.SetFloat(mixerGroup.name, Mathf.Log10(volume)*20);
            PlayerPrefs.SetFloat(mixerGroup.name,volume);
            Debug.Log("Setting volume of "+mixerGroup.name+" to "+ volume);
        }

        void LoadVolume()
        {
            slider.value = PlayerPrefs.GetFloat(mixerGroup.name);
        }
    }
}

