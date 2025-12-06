using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingPanel;
    public GameObject aboutPanel;

    [Header("Audio Settings")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Toggle muteToggle;

    private bool isMuted = false;

    void Start()
    {
        // Gán giá trị ban đầu cho slider nếu có
        if (volumeSlider != null)
        {
            float volume;
            if (audioMixer.GetFloat("MasterVolume", out volume))
            {
                volumeSlider.value = Mathf.Pow(10, volume / 20); // convert từ dB sang 0–1
            }
        }

        if (settingPanel != null) settingPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(false);
    }

    // ---------------- MAIN MENU ----------------
    public void PlayGame()
    {
        SceneManager.LoadScene("map 1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    // ---------------- SETTING MENU ----------------
    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", dB);
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        if (mute)
            audioMixer.SetFloat("MasterVolume", -80f); // tắt âm
        else
            SetVolume(volumeSlider.value);
    }

    // ---------------- ABOUT MENU ----------------
    public void OpenAbout()
    {
        aboutPanel.SetActive(true);
    }

    public void CloseAbout()
    {
        aboutPanel.SetActive(false);
    }
}
