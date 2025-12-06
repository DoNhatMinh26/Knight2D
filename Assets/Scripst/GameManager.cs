using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject gameWinUi;
    private bool isGameOver = false;
    void Start()
    {
        UpdateScore();
        gameOverUi.SetActive(false);
        gameWinUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddScore(int points)
    {
        
        score += points;
        UpdateScore();
    }
    private void UpdateScore() { 
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        //ko nhận inputs
        Time.timeScale = 0;

        gameOverUi.SetActive(true);
    }
    public void GameWin()
    {
        score = 0;
        //ko nhận inputs
        Time.timeScale = 0;
        gameWinUi.SetActive(true);
    }
    public void Replay()
    {
        isGameOver = false;
        score = 0;
        UpdateScore();
        //ko nhận inputs
        Time.timeScale = 1;
        SceneManager.LoadScene("map 1");
    }
    public void MainMenu()
    {
        isGameOver = false;
        score = 0;
        UpdateScore();
        //ko nhận inputs
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

 
}
