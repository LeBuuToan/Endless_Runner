using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    UIManager m_ui;
    GameController m_gc;

    public float spawnTime;

    float m_spawnTime;

    private Touch initialTouch = new Touch();
    private float distance = 0;
    private bool hasSwiped = false;

    public bool jump = false;

    public bool slide = false;

    public bool left = false;
    public bool right = false;

    private GameObject trigger;
    private Animator anim;

    public float score = 0;
    public Text scoreText;

    int oneJump;
    int oneSlide;
    int oneLeft;
    int oneRight;

    public bool boost = false;
    bool isBoost = false;
    public bool undie = false;
    public bool magnet = false;

    public bool death = false;

    public AudioSource myAudio;

    public AudioClip boostCollection;
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip deadSound;

    public Image gameOverImg;
    public Text bestScoreText;
    public float lastScore;

    public static bool GameIsPaused = false;
    
    public GameObject pauseMenuUI;

    public GameObject dieVFX;
    public GameObject boostVFX;
    public GameObject magnetVFX;

    public GameObject coinDetector;

    public Transform playerTransform;

    int pausebtn = 0;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);

        m_spawnTime = 0;
        
        anim = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();

        coinDetector.SetActive(false);
        boostVFX.SetActive(false);
        magnetVFX.SetActive(false);

        lastScore = PlayerPrefs.GetFloat("My Score");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        //if(death == true || pausebtn == 1)
        //{
            //transform.Translate(0, 0, 0);
        //}
        //else
        //{    
            //Move();           
        //} 

        if(death == true || pausebtn == 1)
        {
            m_spawnTime = 0;
            return;
        }

        m_spawnTime -= Time.deltaTime;

        if (m_spawnTime <= 0)
        {
            Move();

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

        //player control end
        trigger = GameObject.FindGameObjectWithTag("Obstacle");

        //Gravity
        Physics.gravity = new Vector3(0, -500, 0);
    } 

    public void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "PlayerTrigger")
        {
           Destroy(trigger.gameObject);
        }

        if(other.gameObject.tag == "Coin")
        {
            //Debug.Log("Cham Coin");
            Destroy(other.gameObject, 0.5f);
            score = score + 5;
        }

        if(other.gameObject.tag == "Boost")
        {
            Destroy(other.gameObject);
            myAudio.PlayOneShot(boostCollection, 1);
            StartCoroutine(BoostController());
        }

        if(other.gameObject.tag == "DeathPoint")
        {  
            if(undie == false && boost == false && magnet == false)
            {
                death = true;

                myAudio.PlayOneShot(deadSound, 1);
                Instantiate(dieVFX, playerTransform.position, Quaternion.identity);

                if(anim)
                {
                    anim.SetTrigger("Die");
                    StartCoroutine(DelayedDead(anim.GetCurrentAnimatorStateInfo(0).length));
                }

                if(score > lastScore)
                {
                    PlayerPrefs.SetFloat("My Score", score);
                }

                //Debug.Log("Death");
            }
        }

        if(other.gameObject.tag == "CoinMagnet")
        {
            StartCoroutine(ActivateCoin());
        } 
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Ground")
        {
            //Debug.Log("cham dat");
            oneJump = 0;
            oneSlide = 0;
            oneLeft = 0;
            oneRight = 0;
        } 
    }

    IEnumerator DelayController()
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator BoostController()
    {
        boostVFX.SetActive(true);
        boost = true;
        undie = true;
        yield return new WaitForSeconds(5f);
        boost = false;
        yield return new WaitForSeconds(5f);
        undie = false;
        boostVFX.SetActive(false);
    }

    IEnumerator Left()
    {
        left = true;
        yield return new WaitForSeconds(0f);
        left = false;
    }

    IEnumerator Right()
    {
        right = true;
        yield return new WaitForSeconds(0f);
        right = false;
    }

    IEnumerator JumpController()
    {
        jump = true;

        if(oneJump == 1 && myAudio && jumpSound)
        {
            myAudio.PlayOneShot(jumpSound, 1);
        }

        yield return new WaitForSeconds(0.2f);
        jump = false;
    }

    IEnumerator SlideController()
    {
        slide = true;
        yield return new WaitForSeconds(0.15f);
        slide = false;
    }

    IEnumerator DelayedDead(float _delay = 0)
    {
        yield return new WaitForSeconds(_delay);
        //Show Dead UI...
        gameOverImg.gameObject.SetActive(true);  
    } 

    IEnumerator ActivateCoin()
    {
        magnet = true;
        undie = true;
        magnetVFX.SetActive(true);
        coinDetector.SetActive(true);
        yield return new WaitForSeconds(20f);
        coinDetector.SetActive(false);
        magnetVFX.SetActive(false);
        magnet = false;
        undie = false;
    }

    void OnAction()
    {
        if(isBoost == true)
        {
            time = 5f;
            boostVFX.SetActive(true);
            boost = true;
            undie = true;
            StartCoroutine(DelayController());
            boost = false;
            StartCoroutine(DelayController());
            undie = false;
            boostVFX.SetActive(false);
        }
    }    

    void Move()
    {
        foreach(Touch t in Input.touches)
        {
            if(t.phase == TouchPhase.Began)
            {
                initialTouch = t;
            }
            else if(t.phase == TouchPhase.Moved && !hasSwiped)
            {
                float deltaX = initialTouch.position.x - t.position.x;
                float deltaY = initialTouch.position.y - t.position.y;
                distance = Mathf.Sqrt((deltaX*deltaX) + (deltaY*deltaY));
                bool swipedSideWay = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);

                if(distance > 100f)
                {
                    if(swipedSideWay && deltaX > 0)
                    {
                        //swiped left

                        if(transform.position.x < 50f)
                            return;

                        if(boost == false)
                        {
                            oneLeft += 1;
                            StartCoroutine(Left());
                            transform.Translate(-50f, 0, 0);
                        }
                    }

                    if(swipedSideWay && deltaX <= 0)
                    {
                        //swiped right

                        if(transform.position.x > 100f)
                           return;
                           
                        if(boost == false)
                        {
                            oneRight += 1;
                            StartCoroutine(Right());
                            transform.Translate(50f, 0, 0);
                        }
                    }

                    if(!swipedSideWay && deltaY > 0)
                    {
                        //swiped down 

                        if(oneSlide == 0 && slide == false)
                        {
                            oneSlide += 1; 
                            StartCoroutine(SlideController());

                            if(myAudio && slideSound)
                            {
                                myAudio.PlayOneShot(slideSound, 1);
                            }
                        } 
                    }

                    if(!swipedSideWay && deltaY <= 0)
                    {
                        //swiped up

                        oneJump += 1;                       
                        StartCoroutine(JumpController());
                    }

                    hasSwiped = true;
                }
            }
            else if(t.phase == TouchPhase.Ended)
            {
                initialTouch = new Touch();
                hasSwiped = false;
            }
        }

        if(boost == true)
        {
            transform.Translate(0, 0, 50);
        }

        if(score < 500)
        {
            transform.Translate(0, 0, 20);
        }
        else if(score > 500)
        {
            transform.Translate(0, 0, 22);
        }
        else if(score > 1000)
        {
            transform.Translate(0, 0, 25);
        } 
        else if(score > 1500)
        {
            transform.Translate(0, 0, 28);
        } 
        else if(score > 2000)
        {
            transform.Translate(0, 0, 32);
        }
        else if(score > 2500)
        {
            transform.Translate(0, 0, 35);
        } 
    }

    void StopMove()
    {
        transform.Translate(0, 0, 0);
    }   

    void OnAnimatorMove()
    {
    
        if(death == false && jump == true && oneJump == 1)
        {
            anim.SetBool("isJump", jump);
            transform.Translate(0, 50, 4);
            oneSlide = 2;
        }
        else if(jump == false)
        {
            anim.SetBool("isJump", jump);
        }

        if(death == false && slide == true && oneSlide == 1)
        {
            anim.SetBool("isSlide", slide);
            transform.Translate(0, 0, 4);
        }
        else if (slide == false)
        {           
            anim.SetBool("isSlide", slide);
        }

        if(left == true)
        {
            anim.SetBool("isLeft", left);
            transform.Translate(0, 17, 0);
            oneSlide = 2;
        }
        else if (left == false)
        {
            anim.SetBool("isLeft", left);
        }

        if(right == true)
        {
            anim.SetBool("isRight", right);
            transform.Translate(0, 17, 0);
            oneSlide = 2;
        }
        else if (right == false)
        {
            anim.SetBool("isRight", right);
        }
    }

    public void Menu()
    {
        //Destroy(gameObject);
        SceneManager.LoadScene("MenuScene");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ResumeButton()
    {
        pausebtn = 0;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void PauseButton()
    {
        pausebtn = 1;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
