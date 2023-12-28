using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // Load graphics settings and update UI
        LoadGraphicsSettings();

        if (resolutionDropdown != null)
        {
            // Initialize resolution options
            InitializeResolutionOptions();
        }
        else
        {
            Debug.LogError("Resolution Dropdown is not assigned.");
        }

        // Initialize quality options
        InitializeQualityOptions();
    }

    // Function to handle fullscreen toggle change
    public void OnFullscreenToggleChanged()
    {
        // Set fullscreen mode immediately
        Screen.fullScreen = fullscreenToggle.isOn;

        // Save graphics settings
        SaveGraphicsSettings();
    }

    // Function to handle resolution dropdown change
    public void OnResolutionDropdownChanged()
    {
        string[] resolutionString = resolutionDropdown.options[resolutionDropdown.value].text.Split('x');
        int width = int.Parse(resolutionString[0]);
        int height = int.Parse(resolutionString[1]);

        // Set resolution immediately
        Screen.SetResolution(width, height, Screen.fullScreen);

        // Save graphics settings
        SaveGraphicsSettings();
    }

    // Function to handle quality dropdown change
    public void OnQualityDropdownChanged()
    {
        // Set quality immediately
        QualitySettings.SetQualityLevel(qualityDropdown.value, true);

        // Save graphics settings
        SaveGraphicsSettings();
    }

    // Function to save graphics settings
    private void SaveGraphicsSettings()
    {
        Debug.Log("Saving Graphics Settings");

        int fullscreenValue = Screen.fullScreen ? 1 : 0;
        PlayerPrefs.SetInt("Fullscreen", fullscreenValue);

        string resolutionText = resolutionDropdown.options[resolutionDropdown.value].text;
        PlayerPrefs.SetString("Resolution", resolutionText);

        int qualityValue = qualityDropdown.value;
        PlayerPrefs.SetInt("Quality", qualityValue);

        // Save PlayerPrefs immediately
        PlayerPrefs.Save();
    }

    // Function to load graphics settings
    private void LoadGraphicsSettings()
    {
        Debug.Log("Loading Graphics Settings");

        // Set fullscreen toggle
        int fullscreenValue = PlayerPrefs.GetInt("Fullscreen", 1);
        fullscreenToggle.isOn = fullscreenValue == 1;

        // Set resolution dropdown
        string savedResolution = PlayerPrefs.GetString("Resolution", "");
        if (!string.IsNullOrEmpty(savedResolution))
        {
            int index = FindResolutionIndex(savedResolution);
            if (index != -1)
            {
                resolutionDropdown.value = index;
            }
            else
            {
                Debug.LogWarning($"Saved resolution {savedResolution} not found in options.");
            }
        }
        else
        {
            Debug.LogWarning("Saved resolution is empty or null.");
        }

        // Set quality dropdown
        int savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQuality;
    }

    // Function to find the index of the saved resolution in the dropdown options
    private int FindResolutionIndex(string savedResolution)
    {
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == savedResolution)
            {
                return i;
            }
        }
        return -1;
    }

    // Function to initialize resolution options
    private void InitializeResolutionOptions()
    {
        // Populate resolution options
        resolutions = GetCommonResolutions();
        resolutionDropdown.ClearOptions();
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.width + "x" + resolution.height));
        }

        // Set default value in the dropdown
        resolutionDropdown.value = GetIndexOfCurrentResolution();
        resolutionDropdown.RefreshShownValue();
    }

    // Function to initialize quality options
    private void InitializeQualityOptions()
    {
        // Populate quality options
        qualityDropdown.ClearOptions();
        string[] qualityNames = QualitySettings.names;
        foreach (string qualityName in qualityNames)
        {
            qualityDropdown.options.Add(new Dropdown.OptionData(qualityName));
        }
        qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    // Function to get the index of the current resolution in the dropdown
    private int GetIndexOfCurrentResolution()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return resolutions.Length - 1;
    }

    // Function to get common resolutions
    private Resolution[] GetCommonResolutions()
    {
        // Define a list of common resolutions
        Resolution[] commonResolutions = new Resolution[]
        {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1024, height = 768 },
            new Resolution { width = 800, height = 600 }
        };

        return commonResolutions;
    }
}
