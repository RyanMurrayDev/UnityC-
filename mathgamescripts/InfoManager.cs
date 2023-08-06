using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InfoManager : MonoBehaviour
{
 
    public bool fromLogIn = false;

    public bool successUpdate;

    private int EasyWin;
    private int MediumWin;
    private int HardWin;
    private int MultiplayerWin;
    private int Coins;
    //private bool[] backgrounds = new bool[6];

    public static InfoManager instance;



    void Awake()
    {
        //singleton 
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    public bool GetFromLogIn()
    {
        //Debug.Log("get from login");
        return fromLogIn;
    }

    public void SetFromLogIn(bool b)
    {
        fromLogIn = b;
        //Debug.Log("from log in set to " + b);
    }

    public void CallGetWins()
    {
        //Debug.Log("call get wins");
        StartCoroutine(GetWins());
    }

    IEnumerator GetWins()
    {

        String username = PlayerPrefs.GetString("Username");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", username));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:80/sqlconnect/getuserdata.php", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        string s = www.downloadHandler.text;
        Debug.Log("s: " + s);
        if (www.downloadHandler.text == "")
        {
            Debug.Log("server down");
            successUpdate = false;

        }
        else if (www.downloadHandler.text[0].Equals('0'))
        {
            
            string[] split = www.downloadHandler.text.Split('\t');
            //Debug.Log(split[0]);
            //Debug.Log("easy wins " + split[1]);
            //Debug.Log("medium wins " + split[2]);
            //Debug.Log("hard wins " + split[3]);
            //Debug.Log("multiplayer wins " + split[4]);
           EasyWin = System.Convert.ToInt32(split[1]);
            Debug.Log("easy win: " + EasyWin);
            MediumWin = System.Convert.ToInt32(split[2]);
            HardWin = System.Convert.ToInt32(split[3]);
            MultiplayerWin = System.Convert.ToInt32(split[4]);
            Coins = System.Convert.ToInt32(split[5]);
            if (EasyWin < PlayerPrefs.GetInt("EasyWins", 0))
            {
                EasyWin = PlayerPrefs.GetInt("EasyWins");
                Debug.Log("player prefs more  easy wins: " + EasyWin);
            }
            else
            {
                PlayerPrefs.SetInt("EasyWins",EasyWin);
            }
            if (MediumWin < PlayerPrefs.GetInt("MediumWins", 0))
            {
                MediumWin = PlayerPrefs.GetInt("MediumWins");
                Debug.Log("MediumWin wins: " + MediumWin);
            }
            else
            {
                PlayerPrefs.SetInt("MediumWins", MediumWin);
            }
            if (HardWin < PlayerPrefs.GetInt("HardWins", 0))
            {
                HardWin = PlayerPrefs.GetInt("HardWins");
                Debug.Log("HardWin wins: " + HardWin);
            }
            else
            {
                PlayerPrefs.SetInt("HardWins", HardWin);
            }
            if (MultiplayerWin < PlayerPrefs.GetInt("MultiplayerWins", 0))
            {
                MultiplayerWin = PlayerPrefs.GetInt("MultiplayerWins");
                Debug.Log("MultiplayerWin wins: " + MultiplayerWin);
            }
            else
            {
                PlayerPrefs.SetInt("MultiplayerWins", MultiplayerWin);
            }
            if (Coins < PlayerPrefs.GetInt("Coins", 0))
            {
                Coins = PlayerPrefs.GetInt("Coins");
                Debug.Log("Coins: " + Coins);
            }
            else
            {
                PlayerPrefs.SetInt("Coins", Coins);
            }
            StartCoroutine(UpdateWins());
        }
        else
        {
            Debug.Log("User creation failed. Error #" + s);
            successUpdate = false;

        }
    }

    IEnumerator UpdateWins()
    {
        Debug.Log("update wins called");
        String username = PlayerPrefs.GetString("Username");
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", username));
        formData.Add(new MultipartFormDataSection("easywins", EasyWin.ToString()));
        formData.Add(new MultipartFormDataSection("mediumwins", MediumWin.ToString()));
        formData.Add(new MultipartFormDataSection("hardwins", HardWin.ToString()));
        formData.Add(new MultipartFormDataSection("multiplayerwins", MultiplayerWin.ToString()));
        formData.Add(new MultipartFormDataSection("coins", Coins.ToString()));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:80/sqlconnect/savedata.php", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        string s = www.downloadHandler.text;
        Debug.Log("s: " + s);
        if (www.downloadHandler.text == "")
        {
            Debug.Log("server down");
            successUpdate = false;
        }
        else if (www.downloadHandler.text == "0")
        {
            //no errors
            Debug.Log("Data saved successfully");
            successUpdate = true;
        }
        else
        {
            Debug.Log("User creation failed. Error #" + s);
            successUpdate = false;

        }
    }

    public bool WasSuccess()
    {
        return successUpdate;
    }
}
