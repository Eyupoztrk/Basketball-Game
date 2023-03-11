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

public class LoginDM : MonoBehaviour
{
    public static LoginDM instance;
    public DatabaseReference usersRef;
    public DatabaseReference withdrawRef;
    string email;
   
    public float coinAmount;
    

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
       
        StartCoroutine(Initilization());
        Debug.Log(getData());
       
      

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
            setUser(2);
           

        }
        else
            Debug.LogError("database Error");


    }

    public void setUser(float allcoin)
    {
        Dictionary<string, object> user = new Dictionary<string, object>();

        user["coinAmount"] = allcoin;
        
        usersRef.Child(Login.instance.newUser.UserId).UpdateChildrenAsync(user);
        

    }



    public void setWithdraw(string address, string amount)
    {
        Dictionary<string, object> withdraw = new Dictionary<string, object>();
        withdraw["address"] = address;
        withdraw["amount"] = amount;
        string key = withdrawRef.Push().Key;
        withdrawRef.Child(key).UpdateChildrenAsync(withdraw);
    }




    public float getData()
    {
        StartCoroutine(getUserData());
        return coinAmount;
    }


    public IEnumerator getUserData()
    {
      
        var task = usersRef.Child("egZDMUFyRodSccrXQ5kakSIzL7m2").GetValueAsync();
        while (!task.IsCompleted)
            yield return null;


        if (task.IsCanceled || task.IsFaulted)
            yield break;

        DataSnapshot snapshot = task.Result;
        foreach (DataSnapshot user in snapshot.Children)
        {
            if (user.Key == "coinAmount")
                coinAmount = (float)user.Value;
        }
    }
}
