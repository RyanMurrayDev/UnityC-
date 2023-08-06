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

public class MultLobby : MonoBehaviourPunCallbacks
{
    public TMP_Text LobbyCodeText;
    public TMP_Text NamesText;
    public TMP_Text TotalText;
    public GameObject StartButton;
    private string LobbyCode;
    private string Name;
    private string isAdmin = "False";
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    //private string questions = "";

    public void StartButtonClick()
    {
        Debug.Log("Start clicked");
        //if i want to make closed when started
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("GameMulti");
        //set lobby joinable from 0 to 1
    }

    public void HomeButtonClick()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Disconnected from photon");
    }

    /*IEnumerator GetNames(string lobby)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("lobbycode", lobby));
        Debug.Log("making web request post");
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:80/sqlconnect/updatelobby.php", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        string s = www.downloadHandler.text;
        //Debug.Log("s: " + s);
        if (www.downloadHandler.text == "")
        {
            Debug.Log("server down");
        }
        else if (s == "0")
        {
            
        }
        else
        {
            //HERE ******************* GET NAMES FROM SERVER AND CONSTANTLY UPDATE IN A FUNCTION ***************
            Debug.Log(s);
            Debug.Log(s.GetType());
            string s1 = JsonUtility.ToJson(s);
            Debug.Log(s1);
            //string c = "";
            //Debug.Log(c);
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
        //when host clicks start every user move to game scene
        PhotonNetwork.AutomaticallySyncScene = true;
        //Debug.Log("start");
        //Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["questions"]);
        //LobbyCode = PlayerPrefs.GetString("Lobby", "");
        // Name = PlayerPrefs.GetString("OnlineName", "Guest");
        //PlayerPrefs.SetString("isAdmin", "True");
        LobbyCode = PhotonNetwork.CurrentRoom.Name;
        Name = PhotonNetwork.LocalPlayer.NickName;


        //now every player will have same time and questions just in case original host disconnects
        Debug.Log("Setting user properties to master clients " + (string)PhotonNetwork.MasterClient.CustomProperties["questions"]
            + " time: " + (string)PhotonNetwork.MasterClient.CustomProperties["time"]);
        PhotonNetwork.LocalPlayer.CustomProperties["questions"] = (string)PhotonNetwork.MasterClient.CustomProperties["questions"];
        PhotonNetwork.LocalPlayer.CustomProperties["time"] = (string)PhotonNetwork.MasterClient.CustomProperties["time"];

        if(LobbyCode == "")
        {
            LobbyCodeText.text = "Error: Go to Main Menu";
        }
        else
        {
            string s = PhotonNetwork.CurrentRoom.Name;
            LobbyCodeText.text = PhotonNetwork.CurrentRoom.Name;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
        Debug.Log("At Lobby  CODE: " + LobbyCode + " NAME: " + Name + " isAdmin: " + isAdmin);
        //StartCoroutine(GetNames(LobbyCode));
        TotalText.text = "Total: " + PhotonNetwork.CurrentRoom.PlayerCount;
        AddAllActivePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        //AddAllActivePlayers();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        NamesText.text = "Getting room list";
        Debug.Log("Room list update from mult lobby");
        //After this callback, update the room list
        createdRooms = roomList;
        Debug.Log("room list: " + createdRooms);
        AddAllActivePlayers();
   
    }
    public void AddAllActivePlayers()
    {
        string s = "";
        Dictionary<int, Photon.Realtime.Player> pList = Photon.Pun.PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Photon.Realtime.Player> p in pList)
        {
            //Debug.Log(p.Value.NickName);
            s += p.Value.NickName + "\n";
        }
        /*foreach (Player player in PhotonNetwork.PlayerList)    could try this
{
    print(player.NickName);
}*/
        NamesText.text = s;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //update player text field on next screen
        Debug.Log("new player entered room: " + newPlayer.NickName);
        AddAllActivePlayers();
        TotalText.text = "Total: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("a player left room: " + otherPlayer.NickName);
        AddAllActivePlayers();
        TotalText.text = "Total: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

 

}
