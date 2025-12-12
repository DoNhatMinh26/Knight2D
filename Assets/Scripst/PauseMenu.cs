using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;   // nút góc phải (||)
    [SerializeField] private Button resumeButton;  // nút trong panel
    [SerializeField] private Button restartButton; // trong panel
    [SerializeField] private Button mainMenuButton;// trong panel
    [SerializeField] private Button quitButton;    // optional

    private bool isPaused = false;
    private float prevTimeScale = 1f;

    private void Awake()
    {
        if (pausePanel) pausePanel.SetActive(false);

        // gán sự kiện nếu có kéo tham chiếu
        if (pauseButton) pauseButton.onClick.AddListener(TogglePause);
        if (resumeButton) resumeButton.onClick.AddListener(Resume);
        if (restartButton) restartButton.onClick.AddListener(Restart);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(ToMainMenu);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        // Cho phép bấm ESC để pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;

        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;          // dừng gameplay
        AudioListener.pause = true;    // pause toàn bộ âm thanh

        if (pausePanel) pausePanel.SetActive(true);
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        Time.timeScale = prevTimeScale <= 0 ? 1f : prevTimeScale;
        AudioListener.pause = false;

        if (pausePanel) pausePanel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
