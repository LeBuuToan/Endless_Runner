using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Image SettingImage;
    public Image BestScoreImage;
    public Image CreditImage;
    public Image VolumeImage;

    public Text scoreText;

    public AudioSource myAudio;
    //public Slider volumeSlider;
    public Scrollbar volumeScrollbar;

    public float bestScore;

    private Animator animMenu;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        myAudio = GetComponent<AudioSource>();
        animMenu = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bestScore = PlayerPrefs.GetFloat("My Score");
        scoreText.text = bestScore.ToString();
        //myAudio.volume = volumeSlider.value;
        myAudio.volume = volumeScrollbar.value;
    }

    public void Setting()
    {
        SettingImage.gameObject.SetActive(true);
    }

    public void Exit()
    {
        SettingImage.gameObject.SetActive(false);
    }

    public void BestScore()
    {
        BestScoreImage.gameObject.SetActive(true);
    }

    public void ExitBestScore()
    {
        BestScoreImage.gameObject.SetActive(false);
    }

    public void Credit()
    {
        CreditImage.gameObject.SetActive(true);
    }

    public void ExitCredit()
    {
        CreditImage.gameObject.SetActive(false);
    }

    public void Volume()
    {
        VolumeImage.gameObject.SetActive(true);
    }

    public void ExitVolume()
    {
        VolumeImage.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    } 
}
