using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed;

    CoinMove coinMoveScript;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coinMoveScript = gameObject.GetComponent<CoinMove>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CoinDetector")
        {
            coinMoveScript.enabled = true;
        }
    }
}
