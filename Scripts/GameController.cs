using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float spawnTime;

    float m_spawnTime;

    bool m_isGameover;

    UIManager m_ui;
    PlayerController m_player;

    public static bool GameIsPaused = false;
    
    public GameObject pauseMenuUI;

    int pausebtn = 0;

    public int score = 0;
    public Text scoreText;

    public Text bestScoreText;
    public float lastScore;

    bool m_pause;

    // Start is called before the first frame update
    void Start()
    {
        m_spawnTime = 0;
        m_ui = FindObjectOfType<UIManager>();
        lastScore = PlayerPrefs.GetFloat("My Score");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        if (m_isGameover)
        {
            m_spawnTime = 0;
            m_ui.showGameOverPanel(true);
            return;
        }

        m_spawnTime -= Time.deltaTime;

        if(m_spawnTime <= 0)
        {
            //SpawnEnemy();

            m_spawnTime = spawnTime;
        }
    }

    void FixedUpdate()
    {
        if(score > lastScore)
        {
            bestScoreText.text = "Best Score: " + score.ToString();
        }
        else
        {
            bestScoreText.text = "Your Score: " + score.ToString();
        }
    }

    //public void Replay()
    //{
        //SceneManager.LoadScene("GamePlay");
    //}

    public void SetScore(int value)
    {
        score = value;
    }

    public int GetScore()
    {
        return score;
    }

    public void ScoreIncrement()
    {
        if (m_isGameover)
            return;
        score += 5;
    }

    public void SetGameOverState(bool state)
    {
        m_isGameover = state;
    }

    public bool isGameover()
    {
        return m_isGameover;
    }

    public void SetPause(bool state)
    {
        m_pause = state;
    }

    public bool isPause()
    {
        return m_pause;
    }

    public void ResumeButton()
    {
        pausebtn = 0;
        //pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void PauseButton()
    {
        pausebtn = 1;
        //pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
