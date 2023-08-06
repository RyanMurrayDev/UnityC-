using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementsScript : MonoBehaviour
{
    public TMP_Text usernametext;
    public TMP_Text refreshstatus;
    public TMP_Text EasyWText;
    public TMP_Text MedWText;
    public TMP_Text HardWText;
    public TMP_Text MultwText;
    public TMP_Text TotalwText;
    public TMP_Text CoinsText;

    private string username;
    private int Easyw;
    private int Medw;
    private int Hardw;
    private int Multw;
    private int Totalw;
    private int Coins;
    public void HomeButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }


    void Start()
    {
        //Instead of having refresh button  
        //call refresh in start and make sure it connects database and refreshes 
        //before displaying changes
        //InfoManager.instance.CallGetWins();
        //StartCoroutine(WaitToRefresh());
        username = PlayerPrefs.GetString("Username", "GUEST");
        usernametext.text = username;
        //refreshstatus.text = "";
        Easyw = PlayerPrefs.GetInt("EasyWins", 0);
        Medw = PlayerPrefs.GetInt("MediumWins", 0);
        Hardw = PlayerPrefs.GetInt("HardWins", 0);
        Multw = PlayerPrefs.GetInt("MultiplayerWins", 0);
        Totalw = Easyw + Medw + Hardw + Multw;
        Coins = PlayerPrefs.GetInt("Coins", 0);
        EasyWText.text = "Easy Wins: " + Easyw;
        MedWText.text = "Medium Wins: " + Medw;
        HardWText.text = "Hard Wins: " + Hardw;
        MultwText.text = "Multiplayer Wins: " + Multw;
        TotalwText.text = "Total Wins: " + Totalw;
        CoinsText.text = "Coins: " + Coins;
    }

    public void RefreshButtonClick()
    {
        Debug.Log("refreshbutton click");
        if(PlayerPrefs.GetString("Username") == "GUEST")
        {
            refreshstatus.text = "Must be signed in to connect to database and refresh";
        }
        else
        {
            refreshstatus.text = "Refreshing...";
            InfoManager.instance.CallGetWins();
            StartCoroutine(WaitToRefresh());
        }
        
    }

    IEnumerator WaitToRefresh()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("recall start");
        bool yn = InfoManager.instance.WasSuccess();
        if (yn==true)
        {
            Debug.Log("true");
            refreshstatus.text = "Refreshed";
            Start();
        }
        else
        {
            refreshstatus.text = "Refresh failed. Check connection";
        }
    }
    
}
