using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Notifications : MonoBehaviour
{
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;

    }
    
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {

    }
    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs gelenMesaj)
    {

    }

}
