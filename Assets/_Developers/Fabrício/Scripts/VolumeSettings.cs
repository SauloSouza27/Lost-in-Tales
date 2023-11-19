using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer volumeMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("audioVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetAudioVolume();
        }
    }

    public void SetAudioVolume()
    {
        float volume = volumeSlider.value;
        volumeMixer.SetFloat("audio", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("audioVolume", volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("audioVolume");
        SetAudioVolume(); 
    }
}
