using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip jump;
    public AudioClip run;
    public AudioClip die;
    public AudioClip hit;
    public AudioClip atack;
    public AudioClip win;
    public AudioClip takecoin;

    [Header("Master Volumes")]
    [Range(0f, 1f)] public float masterMusicVolume = 1f;
    [Range(0f, 1f)] public float masterSfxVolume = 1f;

    [Header("SFX Volumes (per sound)")]
    [Range(0f, 1f)] public float jumpVol = 1f;
    [Range(0f, 1f)] public float runVol = 0.6f;
    [Range(0f, 1f)] public float dieVol = 1f;
    [Range(0f, 1f)] public float hitVol = 0.8f;
    [Range(0f, 1f)] public float attackVol = 0.9f;
    [Range(0f, 1f)] public float winVol = 1f;
    [Range(0f, 1f)] public float coinVol = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.volume = masterMusicVolume;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume * masterSfxVolume);
    }

    // Gọi nhanh theo tên
    public void SFX_Jump() => PlaySFX(jump, jumpVol);
    public void SFX_Run() => PlaySFX(run, runVol);
    public void SFX_Die() => PlaySFX(die, dieVol);
    public void SFX_Hit() => PlaySFX(hit, hitVol);
    public void SFX_Attack() => PlaySFX(atack, attackVol);
    public void SFX_Win() => PlaySFX(win, winVol);
    public void SFX_TakeCoin() => PlaySFX(takecoin, coinVol);


    public void SetMusicVolume(float v)
    {
        masterMusicVolume = Mathf.Clamp01(v);
        if (musicSource != null) musicSource.volume = masterMusicVolume;
    }

    public void SetSfxVolume(float v)
    {
        masterSfxVolume = Mathf.Clamp01(v);
        // với SFX dùng PlayOneShot nên không set trực tiếp lên sfxSource.volume,
        // mà masterSfxVolume sẽ nhân vào mỗi lần PlaySFX.
    }

    public void MuteAll(bool muted)
    {
        if (muted)
        {
            if (musicSource) musicSource.volume = 0f;
        }
        else
        {
            if (musicSource) musicSource.volume = masterMusicVolume;
        }
    }

}
