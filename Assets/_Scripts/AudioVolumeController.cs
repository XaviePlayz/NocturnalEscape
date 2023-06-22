using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioVolumeController : MonoBehaviour
{
    [Header("Volume")]
    public Slider volumeSlider;
    private const string VolumeKey = "Volume";

    [Header("Scenes")]
    public GameObject game;
    public GameObject options;
    public GameObject houseSettings;
    public GameObject noiseMeter;
    float gameVolume;

    private void Start()
    {
        // Load Game
        game.SetActive(true);
        noiseMeter.SetActive(true);
        options.SetActive(false);
        houseSettings.SetActive(false);

        // Load the saved volume value
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Attach a listener to the slider's OnValueChanged event
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (game.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.None;
                game.SetActive(false);
                noiseMeter.SetActive(false);
                options.SetActive(true);
                houseSettings.SetActive(true);
                SetVolume(gameVolume);

                // Load the saved volume value
                float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
                volumeSlider.value = savedVolume;
                SetVolume(savedVolume);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                game.SetActive(true);
                noiseMeter.SetActive(true);
                options.SetActive(false);
                houseSettings.SetActive(false);
                SetVolume(gameVolume);
            }
        }
    }

    public void ReturnToGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        game.SetActive(true);
        noiseMeter.SetActive(true);
        options.SetActive(false);
        houseSettings.SetActive(false);
        SetVolume(gameVolume);
    }

    public void OnVolumeChanged(float volume)
    {
        // Set the volume for all audio sources
        SetVolume(volume);
        gameVolume = volume;

        // Save the volume value
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        // Find all instances of AudioSource in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Set the volume for all audio sources
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
