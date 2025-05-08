using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;

    private void Start()
    {
        LoadValues();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged); 
    }

    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged); 
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("VolumeValue", value);
        PlayerPrefs.Save();
    }

    private void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue", 1f);
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}
