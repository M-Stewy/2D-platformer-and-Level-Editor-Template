using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings
{
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

