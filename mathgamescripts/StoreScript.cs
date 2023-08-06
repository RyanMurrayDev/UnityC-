using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    private Button selectedBackgroundButton;
    private int Coins;
    public Button bg0;
    public Button bg1;
    public Button bg2;
    public Button bg3;
    public Button bg4;
    public Button bg5;
    private Button selectedCardButton;
    public Button c0;
    public Button c1;
    public Button c2;
    public Button c3;
    public Button c4;
    public Button c5;
    public TMP_Text coinText;
    public TMP_Text bg1Text;
    public TMP_Text bg2Text;
    public TMP_Text bg3Text;
    public TMP_Text bg4Text;
    public TMP_Text bg5Text;
    public TMP_Text c1Text;
    public TMP_Text c2Text;
    public TMP_Text c3Text;
    public TMP_Text c4Text;
    public TMP_Text c5Text;
    public GameObject ConfirmWindow;
    public TMP_Text confirmText;
    private TMP_Text buyingText;
    //private string buyingName;
    private int coinsNeeded;
    private string buyingName;


    //set proper selected background.card
    //enable and disable text for already purchased backgrounds and images
    //if try purhcase confirm purchase then update in database
    public void HomeButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayButtonClick()
    {
        SceneManager.LoadScene("GameSinglePlayer");
    }

    public void setSelectedBackgroundButton()
    {
        int selected = PlayerPrefs.GetInt("Background", 0);
        //Debug.Log("selected from player prefs: " + selected);
        if(selected == 0)
        {
            bg0.GetComponent<Image>().enabled = true;
            selectedBackgroundButton = bg0;
        }
        else if (selected == 1)
        {
            bg1.GetComponent<Image>().enabled = true;
            selectedBackgroundButton = bg1;
        }

        else if (selected == 2)
        {
            bg2.GetComponent<Image>().enabled = true;
            selectedBackgroundButton = bg2;
        }
        else if (selected == 3)
        {
            bg3.GetComponent<Image>().enabled = true;
            selectedBackgroundButton = bg3;
        }
        else if (selected == 4)
        {
            bg4.GetComponent<Image>().enabled = true;
            selectedBackgroundButton = bg4;
        }
        else if (selected == 5)
        {
            bg5.GetComponent<Image>().enabled = true;
            selectedBackgroundButton = bg5;
        }
    }

    public void setSelectedCardButton()
    {
        int selected = PlayerPrefs.GetInt("Card", 0);
        //Debug.Log("selected card from player prefs: " + selected);
        if (selected == 0)
        {
            c0.GetComponent<Image>().enabled = true;
            selectedCardButton = c0;
        }
        else if (selected == 1)
        {
            c1.GetComponent<Image>().enabled = true;
            selectedCardButton = c1;
        }

        else if (selected == 2)
        {
            c2.GetComponent<Image>().enabled = true;
            selectedCardButton = c2;
        }
        else if (selected == 3)
        {
            c3.GetComponent<Image>().enabled = true;
            selectedCardButton = c3;
        }
        else if (selected == 4)
        {
            c4.GetComponent<Image>().enabled = true;
            selectedCardButton = c4;
        }
        else if (selected == 5)
        {
            c5.GetComponent<Image>().enabled = true;
            selectedCardButton = c5;
        }
    }

    public void backgroundselect(Button button)
    {
        if(button == bg0)
        {
            PlayerPrefs.SetInt("Background", 0);
        }
        else if (button == bg1)
        {
            if (bg1Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Jordan Background for 1 Coin?";
                buyingText = bg1Text;
                buyingName = "bg1";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Background", 1);
            }
        }
        else if (button == bg2)
        {
            if (bg2Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Money Background for 1 Coin?";
                buyingText = bg2Text;
                buyingName = "bg2";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Background", 2);
            }
        }
        else if (button == bg3)
        {
            if (bg3Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Naruto Background for 1 Coin?";
                buyingText = bg3Text;
                buyingName = "bg3";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Background", 3);
            }
        }
        else if (button == bg4)
        {
            if (bg4Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Baseball Background for 1 Coin?";
                buyingText = bg4Text;
                buyingName = "bg4";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Background", 4);
            }
        }
        else if (button == bg5)
        {
            if (bg5Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Shoe Background for 10 Coins?";
                buyingText = bg5Text;
                buyingName = "bg5";
                coinsNeeded = 10;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Background", 5);
            }
        }
        //make old image background not visible
        selectedBackgroundButton.GetComponent<Image>().enabled = false;
        //Debug.Log("clicked");
        //set border image visible
        button.GetComponent<Image>().enabled = true;
        //save current scale then make it bigger
        Vector3 originalScale = button.transform.localScale;
        button.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        selectedBackgroundButton = button;
        StartCoroutine(WaitToDownsize(.25f, button, originalScale));
    }

    //coroutine to have button return to normal size after so many second  used in numbutton clicked
    private IEnumerator WaitToDownsize(float waitTime, Button button, Vector3 originalScale)
    {
        yield return new WaitForSeconds(waitTime);
        button.transform.localScale = originalScale;
        //Debug.Log("button returned to normal size");
   
    }

    public void cardbackgroundselect(Button button)
    {
        if (button == c0)
        {
            PlayerPrefs.SetInt("Card", 0);
        }
        else if (button == c1)
        {
            if (c1Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Blue Card Background for 1 Coin?";
                buyingText = c1Text;
                buyingName = "c1";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Card", 1);
            } 
        }
        else if (button == c2)
        {
            if (c2Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Baseball Card Background for 10 Coins?";
                buyingText = c2Text;
                buyingName = "c2";
                coinsNeeded = 10;
                return;
            }
            else
            {
                 PlayerPrefs.SetInt("Card", 2);
            }
           
        }
        else if (button == c3)
        {
            if (c3Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Football Card Background for 10 Coins?";
                buyingText = c3Text;
                buyingName = "c3";
                coinsNeeded = 10;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Card", 3);
            }   
        }
        else if (button == c4)
        {
            if (c4Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the White Card Background for 1 Coin?";
                buyingText = c4Text;
                buyingName = "c4";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Card", 4);
            }   
        }
        else if (button == c5)
        {
            if (c5Text.enabled)
            {
                ConfirmWindow.SetActive(true);
                confirmText.text = "Are you sure you want to buy the Black Card Background for 1 Coin?";
                buyingText = c5Text;
                buyingName = "c5";
                coinsNeeded = 1;
                return;
            }
            else
            {
                PlayerPrefs.SetInt("Card", 5);
            }
        }
        //make old image background not visible
        selectedCardButton.GetComponent<Image>().enabled = false;
        //Debug.Log("clicked");
        //set border image visible
        button.GetComponent<Image>().enabled = true;
        //save current scale then make it bigger
        Vector3 originalScale = button.transform.localScale;
        button.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        selectedCardButton = button;
        StartCoroutine(WaitToDownsize(.25f, button, originalScale));
    }


    public void disableTextOfAlreadyBought()
    {
        if (PlayerPrefs.GetInt("bg1", 0) == 1)
        {
            bg1Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("bg2", 0) == 1)
        {
            bg2Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("bg3", 0) == 1)
        {
            bg3Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("bg4", 0) == 1)
        {
            bg4Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("bg5", 0) == 1)
        {
            bg5Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("c1", 0) == 1)
        {
            c1Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("c2", 0) == 1)
        {
            c2Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("c3", 0) == 1)
        {
            c3Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("c4", 0) == 1)
        {
            c4Text.enabled = false;
        }
        if (PlayerPrefs.GetInt("c5", 0) == 1)
        {
            c5Text.enabled = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("Coins", 88);
        ConfirmWindow.SetActive(false);
        setSelectedBackgroundButton();
        setSelectedCardButton();
        disableTextOfAlreadyBought();
        Coins = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = "Coins: " + Coins;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void YesButtonClick()
    {
       // Debug.Log("yes  purchase confirmed");
        if(Coins < coinsNeeded)
        {
            confirmText.text = "Not Enough Coins. Click No to close";
            return;
        }
        ConfirmWindow.SetActive(false);
        Coins = Coins - coinsNeeded;
        PlayerPrefs.SetInt("Coins", Coins);
        coinText.text = "Coins: " + Coins;
        //make text of button just bought not visible so know you don't have to buy it
        buyingText.enabled = false;
        PlayerPrefs.SetInt(buyingName, 1);
        //input purchase in databse
        //*********************************************
    }

    public void NoButtonClick()
    {
       // Debug.Log("purchase NOT confirmed");
        ConfirmWindow.SetActive(false);
        setSelectedBackgroundButton();
        setSelectedCardButton();
    }
}
