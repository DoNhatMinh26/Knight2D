using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("UI refs")]
    public Slider musicSlider;   // RowMusic/Slider
    public Slider sfxSlider;     // RowSFX/Slider
    public Toggle sfxToggle;     // RowNotification/Toggle (switch)

    const string KEY_MUSIC = "musicVol";
    const string KEY_SFX = "sfxVol";
    const string KEY_SFXON = "sfxOn";

    void OnEnable()
    {
        // 1) Load giá trị đã lưu (mặc định 1)
        float music = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);
        bool on = PlayerPrefs.GetInt(KEY_SFXON, 1) == 1;

        // 2) Set lên UI
        if (musicSlider) musicSlider.value = music;
        if (sfxSlider) sfxSlider.value = sfx;
        if (sfxToggle) sfxToggle.isOn = on;

        // 3) Áp vào AudioManager
        ApplyAll();
        // 4) Đăng ký lắng nghe thay đổi
        Hook(true);
    }

    void OnDisable()
    {
        Hook(false);
        PlayerPrefs.Save();
    }

    void Hook(bool add)
    {
        if (add)
        {
            if (musicSlider) musicSlider.onValueChanged.AddListener(OnMusicChanged);
            if (sfxSlider) sfxSlider.onValueChanged.AddListener(OnSfxChanged);
            if (sfxToggle) sfxToggle.onValueChanged.AddListener(OnSfxToggle);
        }
        else
        {
            if (musicSlider) musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            if (sfxSlider) sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
            if (sfxToggle) sfxToggle.onValueChanged.RemoveListener(OnSfxToggle);
        }
    }

    void OnMusicChanged(float v)
    {
        PlayerPrefs.SetFloat(KEY_MUSIC, v);
        if (AudioManager.Instance) AudioManager.Instance.SetMusicVolume(v);
    }

    void OnSfxChanged(float v)
    {
        PlayerPrefs.SetFloat(KEY_SFX, v);
        if (AudioManager.Instance)
        {
            // nếu đang ON thì áp liền, OFF thì chỉ lưu
            AudioManager.Instance.SetSfxVolume(sfxToggle && sfxToggle.isOn ? v : 0f);
        }
    }

    void OnSfxToggle(bool on)
    {
        PlayerPrefs.SetInt(KEY_SFXON, on ? 1 : 0);
        if (AudioManager.Instance)
        {
            if (on) AudioManager.Instance.SetSfxVolume(sfxSlider ? sfxSlider.value : 1f);
            else AudioManager.Instance.SetSfxVolume(0f);
        }
    }

    void ApplyAll()
    {
        if (!AudioManager.Instance) return;
        AudioManager.Instance.SetMusicVolume(musicSlider ? musicSlider.value : 1f);
        AudioManager.Instance.SetSfxVolume((sfxToggle == null || sfxToggle.isOn) ?
                                            (sfxSlider ? sfxSlider.value : 1f) : 0f);
    }
}
