using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Button muteButton = null;
    [SerializeField] private GameObject unmuteSprite = null;
    [SerializeField] private GameObject muteSprite = null;
    [SerializeField] private MusicManager musicManager = null;

    private float previousVolume = 1f;

    private void Start()
    {
        if (musicManager == null)
        {
            musicManager = FindObjectOfType<MusicManager>();
        }

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
        previousVolume = value;

        if (!PlayerPrefs.HasKey("IsMuted") || PlayerPrefs.GetInt("IsMuted", 0) == 0)
        {
            musicManager.SetVolume(value);
        }

        PlayerPrefs.SetFloat("VolumeValue", value);
        PlayerPrefs.Save();
    }

    private void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue", 1f);
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        previousVolume = volumeValue;
        volumeSlider.value = volumeValue;

        musicManager.SetVolume(isMuted ? 0f : volumeValue);
        UpdateMuteButtonUI(isMuted);
    }

    private void OnMuteButtonClicked()
    {
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        if (isMuted)
        {
            musicManager.SetVolume(previousVolume);
            PlayerPrefs.SetInt("IsMuted", 0);
            UpdateMuteButtonUI(false);
        }
        else
        {
            previousVolume = musicManager.GetVolume();
            musicManager.SetVolume(0f);
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
