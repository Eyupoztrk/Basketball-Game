using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    
    public bool isEnter;
    public bool isMiss;

    public Camera camera;
    Vector3 ballPos;

    public bool inArea;
    bool firstCollision;
    public GameManager gameManager;

    

   



    void Start()
    {
      
        ballPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 4);
        gameManager.collision = false;
    }

    
    void Update()
    {
        float distance = Vector3.Distance(camera.transform.position, transform.position);
        if(distance<7 )
        camera.transform.Translate(ballPos * 0.05f * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(inArea)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1,0,0) * 50);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("pota"))
        {
            firstCollision = true;
        }

        if(other.gameObject.CompareTag("pota2")  && gameObject.CompareTag("ball") && firstCollision)
        {
            
            gameObject.tag = "Player";
            gameManager.basketSound.Play();
            isEnter = true;
            firstCollision = false;
        }

        if(other.gameObject.CompareTag("area"))
        {
            inArea = true;
        }

        if(other.gameObject.CompareTag("coin"))
        {
            Debug.Log("coinn");
            
            gameManager.CoinPoint += 0.001f;
            gameManager.coinsound.Play();
            other.gameObject.SetActive(false);
        }

        if(other.gameObject.CompareTag("heart"))
        {
            other.gameObject.SetActive(false);
            gameManager.heartSound.Play();
            if (gameManager.right != 0)
            {
                gameManager.right--;
                gameManager.hearts[gameManager.right].GetComponent<RawImage>().color = Color.white;
                
            }
            
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("area"))
        {
            inArea = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("plane") && gameObject.CompareTag("ball"))
        {
            Debug.Log("kaybettiniz");
            gameManager.planeSound.Play();
            gameObject.tag = "Player";
            isMiss = true;
        }

        if(collision.gameObject.CompareTag("sound"))
        {
           gameManager.collisionSound.Play();
            
           
        }
       

        
    }
}
