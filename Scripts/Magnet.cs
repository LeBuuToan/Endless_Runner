using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    //public GameObject coinDetector;
    public AudioSource myAudio;
    public AudioClip magnetCollection;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    void update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Player")
        {
            myAudio.PlayOneShot(magnetCollection, 1);
            //StartCoroutine(ActivateCoin());
            Destroy(transform.GetChild(0).gameObject);   
        }
    }

    //IEnumerator ActivateCoin()
    //{
        //coinDetector.SetActive(true);
        //yield return new WaitForSeconds(20f);
        //coinDetector.SetActive(false);
    //}
}
