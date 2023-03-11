using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class Login : MonoBehaviour
{
    public static Login instance;
    public  FirebaseAuth auth;
    public  FirebaseUser newUser;
    public Toggle toggle;
    

    public TMP_InputField RegisterEmail, RegisterPassword, RegisterNameSurname, RegisterNumber;
    public TMP_InputField LoginEmail, LoginPassword;

    public GameObject loginPanel, registerPanel;

    public TextMeshProUGUI errorText;
    public GameObject errorPanel;

    public int x = 5;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    public string displayName, emailAddress,number;
    void Start()
    {
        
        
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        //Debug.Log(newUser.Email);

       

    }

    
   public IEnumerator CreateUser(string email, string password)
    {
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        
        while(!task.IsCompleted)
        {
            yield return null;  
        }

            if (task.IsCanceled)
            {
               
            StartCoroutine(GeterrorMassage("CreateUserWithEmailAndPasswordAsync was canceled."));
            yield break;
            }
            if (task.IsFaulted)
            {
                
            StartCoroutine(GeterrorMassage("CreateUserWithEmailAndPasswordAsync encountered an error: " ));

            yield break;
            }

            // Firebase user has been created.
             newUser = task.Result;
            DataBaseManager.instance.setUser(0);
            loginPanel.SetActive(true);
            registerPanel.SetActive(false);
       
    }

    public IEnumerator SignIn(string email, string password)
    {
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);

        while (!task.IsCompleted)
        {
            yield return null;
        }

            if (task.IsCanceled)
            {
               
                StartCoroutine(GeterrorMassage("There is a error in Email or Password"));
                yield break;
            }
            if (task.IsFaulted)
            {
               
                StartCoroutine(GeterrorMassage("There is a error in Email or Password"));
                yield break;
            }

            newUser = task.Result;
            
        PlayerPrefs.SetString("login", "email");
      
     
        SceneManager.LoadScene("SampleScene");
       






    }

    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != newUser)
        {
            bool signedIn = newUser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && newUser != null)
            {
                Debug.Log("Signed out " + newUser.UserId);
                SceneManager.LoadScene("LoginScene");
            }
            newUser = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + newUser.UserId);
                PlayerPrefs.SetString("login", "email");
                SceneManager.LoadScene("SampleScene");
                
            }
        }
    }


    public void Register()
    {
       if(RegisterEmail.text.Contains(' '))
        {
            Debug.Log("ddd");
            RegisterEmail.text =RegisterEmail.text.Remove(RegisterEmail.text.LastIndexOf(RegisterEmail.text));
         

            Debug.Log(RegisterEmail.text.Contains(' '));
        }
        
         if (toggle.isOn)
                StartCoroutine(CreateUser(RegisterEmail.text, RegisterPassword.text));
        
       
        
      
    }

    public void SignIn()
    {
        
        if (LoginEmail.text.Contains(' '))
        {
            Debug.Log("ddf");
           LoginEmail.text= LoginEmail.text.Remove(LoginEmail.text.Length-1);
            Debug.Log(LoginEmail.text);
        }
        

        StartCoroutine(SignIn(LoginEmail.text, LoginPassword.text));
        
    }

   

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }
    public void OpenRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void CloseLoginPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    } 
    
    public void CloseRegisterPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    IEnumerator GeterrorMassage(string text)
    {
        errorPanel.SetActive(true);
        errorText.text = text;
        yield return new WaitForSeconds(2f);
        errorPanel.SetActive(false);
    }

    public void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("LoginScene");
    }

    public void Privacy()
    {
        Application.OpenURL("https://monegon.com/gizlilik-politikasi/");
    }





}
