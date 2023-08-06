using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using System.Linq; //for ordering dictionary

public class MultiLeaderboard : MonoBehaviourPunCallbacks
{

    public TMP_Text TitleText;
    public TMP_Text UsersNamesText;
    public TMP_Text UsersScoresText;
    public TMP_Text ScoreText;
    public ParticleSystem WinPS;
    public GameObject nextQuestionButton;

    private int onQuestion;
    private string questions;
    private string[] questionarray;
    private int numQuestions;
    private bool isOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start leaderboard wait 2");
        StartCoroutine(WaitTime(2));
    }

    IEnumerator WaitTime(int t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log("done waiting");
        WinPS.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            nextQuestionButton.SetActive(true);
        }
        else
        {
            nextQuestionButton.SetActive(false);
        }
        //minus one cuz already changed on multgame
        onQuestion = (int)PhotonNetwork.LocalPlayer.CustomProperties["onQuestion"]-1;
        questions = (string)PhotonNetwork.LocalPlayer.CustomProperties["questions"];
        questionarray = questions.Split(' ');
        numQuestions = questionarray.Length / 6;
        Debug.Log("Just did question: " + onQuestion + " out of " + numQuestions);
        if (onQuestion >= numQuestions)
        {
            TitleText.text = "Game Over";
            isOver = true;
            nextQuestionButton.SetActive(false);
        }
        SetPlayerScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HomeButtonClick()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected from photon");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextQuestionClick()
    {
        //change question on variable
        //do that after a win
        //PhotonNetwork.LocalPlayer.CustomProperties["onQuestion"] = (int)PhotonNetwork.LocalPlayer.CustomProperties["onQuestion"] + 1;
        SceneManager.LoadScene("GameMulti");
    }

    public void SetPlayerScores()
    {
        List<KeyValuePair<string, int>> highscoreList = new List<KeyValuePair<string, int>>();
        string scores = "";
        string names = "";
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            Debug.Log(p.NickName + " :" + (int)p.CustomProperties["score"]);
            highscoreList.Add(new KeyValuePair<string, int>(p.NickName, (int)p.CustomProperties["score"]));
        }
        /*highscoreList.Add(new KeyValuePair<string, int>("Patty", 200));
        highscoreList.Add(new KeyValuePair<string, int>("Lizard", 324));
        highscoreList.Add(new KeyValuePair<string, int>("Jake", 543));
        highscoreList.Add(new KeyValuePair<string, int>("Hunt", 2));
        highscoreList.Add(new KeyValuePair<string, int>("Roman", 56));
        highscoreList.Add(new KeyValuePair<string, int>("Hunt", 125));
        highscoreList.Add(new KeyValuePair<string, int>("Jayln", 345));*/

        Debug.Log("highscores");
        foreach (KeyValuePair<string, int> kvp in highscoreList)
        {
            //Debug.Log("Key = {0} + Value = {1}" + kvp.Key + kvp.Value);
            Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
        }
        var top3 = highscoreList.OrderByDescending(pair => pair.Value).Take(5);
        Debug.Log("TOP 5");
        foreach (KeyValuePair<string, int> kvp in top3)
        {
            Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
            names += kvp.Key + "\n";
            scores += kvp.Value + "\n";
        }
        if (isOver)
        {
            var top1 = highscoreList.OrderByDescending(pair => pair.Value).Take(1);
            foreach (KeyValuePair<string, int> kvp in top1)
            {
                Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
                if(kvp.Key == PhotonNetwork.LocalPlayer.NickName)
                {
                    TitleText.text = "You Win!";
                    WinPS.gameObject.SetActive(true);
                    WinPS.Play();
                }
            }
        }
        
        UsersNamesText.text = names;
        UsersScoresText.text = scores;
        ScoreText.text = "Your Score: " + (int)PhotonNetwork.LocalPlayer.CustomProperties["score"];
        //WinPS.Play();  //play if user is winner
    }
}
