using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Google;
using System.Net.Http;

public class GoogleLogin : MonoBehaviour
{

    public static GoogleLogin instance;
    public string GoogleWepApi = "35599037213-2q12i0p8laq0jlmp9s56t6ldf1055ip5.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    /*Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    static Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;*/

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    static FirebaseAuth auth;
    public  FirebaseUser user;

   

    public static TextMeshProUGUI UserNameText, UserEmailText,UserNumberText;
    public static Image UserProfilePic;
    private string imageUrl;


    public GameObject loginPanel, registerPanel;

    public TextMeshProUGUI errorText;
    public GameObject errorPanel;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleWepApi,
            RequestIdToken = true
        };

       
    }
    private void Start()
    {
        InitFirebae();
        InitializeFirebase();
       
        // SceneManager.LoadScene("SampleScene");

    }
    void InitFirebae()
    {
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth =FirebaseAuth.DefaultInstance;
    }

   public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthFinised);
    }

    void OnGoogleAuthFinised(Task<GoogleSignInUser> task)
    {
        if(task.IsFaulted)
        {
            Debug.LogError("Fault");
            StartCoroutine(GeterrorMassage("Fault"));
            //SceneManager.LoadScene("SampleScene");
        }
        else if(task.IsCanceled)
        {
            Debug.LogError("Login Canceled");
            StartCoroutine(GeterrorMassage("Login Canceled"));
            //SceneManager.LoadScene("SampleScene");
        }
        else
        {
           // Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
           Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {

                if (task.IsCanceled)
                {
                    Debug.LogError("hattaa");
                    StartCoroutine(GeterrorMassage("Login Canceled"));
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("hata " + task.Exception);
                    StartCoroutine(GeterrorMassage("Login Fault"));
                    return;
                }

                SceneManager.LoadScene("SampleScene");
                user = auth.CurrentUser;

               /* UserNameText.text = user.DisplayName;
                UserEmailText.text = user.Email;
                UserNumberText.text = user.PhoneNumber;*/

                
                

                StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
                PlayerPrefs.SetString("login","google");
                SceneManager.LoadScene("SampleScene");


            });
        }
    }

    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
            return url;

        return imageUrl;
    }

    IEnumerator LoadImage( string imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;

        UserProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

   

    public static void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("LoginScene");
    }

    IEnumerator GeterrorMassage(string text)
    {
        errorPanel.SetActive(true);
        errorText.text = text;
        yield return new WaitForSeconds(2f);
       errorPanel.SetActive(false);
    }


    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                SceneManager.LoadScene("LoginScene");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                PlayerPrefs.SetString("login","google");
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}
