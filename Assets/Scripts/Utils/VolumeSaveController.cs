using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Button muteButton = null;
    [SerializeField] private GameObject unmuteSprite = null;
    [SerializeField] private GameObject muteSprite = null;

    private float previousVolume = 1f;

    private void Start()
    {
        LoadValues();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteButton.onClick.AddListener(OnMuteButtonClicked);
    }

    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        muteButton.onClick.RemoveListener(OnMuteButtonClicked);
    }

    private void OnVolumeChanged(float value)
    {
        if (!PlayerPrefs.HasKey("IsMuted") || !PlayerPrefs.GetInt("IsMuted", 0).Equals(1))
        {
            AudioListener.volume = value;
        }

        PlayerPrefs.SetFloat("VolumeValue", value);
        PlayerPrefs.Save();
    }

    private void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue", 1f);
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        volumeSlider.value = volumeValue;
        previousVolume = volumeValue;

        if (isMuted)
        {
            AudioListener.volume = 0f;
            UpdateMuteButtonUI(true);
        }
        else
        {
            AudioListener.volume = volumeValue;
            UpdateMuteButtonUI(false);
        }
    }

    private void OnMuteButtonClicked()
    {
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        if (isMuted)
        {
            // Desactivar mute
            AudioListener.volume = previousVolume;
            PlayerPrefs.SetInt("IsMuted", 0);
            UpdateMuteButtonUI(false);
        }
        else
        {
            // Activar mute
            previousVolume = AudioListener.volume;
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("IsMuted", 1);
            UpdateMuteButtonUI(true);
        }

        PlayerPrefs.Save();
    }

    private void UpdateMuteButtonUI(bool isMuted)
    {
        muteSprite.SetActive(isMuted);
        unmuteSprite.SetActive(!isMuted);
    }
}