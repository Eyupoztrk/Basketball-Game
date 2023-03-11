using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class walletManager : MonoBehaviour
{
    
   
    public TMP_InputField coinAmount,coinAddress;
    public TextMeshProUGUI fee;
    public DataBaseManager DMManager;
    public GameObject infoPanel,donePanel,infoPanel2;
    public TextMeshProUGUI walletCoinText;
    int minAmount;
    void Start()
    {
        donePanel.SetActive(false);
        minAmount = 20;

    }

   
    void Update()
    {
      
    }
    
    public void getWithdraw()
    {
        if( int.Parse(coinAmount.text) > PlayerPrefs.GetFloat("AllCoin"))
        {
            StartCoroutine(showPanel(infoPanel));
        }
        else if(int.Parse(coinAmount.text) < minAmount )
        {
            StartCoroutine(showPanel(infoPanel2));
        }
        else if(int.Parse(coinAmount.text) >= minAmount && PlayerPrefs.GetFloat("AllCoin")>= float.Parse(coinAmount.text))
        {
            PlayerPrefs.SetFloat("AllCoin", PlayerPrefs.GetFloat("AllCoin") - float.Parse(coinAmount.text));
            PlayerPrefs.SetFloat("coinAmount", PlayerPrefs.GetFloat("AllCoin"));
            walletCoinText.text = PlayerPrefs.GetFloat("AllCoin").ToString();
            DMManager.setUser(PlayerPrefs.GetFloat("AllCoin"));
            DMManager.setWithdraw(coinAddress.text, coinAmount.text);

            StartCoroutine(showPanel(donePanel));
            coinAmount.text = "";
            coinAddress.text = "";
            
        }
      
    }

    public IEnumerator showPanel(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);
       
        panel.SetActive(false);
    }
}
