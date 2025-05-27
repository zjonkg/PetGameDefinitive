using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        // Si ya existe una instancia, destruir esta para evitar duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Establecer esta como la instancia principal
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public float GetVolume()
    {
        return musicSource.volume;
    }
}
