using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

 public class DataBaseManager : MonoBehaviour
    {

        public static DataBaseManager instance;
        public DatabaseReference usersRef;
        public DatabaseReference withdrawRef;
        string email;
        public GameManager gameManager;
        public float coinAmount;

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        void Start()
        {

        StartCoroutine(Initilization());
       


    }

      
        void Update()
        {

        }

       private IEnumerator Initilization()
        {
            var task = FirebaseApp.CheckAndFixDependenciesAsync();
            while (!task.IsCompleted)
                yield return null;


            if (task.IsCanceled || task.IsFaulted)
                Debug.Log("firebase Error");

            var dependencyStatus = task.Result;


            if (dependencyStatus == DependencyStatus.Available)
            {
                usersRef = FirebaseDatabase.DefaultInstance.GetReference("Users");
                withdrawRef = FirebaseDatabase.DefaultInstance.GetReference("Withdraw");
                Debug.Log("init completed");


                StartCoroutine(getUserData());
                

            }
            else
                Debug.LogError("database Error");

            
        }

        public void setUser(float allcoin)
         {
             Dictionary<string, object> user = new Dictionary<string, object>();


             user["kazandigi coin miktari"] = allcoin;
            

           
            if(PlayerPrefs.GetString("login").Equals("email"))
           {
            user["kullanici E postasi"] = Login.instance.newUser.Email;
            usersRef.Child(Login.instance.newUser.UserId).UpdateChildrenAsync(user);
            
           }
             
        
           else if(PlayerPrefs.GetString("login").Equals("google"))
           {
            user["kullanici E postasi"] = GoogleLogin.instance.user.Email;
            usersRef.Child(GoogleLogin.instance.user.UserId).UpdateChildrenAsync(user);
           }
            


         } 

       








        public void setWithdraw(string address, string amount)
        {
            Dictionary<string, object> withdraw = new Dictionary<string, object>();
            withdraw["Coin adresi"] = address;
            withdraw["Istenilen miktar"] = amount;

        if (PlayerPrefs.GetString("login").Equals("email"))
            withdraw["kullanici E postasi"] = Login.instance.newUser.Email;

        else if (PlayerPrefs.GetString("login").Equals("google"))
            withdraw["kullanici E postasi"] = GoogleLogin.instance.user.Email;

       
            withdraw["Cekim Tarihi"] = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); ;
            string key = withdrawRef.Push().Key;
            withdrawRef.Child(key).UpdateChildrenAsync(withdraw);
        }






        public IEnumerator getUserData()
        {

            var task = usersRef.Child(Login.instance.newUser.UserId).GetValueAsync();
            

            while (!task.IsCompleted)
            {
                Debug.Log("hata1");
                yield return null;
            }



            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("hata2");
                yield break;
            }


            DataSnapshot snapshot = task.Result;
            foreach (DataSnapshot user in snapshot.Children)
            {
               
                if (user.Key == "kazandigi coin miktari")
                {
                    string value = user.Value.ToString();
                    Debug.Log(value);
                    coinAmount = float.Parse(value);
                    PlayerPrefs.SetFloat("coinAmount", coinAmount);

                   
                }

            }
        }
    }

   



