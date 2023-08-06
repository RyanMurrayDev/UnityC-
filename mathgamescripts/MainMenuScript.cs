using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.Networking;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject AnimatdNumbers;
    public GameObject HomeButton;
    public GameObject LoadingScreen;
    public Slider slider;
    public TMP_Dropdown difficultydropdown;
    public Text progressText;
    public Toggle soundtoggle;
    public Toggle animationtoggle;
    public Toggle onlineanimationtoggle;

    public Slider volumeslider;
    public TMP_Text LoginTextField;
    public TMP_Text RegisterTextField;
    public TMP_Text ShowLoggedInUserTF;
    public GameObject LoginMenu;
    public GameObject HowToPlay;
    public GameObject Settings;

    public GameObject SignOutButton;
    public GameObject SignInButton;

    public AudioMixer audioMixer;

    public GameObject LoginGO;
    public GameObject RegisterGO;

    public TMP_InputField UsernameInputLogin = null;
    public TMP_InputField PasswordInputLogin = null;
    public TMP_InputField UsernameInputRegister = null;
    public TMP_InputField PasswordInputRegister = null;
    public TMP_InputField EmailInputRegister = null;

    public Image ex1;
    public Image ex2;
    public Image ex3;
    public Image ex4;
    public Image ex5;
    public Image ex6;



    private int ex;
    private string username = "";
    //private string email = "";
    //private string password = "";
    private Animation LoginAnim;
    private Animation RegisterAnim;

    private TouchScreenKeyboard keyboard;

    private GameObject Opened;

    public void SetVolume(float volume)
    {
        //Debug.Log("changing volume: " + volume);
        audioMixer.SetFloat("volume",volume);
        PlayerPrefs.SetFloat("Volume", volume);
        AudioManager.instance.SetVolume(volume);
    }

    public void DropdownValueChanged()
    {
        int diff = difficultydropdown.value;
       // Debug.Log("drop down changed: " + difficultydropdown.value);
        //value 0-easy 1-medium 2-hard
        PlayerPrefs.SetInt("Difficulty", diff);
    }

    public void SoundToggleChanged()
    {
        //Debug.Log("toggle changed to " + soundtoggle.isOn);
        if(soundtoggle.isOn == true)
        {
            PlayerPrefs.SetInt("Sound", 0);
            AudioManager.instance.Play("background");
           // Debug.Log("checked");
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            AudioManager.instance.StopPlay("background");
           // Debug.Log("not checked");
        }

    }

    public void AnimationToggleChanged()
    {
        if (animationtoggle.isOn == true)
        {
            PlayerPrefs.SetInt("Animations", 0);
            //Debug.Log("show animations");
        }
        else
        {
            PlayerPrefs.SetInt("Animations", 1);
           // Debug.Log("dont show animations");
        }

    }

    public void OnlineAnimationToggleChanged()
    {
        if (onlineanimationtoggle.isOn == true)
        {
            PlayerPrefs.SetInt("OnlineAnimations", 0);
           // Debug.Log("show animations");
        }
        else
        {
            PlayerPrefs.SetInt("OnlineAnimations", 1);
            //Debug.Log("dont show animations");
        }

    }

    //start load =level scripts
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        //Debug.Log("should stop play");
        AudioManager.instance.StopPlay("background");
        //Debug.Log("load called waiting 1 seconds");
        LoadingScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
       
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
           // Debug.Log(progress);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            //yield return null;
            yield return new WaitForEndOfFrame();// saw from other tutorial instead of brackeys null
        }
    }
    //end load level scripts
    public void HomeButtonClick()
    {
        //Debug.Log("Home button click");
        Opened.SetActive(false);
        HomeButton.SetActive(false);
        AnimatdNumbers.SetActive(true);
        MainMenu.SetActive(true);
    }

    public void HowToPlayButtonClick()
    {
        //Debug.Log("How to play click");
        MainMenu.SetActive(false);
        AnimatdNumbers.SetActive(false);
        HomeButton.SetActive(true);
        HowToPlay.SetActive(true);
        Opened = HowToPlay;
        ex = 1;
        ex1.enabled = true;
        ex2.enabled = false;
        ex3.enabled = false;
        ex4.enabled = false;
        ex5.enabled = false;
        ex6.enabled = false;
    }

    public void ExPrevClick()
    {
       // Debug.Log("prev click");
        if (ex == 1)
        {
            //do nothing one already shwoing
        }
        else if(ex == 2)
        {
            ex1.enabled = true;
            ex2.enabled = false;
            ex--;
        }
        else if (ex == 3)
        {
            ex2.enabled = true;
            ex3.enabled = false;
            ex--;
        }
        else if (ex == 4)
        {
            ex3.enabled = true;
            ex4.enabled = false;
            ex--;
        }
        else if(ex ==5)
        {
            ex4.enabled = true;
            ex5.enabled = false;
            ex--;
        }
        else
        {
            ex5.enabled = true;
            ex6.enabled = false;
            ex--;
        }
    }

    public void ExNextClick()
    {
        //Debug.Log("next click");
        if (ex == 1)
        {
            ex1.enabled = false;
            ex2.enabled = true;
            ex++;
        }
        else if (ex == 2)
        {
            ex2.enabled = false;
            ex3.enabled = true;
            ex++;
        }
        else if (ex == 3)
        {
            ex3.enabled = false;
            ex4.enabled = true;
            ex++;
        }
        else if (ex == 4)
        {
            ex4.enabled = false;
            ex5.enabled = true;
            ex++;
        }
        else if(ex == 5)
        {
            ex5.enabled = false;
            ex6.enabled = true;
            ex++;
        }
        else
        {
            //do nothing 6 already showing  no next
        }
    }


    public void SettingsButtonClick()
    {
        //Debug.Log("Settings click");
        MainMenu.SetActive(false);
        AnimatdNumbers.SetActive(false);
        HomeButton.SetActive(true);
        Settings.SetActive(true);
        Opened = Settings;
        volumeslider.value = PlayerPrefs.GetFloat("Volume");
        difficultydropdown.value = PlayerPrefs.GetInt("Difficulty");
        int s = PlayerPrefs.GetInt("Sound", 0);
        if(s == 0)
        {
            soundtoggle.isOn = true;
        }
        else
        {
            soundtoggle.isOn = false;
        }
        s = PlayerPrefs.GetInt("Animations", 0);
        if (s == 0)
        {
            animationtoggle.isOn = true;
        }
        else
        {
            animationtoggle.isOn = false; 
        }
        s = PlayerPrefs.GetInt("OnlineAnimations", 1);
        if (s == 0)
        {
            onlineanimationtoggle.isOn = true;
        }
        else
        {
            onlineanimationtoggle.isOn = false;
        }
    }
    public void SignOutButtonClick()
    {
        //Debug.Log("Signout button click");
        //delete user out of player prefs and set username textbox to say guest
        PlayerPrefs.SetString("Username", "GUEST");
        //so user cannot register many accounts with a lot of wins
        PlayerPrefs.SetInt("EasyWins", 0);
        PlayerPrefs.SetInt("MediumWins", 0);
        PlayerPrefs.SetInt("HardWins", 0);
        PlayerPrefs.SetInt("MultiplayerWins", 0);
        PlayerPrefs.SetInt("Coins", 0);
        LoginDone();
    }

    public void SignInButtonClick()
    {
       // Debug.Log("Signin button click");
        AudioManager.instance.StopPlay("background");
        //make login menu visible
        PlayerPrefs.SetString("Username", "");
        //print("username: " + username);
        Start();
    }
   public void PlayButtonClick()
    {
        //if i want to show loading screen do load level script
        //Debug.Log("Play click");
       SceneManager.LoadScene("GameSinglePlayer");
    }
    public void MultiplayerButtonClick()
    {
        //Debug.Log("Multiplayer click");
        SceneManager.LoadScene("Multiplayer");
    }
   
    public void TournamentButtonClick()
    {
        Debug.Log("Tournament click");
    }
    public void FriendsButtonClick()
    {
        Debug.Log("Friends click");
    }
    public void AchievementButtonClick()
    {
        //Debug.Log("Achievement click");
        SceneManager.LoadScene("Achievements");
    }
    public void StoreButtonClick()
    {
        //Debug.Log("Store click");
        SceneManager.LoadScene("Store");
    }

    public void QuitGame()
    {
        //Debug.Log("Quit Game!");
        Application.Quit(); //wont work inside editor
    }

    private void Start()
    {
       
        //PlayerPrefs.DeleteAll();
        LoginAnim = LoginGO.gameObject.GetComponent<Animation>();
        RegisterAnim = RegisterGO.gameObject.GetComponent<Animation>();
        //check if playerprefs username is set  if not show login screen
        username = PlayerPrefs.GetString("Username", "");//"" is default value it will return if nothing is set
        if (username == "") //need to also check a player pref for is guest so everytime user returns to homescreen the sign in menu is not shown 
        {
            LoginMenu.SetActive(true);
            LoginGO.SetActive(true);
            RegisterGO.SetActive(false);
            //Debug.Log("start login");
            //set everything else inactive then call login done from login manager
            HomeButton.SetActive(false);
            AnimatdNumbers.SetActive(false);
            MainMenu.SetActive(false);
            LoadingScreen.SetActive(false);

            HowToPlay.SetActive(false);
            Settings.SetActive(false);
        }
        else
        {
            LoginDone();
        }
        
    }

    //login manager
    public void LoginDone() //where ill set everything in scene active or not
    {
       
        if (PlayerPrefs.GetInt("Sound", 0) == 0) //if sound == 0 play
        {
            Debug.Log("play background");
            AudioManager.instance.Play("background");
        }
        LoginGO.SetActive(false);
        RegisterGO.SetActive(false);
        //set everything active or inactive accordinely
        HomeButton.SetActive(false);
        AnimatdNumbers.SetActive(true);
        MainMenu.SetActive(true);
        LoadingScreen.SetActive(false);

        HowToPlay.SetActive(false);
        Settings.SetActive(false);
        //Debug.Log("Playerprefs username: ");
        //Debug.Log(PlayerPrefs.GetString("Username"));
        string usern = PlayerPrefs.GetString("Username");
        ShowLoggedInUserTF.text = "User: " + usern;
        if(usern == "GUEST")
        {
            SignInButton.SetActive(true);
            SignOutButton.SetActive(false);
        }
        else
        {
            SignInButton.SetActive(false);
            SignOutButton.SetActive(true);
        }

        //set if settings button is clicked on single player game screen so shows settings and not just home screen
       //Debug.Log("IM: " + InfoManager.instance.GetFromLogIn());
       if (InfoManager.instance.GetFromLogIn() == true)
        {
            Debug.Log("Settings button click");
            SettingsButtonClick();
            InfoManager.instance.SetFromLogIn(false);
        }
    }

    public void ContAsGuest()
    {
        //Debug.Log("continue as guest");
        PlayerPrefs.SetString("Username", "GUEST");
        LoginDone();
    }
  
    public void ShowLoginButtonClick()
    {
        RegisterTextField.text = "Please register "; //so if change back itll be changed
        //make animation to play
        //Debug.Log("Show login click");
        LoginGO.SetActive(true);
        RegisterGO.SetActive(false);   
        LoginAnim.Play("ShowLogin");
    }
    public void ShowRegisterButtonClick()
    {
        LoginTextField.text = "Please login "; //so if change back itll be changed
        //make animation to play
        //Debug.Log("Show register click");
        LoginGO.SetActive(false);
        RegisterGO.SetActive(true);
        RegisterAnim.Play("ShowRegister");
    }

    public void OpenKeyboard()
    {
        Debug.Log("should show keyboard");
        //Debug.Log(keyboard.active);
        //keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
       //keybaord does not work in editor   when i build it make sure it shows  should automatically with inputfield
    }

    void Update()
    {
       
    }

    public void CallRegister()
    {
        //Debug.Log("call Register");
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        //Debug.Log("username: " + UsernameInputRegister.text);
        //Debug.Log("password: " + PasswordInputRegister.text);
        //Debug.Log("email: " + EmailInputRegister.text);
        if (UsernameInputRegister.text == "")
        {
            RegisterTextField.text = "Please enter username ";
        }
        else if (EmailInputRegister.text == "")
        {
            RegisterTextField.text = "Please enter email ";
        }
        else if (PasswordInputRegister.text == "")
        {
            RegisterTextField.text = "Please enter password ";
        }
        //WWWForm form = new WWWForm();
        //form.AddField("name", UsernameInputRegister.text);
        //form.AddField("password", PasswordInputRegister.text);
        //http://localhost:80/sqlconnect/register.php
        //WWW www = new WWW("http://localhost:80/sqlconnect/register.php",form);
        //yield return www; //tells unity put on backburner once info back run rest of code
        else
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("name", UsernameInputRegister.text));
            formData.Add(new MultipartFormDataSection("email", EmailInputRegister.text));
            formData.Add(new MultipartFormDataSection("password", PasswordInputRegister.text));
            //Debug.Log("making web request post");
            UnityWebRequest www = UnityWebRequest.Post("http://localhost:80/sqlconnect/register.php", formData);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            string s = www.downloadHandler.text;
            Debug.Log("s: " + s);
            if (www.downloadHandler.text == "")
            {
                Debug.Log("server down");
                RegisterTextField.text = "Server down. Please continue as guest";
            }
            else if (s == "0")
            {
                Debug.Log("user created successfully...showing login");
                //go to main menu
                ShowLoginButtonClick();
            }
            else
            {
                Debug.Log("User creation failed. Error #" + s);
                RegisterTextField.text = s;
            }
        }
      
    }



   /* IEnumerator Test1()
    {
        WWW request = new WWW("http://localhost/sqlconnect/webtest.php");
        yield return request;
        //Debug.Log(request.text);
        string[] webResults = request.text.Split('\t'); //splits text anytime there is a tab
        foreach(string s in webResults)
        {
            Debug.Log(s);
        }
        int webnum = int.Parse(webResults[1]);
    }*/

    public void CallLogin()
    {
        //Debug.Log("call login");
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        //username = UsernameInputLogin.GetComponent<TMP_InputField>().text;
        //password = PasswordInputLogin.GetComponent<TMP_InputField>().text;
        //Debug.Log("username: " + UsernameInputLogin.text);
        //Debug.Log("password: " + PasswordInputLogin.text);
        if (UsernameInputLogin.text == "")
        {
            LoginTextField.text = "Please enter username ";
        }
        else if (PasswordInputLogin.text == "")
        {
            LoginTextField.text = "Please enter password ";
        }
        //WWWForm form = new WWWForm();
        //form.AddField("name", UsernameInputRegister.text);
        //form.AddField("password", PasswordInputRegister.text);
        //http://localhost:80/sqlconnect/register.php
        //WWW www = new WWW("http://localhost:80/sqlconnect/register.php",form);
        //yield return www; //tells unity put on backburner once info back run rest of code
        else
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("name", UsernameInputLogin.text));
            formData.Add(new MultipartFormDataSection("password", PasswordInputLogin.text));

            UnityWebRequest www = UnityWebRequest.Post("http://localhost:80/sqlconnect/login.php", formData);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            string s = www.downloadHandler.text;
            Debug.Log(s);
            if(www.downloadHandler.text == "")
            {
                Debug.Log("server down");
                LoginTextField.text = "Server down. Please continue as guest";
            }
            else if (www.downloadHandler.text[0].Equals('0'))
            {
                PlayerPrefs.SetString("Username", UsernameInputLogin.text);
                //DBManager.username = UsernameInputLogin.text;
                //DBManager.score = int.Parse(www.text.Split('\t')[1]);
                Debug.Log("user login successfully");
                LoginDone();
            }
            else
            {
                Debug.Log("User log in fail. Error #" + s);
                LoginTextField.text = s;
            }
        }

    }


}