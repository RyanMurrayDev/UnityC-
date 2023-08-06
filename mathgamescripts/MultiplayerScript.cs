using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerScript : MonoBehaviourPunCallbacks
{
    System.Random random = new System.Random();
    public TMP_InputField NameInput = null;
    public TMP_InputField LobbyCodeInput = null;
    public TMP_InputField CardLowRangeInput = null;
    public TMP_InputField CardHighRangeInput = null;
    public TMP_InputField AnswerLowRangeInput = null;
    public TMP_InputField AnswerHighRangeInput = null;
    public TMP_InputField NumQuestionsInput = null;
    public TMP_InputField TimeAnswerInput = null;
    public TMP_Text ErrorText;

  
    private const string gameVersion = "0.1";
    //The list of created rooms
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    //Use this name when creating a Room
    //string roomName = "Room 1";
    //bool joiningRoom = false;



    public void HomeButtonClick()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public void JoinButtonClick()
    {
        if(NameInput.text == "")
        {
            ErrorText.text = "Please Enter a Name";
            ErrorText.enabled = true;
        }
        else if (LobbyCodeInput.text == "")
        {
            ErrorText.text = "Please Enter a Lobby Code to Join";
            ErrorText.enabled = true;
        }
        else
        {
            ErrorText.text = "Joining Lobby...";
            ErrorText.enabled = true;
            //sets nickname on photon network
            //string s = LobbyCodeInput.text + NameInput.text;

            PlayerPrefs.SetString("OnlineName", NameInput.text);
            PhotonNetwork.NickName = NameInput.text;
            PlayerPrefs.SetString("Lobby", LobbyCodeInput.text);
            PlayerPrefs.SetString("isAdmin", "False");

            //calls 
            Hashtable hash = new Hashtable();
            hash.Add("isAdmin", false);
            hash.Add("questions", "");
            hash.Add("onQuestion", 1);
            hash.Add("score", 0);
            hash.Add("time", "");   
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PhotonNetwork.JoinRoom(LobbyCodeInput.text);

        }
    }
    

    public void CreateLobbyButtonClick()
    {
        if (NameInput.text == "")
        {
            ErrorText.text = "Please Enter a Name";
            ErrorText.enabled = true;
            return;
        }
        if (CardLowRangeInput.text == "" || CardHighRangeInput.text == "")
        {
            ErrorText.text = "Please enter card range";
            ErrorText.enabled = true;
            return;
        }
        if (AnswerLowRangeInput.text == "" || AnswerHighRangeInput.text == "")
        {
            ErrorText.text = "Please enter answer range";
            ErrorText.enabled = true;
            return;
        }
        if (NumQuestionsInput.text == "")
        {
            ErrorText.text = "Please enter # of questions wanted";
            ErrorText.enabled = true;
            return;
        }
        if (TimeAnswerInput.text == "")
        {
            ErrorText.text = "Please enter time to answer";
            ErrorText.enabled = true;
            return;
        }
        if (int.Parse(CardLowRangeInput.text) > int.Parse(CardHighRangeInput.text))
        {
            ErrorText.text = "Invalid card range (least to greatest)";
            ErrorText.enabled = true;
            return;
        }
        if (int.Parse(AnswerLowRangeInput.text) > int.Parse(AnswerHighRangeInput.text))
        {
            ErrorText.text = "Invalid answer range (least to greatest)";
            ErrorText.enabled = true;
            return;
        }
        //input fields should only allow numbers to be inputted
        //make recomendation if time given to solve is really short
        ErrorText.text = "Creating Lobby...";
        ErrorText.enabled = true;
        //CHANGE FROM 3 WHEN DEPLOYED
        string code = RandomString(3);
        //create questions
        string q = createQuestions(int.Parse(CardLowRangeInput.text), int.Parse(CardHighRangeInput.text),
            int.Parse(AnswerLowRangeInput.text), int.Parse(AnswerHighRangeInput.text), int.Parse(NumQuestionsInput.text));
        Debug.Log(q);
        //name, room options max players
        PlayerPrefs.SetString("Lobby", code);
        PlayerPrefs.SetString("OnlineName", NameInput.text);
        PlayerPrefs.SetString("isAdmin", "True");
        PlayerPrefs.SetInt("CardLowRange", int.Parse(CardLowRangeInput.text));
        PlayerPrefs.SetInt("CardHighRange", int.Parse(CardHighRangeInput.text));
        PlayerPrefs.SetInt("AnsLowRange", int.Parse(AnswerLowRangeInput.text));
        PlayerPrefs.SetInt("AnsHighRange", int.Parse(AnswerHighRangeInput.text));
        PlayerPrefs.SetInt("NumQuesRange", int.Parse(NumQuestionsInput.text));
        PlayerPrefs.SetInt("TimeAnsRange", int.Parse(TimeAnswerInput.text));

        PhotonNetwork.CreateRoom(code, new RoomOptions { MaxPlayers = 10 });
        Hashtable hash = new Hashtable();
        hash.Add("isAdmin", true);
        hash.Add("questions", q);
        hash.Add("onQuestion", 1);
        hash.Add("score", 0);
        hash.Add("time", TimeAnswerInput.text);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        //StartCoroutine(createLobby());
    }

  
    public string RandomString(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string s = "";

        for (int i = 0; i < length; i++)
        {
            int x = random.Next(0, chars.Length);
            s+= chars[x];
        }
        return s;
    }

    public string createQuestions(int lcr,int hcr, int lar, int har,int numquest)
    {
        string s = "";
        for(int i=0 ; i < numquest; i++)
        {
            for(int z = 0; z < 5; z++)
            {
                s += random.Next(lcr, hcr+1);
                s += " ";
            }
            //ans
            s += random.Next(lar, har+1);
            //seperator
            s += " ";
        }
        return s;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("looking for connection");
        PhotonNetwork.AutomaticallySyncScene = false; //sync scene between players
        if (!PhotonNetwork.IsConnected)
        {
            //Set the App version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            //connect to photon server
            PhotonNetwork.ConnectUsingSettings();
        }

        SetUpInputFields();
        PlayerPrefs.SetString("isAdmin", "False");
        ErrorText.enabled = false;
    }


    //sets input field of previous player name from start
    //Will do that with other input fields as well  saved in player prefs
    private void SetUpInputFields()
    {
        if (PlayerPrefs.HasKey("OnlineName")) {
            string defaultname = PlayerPrefs.GetString("OnlineName");
            NameInput.text = defaultname;
        }
 
        int defaultnum = PlayerPrefs.GetInt("CardLowRange",1);
        CardLowRangeInput.text = defaultnum.ToString();
        defaultnum = PlayerPrefs.GetInt("CardHighRange",10);
        CardHighRangeInput.text = defaultnum.ToString();
        defaultnum = PlayerPrefs.GetInt("AnsLowRange",1);
        AnswerLowRangeInput.text = defaultnum.ToString();
        defaultnum = PlayerPrefs.GetInt("AnsHighRange",10);
        AnswerHighRangeInput.text = defaultnum.ToString();
        defaultnum = PlayerPrefs.GetInt("NumQuesRange",5);
        NumQuestionsInput.text = defaultnum.ToString();
        defaultnum = PlayerPrefs.GetInt("TimeAnsRange",60);
        TimeAnswerInput.text = defaultnum.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster");
        ErrorText.text = "Connected to server";
        ErrorText.enabled = true;
        //After we connected to Master server, join the Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /*public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room list update");
        //After this callback, update the room list
        createdRooms = roomList;
        Debug.Log("room list: " + createdRooms);
    }*/

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
        ErrorText.text = "Sorry. Failed creating room";
        ErrorText.enabled = true;
        //joiningRoom = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
        ErrorText.text = "Sorry. Lobby does not exist or game already started";
        ErrorText.enabled = true;
        //joiningRoom = false;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room got created of name " + PhotonNetwork.CurrentRoom);
        //Set our player name
        PhotonNetwork.NickName = PlayerPrefs.GetString("OnlineName", "error"); ;
        //Load the Scene called GameLevel (Make sure it's added to build settings)
        PhotonNetwork.LoadLevel("MultiplayerLobby");
    }
    //not from site

    public override void OnJoinedRoom()
    {
        Debug.Log("Client successfully joined a room");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.LoadLevel("MultiplayerLobby");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //update player text field on next screen
    }


    public void Quit()
    {
        Application.Quit();
    }

}
