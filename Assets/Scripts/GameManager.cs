using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Ball ballScript;
    public GameObject[] balls;
    public GameObject[] locations;
    public GameObject []obcastles;
    public GameObject [] vantilator;
    public GameObject[] Images;
    GameObject copy;
    public GameObject coins;
    public GameObject heartModel;
    public GameObject coin;

    public int activeBallIndex = 0;
    
    public TextMeshProUGUI scoreText,cointPointText;

    public GameObject[] hearts;
    
    
    public GameObject gameOverPanel, profilePanel,modsPanel,mainPanel, gamePanel,walletPanel;
    public GameObject soundOffBtn, soundOnBtn;
    public TextMeshProUGUI nameText, emailText,numberText;

    public float CoinPoint;
    public GameObject scoreImage;
    
    
    
    public TextMeshProUGUI CoinText,gameOverCoinText,ProfilAllCoinText,walletAllCoinText,mainAllCoinText;

   
   public int right = 0;
   
    
   

    int counter;
    
    public string userEmail;
    public DataBaseManager dataBaseManager;
    public int adCounter;
    public int obcastleCounter;
    int coinCounter;
    int heartCounter;

    public AudioSource collisionSound,fileSound,gameoverSound,backgroundSound,coinsound,heartSound,basketSound,planeSound;

    //public AdManager adManager;
    public Intersititial intersititial;
    public Rewarded rewarded;
    public GameObject[] mods;
    public static bool isPlay;
    int i = 0;
    int j = 0;
    int k = 0;

    bool sound = true;

    public GameObject coinCopy;
    public GameObject heartCopy;
    public int score =0;
    bool internet;
    public GameObject internetPanel;
    public bool collision;



    private void Awake()
    {
        

        if (PlayerPrefs.GetString("login").Equals("email"))
            userEmail = Login.instance.newUser.Email;
        
        else if(PlayerPrefs.GetString("login").Equals("google"))
            userEmail = GoogleLogin.instance.user.Email;

      
        emailText.text = userEmail;

         
        if (!PlayerPrefs.HasKey("coin"))
        {
            PlayerPrefs.SetFloat("coin", 0);
        }


        if (!PlayerPrefs.HasKey("AllCoin"))
        {
            PlayerPrefs.SetFloat("AllCoin", 0);
        }
        
        if (!PlayerPrefs.HasKey("sound"))
        {
            PlayerPrefs.SetInt("sound", 1);
            

        }

        
        
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("sound")==1)
        {
            AudioListener.pause = false;
            soundOffBtn.SetActive(false);
            soundOnBtn.SetActive(true);
        }
            

        if (PlayerPrefs.GetInt("sound") == 0)
        {
            AudioListener.pause = true;
            soundOffBtn.SetActive(true);
            soundOnBtn.SetActive(false);
        }
            



        int random = Random.Range(0, 3);
        mods[random].SetActive(true);
        counter = 0;
        
        mainPanel.SetActive(true);
        gamePanel.SetActive(false);
        rewarded.isErned = false;
        
       
        isPlay = false;
        obcastleCounter = 0;
        coinCounter = 0;
        CoinPoint = 0;

        


    }

   
    void Update()
    {
       if (Application.internetReachability == NetworkReachability.NotReachable)
            internet = false;
        else
            internet = true;

        if (!internet)
            internetPanel.SetActive(true);
        if (internet)
            internetPanel.SetActive(false);



            mainAllCoinText.text = PlayerPrefs.GetFloat("AllCoin").ToString();
        PlayerPrefs.SetFloat("AllCoin", PlayerPrefs.GetFloat("coinAmount"));
        CoinText.text = CoinPoint.ToString();


       
            


        if (ballScript.isEnter)
        {
            counter++;
            score++;
            scoreText.text = score.ToString();

            balls[activeBallIndex+1].SetActive(true);
            balls[activeBallIndex].GetComponent<Ball>().enabled = false;
            StartCoroutine(setActiveFalse(balls[activeBallIndex]));
            activeBallIndex += 1;
            ballScript = balls[activeBallIndex ].GetComponent<Ball>();





            i = 0;
            j = 0;
            k = 0;
            if (copy != null)
                copy.SetActive(false);

            if (coinCopy != null)
                coinCopy.SetActive(false);

            if (heartCopy != null)
                heartCopy.SetActive(false);

            ballScript.isEnter = false;

            if (counter > 3) // her iki atýþta bir coin kazanacak
            {
                CoinPoint += 0.001f;
                if(counter % 2 ==0)
                {
                   
                    fileSound.Play();
                    StartCoroutine(setActiveFalse(Images[Random.Range(0, Images.Length + 1)]));
                }
                   
                

               
            }
        }

        
        
        
        if(ballScript.isMiss)
        {

            counter++;
            balls[activeBallIndex + 1].SetActive(true);
            balls[activeBallIndex].GetComponent<Ball>().enabled = false;
            StartCoroutine(setActiveFalse(balls[activeBallIndex]));
            activeBallIndex += 1;
            ballScript = balls[activeBallIndex].GetComponent<Ball>();
            hearts[right].GetComponent<RawImage>().color = Color.gray;

            right++;

            i = 0;
            j = 0;
            k= 0;

            if (copy != null)
                copy.SetActive(false);
            if (coinCopy != null)
                coinCopy.SetActive(false);
            if (heartCopy != null)
                heartCopy.SetActive(false);


            ballScript.isMiss = false;

            
        }

        if (rewarded.isErned)
        {
            CoinPoint *= 2;
            gameOverCoinText.text = CoinPoint.ToString();

            PlayerPrefs.SetFloat("AllCoin", CoinPoint + PlayerPrefs.GetFloat("AllCoin"));

            dataBaseManager.setUser(PlayerPrefs.GetFloat("AllCoin"));

            CoinPoint = 0;
            right = 0;
            gameOverPanel.SetActive(true);
            gameoverSound.Play();
            backgroundSound.Pause();
         
            isPlay = false;
            rewarded.isErned = false;


            Debug.Log(PlayerPrefs.GetFloat("AllCoin"));
        }

        rewarded.isErned = false;
        if (right>=3) // oyun haklarý için 
        {

            int random = Random.Range(0, 3);
           
            if(random == 1)
            {
                if (intersititial.interstitial.IsLoaded())
                    intersititial.interstitial.Show();
                
            }
            

            setGameOver();
        }

        

        if (activeBallIndex % 3 == 0 & i==0)
        {
            if (activeBallIndex != 0)
                
                getCoins(coins, locations[Random.Range(0, 2)].transform.position);

            i++;
        }
        
        
        if (activeBallIndex % 4 == 0 & j==0)
        {
            if (activeBallIndex != 0)
               
               getHeart(heartModel, locations[Random.Range(0,2)].transform.position);
            j++;
        }
        
        if (activeBallIndex % 2 == 0 & k==0)
        {
            if(activeBallIndex!=0)
            {
               if(activeBallIndex >5)
                getObcastles(balls[activeBallIndex].transform.position + new Vector3(0,-1, 3));
            }

            if (activeBallIndex == 70 || activeBallIndex == 75 || activeBallIndex == 80 || activeBallIndex == 85 || activeBallIndex == 90)
                vantilator[1].SetActive(true);
            
            if (activeBallIndex == 45 || activeBallIndex == 50 || activeBallIndex == 55 || activeBallIndex == 60 || activeBallIndex == 65)
                vantilator[0].SetActive(true);
           
            k++;
        }
       




    }

   
    public void getCoins(GameObject gameObject, Vector3 location)
    {
         coinCopy = Instantiate(gameObject, location, Quaternion.identity);
    }
    
    public void getHeart(GameObject gameObject, Vector3 location)
    {
         heartCopy = Instantiate(gameObject, location, Quaternion.identity);
    }

    public void getObcastles( Vector3 location)
    {
      
        int random = Random.Range(0, obcastles.Length+1);
        obcastles[random].SetActive(true);
        obcastles[random].transform.position = location;
        copy = obcastles[random];
    }
   

    public void openWalletPanel()
    {
       
        walletPanel.SetActive(true);
        mainPanel.SetActive(false);
        walletAllCoinText.text = PlayerPrefs.GetFloat("AllCoin").ToString();
    }
   
    public void soundOn()
    {
        PlayerPrefs.SetInt("sound", 1);
       
        AudioListener.pause = false;
        soundOffBtn.SetActive(false);
        soundOnBtn.SetActive(true);
      
    }

    public void soundOff()
    {
        PlayerPrefs.SetInt("sound", 0);
        
        AudioListener.pause = true;
        soundOffBtn.SetActive(true);
        soundOnBtn.SetActive(false);
        
    }


    public void setGameOver()
    {
        gameOverCoinText.text = CoinPoint.ToString();

        PlayerPrefs.SetFloat("AllCoin", CoinPoint + PlayerPrefs.GetFloat("AllCoin"));

        dataBaseManager.setUser(PlayerPrefs.GetFloat("AllCoin"));

       
        right = 0;
        gameOverPanel.SetActive(true);
        gameoverSound.Play();
        backgroundSound.Pause();
      
        isPlay = false;


        Debug.Log(PlayerPrefs.GetFloat("AllCoin"));
    }

    public void closeWalletPanel()
    {
        walletPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void CloseProfileBtn()
    {
        profilePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OpenProfileBtn()
    {
        profilePanel.SetActive(true);
        mainPanel.SetActive(false);
        ProfilAllCoinText.text = PlayerPrefs.GetFloat("AllCoin").ToString();
       
    }
    public void SignOutbtn()
    {
        PlayerPrefs.SetFloat("AllCoin", 0);
        PlayerPrefs.SetFloat("coinAmount", 0);
        Login.instance.SignOut();
        GoogleLogin.SignOut();

      

    }

    public void screenShot()
    {
        ScreenCapture.CaptureScreenshot("file.png");
    }

    public void openPrivacyUrl()
    {
        Application.OpenURL("https://monegon.com/gizlilik-politikasi/");
    }
    
    public void openRateUsUrl()
    {
        Application.OpenURL("https://monegon.com/anasayfa/");
    }

   

    public void ReStart()
    {
        SceneManager.LoadScene("SampleScene");
        gameOverPanel.SetActive(false);
     }

    
    public void playGame()
    {

       

        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        backgroundSound.Play();

       
        isPlay = true;
     
    }

    public void rewardBtn()
    {
        //adManager.ShowRewardedAds();
       if( rewarded.rewardedAd.IsLoaded())
         {
            rewarded.rewardedAd.Show();
        }
      
        
       
    }

    IEnumerator setActiveFalse(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        scoreImage.SetActive(true);

    }


   

    
}
