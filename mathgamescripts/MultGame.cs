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
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultGame : MonoBehaviourPunCallbacks
{
    private string questions;
    private string time="20";
    private float t=100;
    private int onQuestion = 0;
    private int numQuestions = 0;
    private bool countdown = true;
    private string[] questionarray;
    private bool changescene = true;
    private bool didntwin = true; //update everything on win but if dont update in update method


    //variables from gamemanager
    System.Random random = new System.Random();
    public Button numButton1;
    public Button numButton2;
    public Button numButton3;
    public Button numButton4;
    public Button numButton5;
    public TextMeshProUGUI SolutionText;
    //public Text solutionText;
    public Button AddBut;
    public Button SubBut;
    public Button MultBut;
    public Button DivBut;
    public Button FactBut;
    public Button SqrtBut;
    public Button PowBut;
    public Button AbsValBut;
    public Image mdi1; //movement done image
    public Image mdi2;
    public Image mdi3;
    public Image mdi4;
    public Image mdi5;
    private Image mdi;
    public Sprite AddImage;
    public Sprite SubImage;
    public Sprite MultImage;
    public Sprite DivImage;
    public Sprite FactImage;
    public Sprite AbsValImage;
    public Sprite SqRtImage;
    public Sprite PowOfImage;
    public ParticleSystem WinPS;
    public ParticleSystem Button1PS;
    public ParticleSystem Button2PS;
    public ParticleSystem Button3PS;
    public ParticleSystem Button4PS;
    public ParticleSystem Button5PS;

    public TMP_Text UserSettingText;
    public TMP_Text TimerText;
    public TMP_Text QuesNumText;
    public GameObject ProblemCompleteScreen;
    public TMP_Text ProblemCompleteAddedScoreText;
    public TMP_Text ProblemCompleteTotalScoreText;

    //setting background images
    /*
    public Image BackgroundImage;
    public Sprite bg0;
    public Sprite bg1;
    public Sprite bg2;
    public Sprite bg3;
    public Sprite bg4;
    public Sprite bg5;

    public Image Card1Image;
    public Image Card2Image;
    public Image Card3Image;
    public Image Card4Image;
    public Image Card5Image;
    public Image Solution;
    public Sprite c0;
    public Sprite c1;
    public Sprite c2;
    public Sprite c3;
    public Sprite c4;
    public Sprite c5;*/


    private Vector3 NumBut1Pos;
    private Vector3 NumBut2Pos;
    private Vector3 NumBut3Pos;
    private Vector3 NumBut4Pos;
    private Vector3 NumBut5Pos;
    private Sprite OperationImage; //to avoid extra for loops  set that when select operator
    private int num1;
    private int num2;
    private int num3;
    private int num4;
    private int num5;
    private int solution;
    private Button currentSelectedNumButton;
    private Button currentSelectedNumButton2;
    private Button currentSelectedOpButton;
    private bool AllowTwoNumSelected = true; //if ! or sqrt only one num selected else 2
    private int ButtonsLeft = 5;
    private Animation Anim;
    private Animation Anim2;
    private Animation S1; //for spawn
    private Animation S2;
    private Animation S3;
    private Animation S4;
    private Animation S5;
    private bool CanClickButton = true; //so you cant click a button if one is in movement


    //methods i might use
    private IEnumerator WaitToTrailRenderer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Debug.Log("trail renderer enabled");
        numButton1.GetComponent<TrailRenderer>().enabled = true;
        numButton2.GetComponent<TrailRenderer>().enabled = true;
        numButton3.GetComponent<TrailRenderer>().enabled = true;
        numButton4.GetComponent<TrailRenderer>().enabled = true;
        numButton5.GetComponent<TrailRenderer>().enabled = true;
    }

    public void SetGame(int c1, int c2, int c3, int c4, int c5, int sol)
    {
        //Debug.Log("SetGame() called");
        CanClickButton = true;
        ButtonsLeft = 5;
        currentSelectedNumButton = null;
        currentSelectedNumButton2 = null;
        currentSelectedOpButton = null;
        numButton1.gameObject.SetActive(true);
        numButton2.gameObject.SetActive(true);
        numButton3.gameObject.SetActive(true);
        numButton4.gameObject.SetActive(true);
        numButton5.gameObject.SetActive(true);

        //make sure all borders not showing
        numButton1.GetComponent<Image>().enabled = false;
        numButton2.GetComponent<Image>().enabled = false;
        numButton3.GetComponent<Image>().enabled = false;
        numButton4.GetComponent<Image>().enabled = false;
        numButton5.GetComponent<Image>().enabled = false;

        AddBut.GetComponent<Image>().enabled = false;
        SubBut.GetComponent<Image>().enabled = false;
        MultBut.GetComponent<Image>().enabled = false;
        DivBut.GetComponent<Image>().enabled = false;
        FactBut.GetComponent<Image>().enabled = false;
        SqrtBut.GetComponent<Image>().enabled = false;
        PowBut.GetComponent<Image>().enabled = false;
        AbsValBut.GetComponent<Image>().enabled = false;

        ProblemCompleteScreen.SetActive(false);
        // Returns an `int` value greater in the range 10 <= value < 50
        //int num = random.Next(10, 50)
        //Debug.Log("Userdiff: " + UserDiff);
       
        num1 = c1;
        num2 = c2;
        num3 = c3;
        num4 = c4;
        num5 = c5;
        solution = sol;
        //set texts on buttons
        numButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num1.ToString();
        numButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num2.ToString();
        numButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num3.ToString();
        numButton4.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num4.ToString();
        numButton5.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num5.ToString();
        SolutionText.text = solution.ToString();

        //spawn buttons in
        //Debug.Log("spawning in");
        S1 = numButton1.gameObject.GetComponent<Animation>();
        S2 = numButton2.gameObject.GetComponent<Animation>();
        S3 = numButton3.gameObject.GetComponent<Animation>();
        S4 = numButton4.gameObject.GetComponent<Animation>();
        S5 = numButton5.gameObject.GetComponent<Animation>();
        S1.Play("spawnup1");
        S2.Play("spawnup2");
        S3.Play("spawnup3");
        S4.Play("spawnup4");
        S5.Play("spawnup5");

        /*
          S1.Play("spawn1");
          S2.Play("spawn2");
          S3.Play("spawn3");
          S4.Play("spawn4");
          S5.Play("spawn5");*/
        Canvas.ForceUpdateCanvases();
        StartCoroutine(WaitToTrailRenderer(1f));
    }

    public void ResetGame(int c1, int c2, int c3, int c4, int c5, int sol)
    {
        ProblemCompleteScreen.SetActive(false);
        numButton1.gameObject.SetActive(true);
        numButton2.gameObject.SetActive(true);
        numButton3.gameObject.SetActive(true);
        numButton4.gameObject.SetActive(true);
        numButton5.gameObject.SetActive(true);


        Button1PS.Play();
        Button2PS.Play();
        Button3PS.Play();
        Button4PS.Play();
        Button5PS.Play();

        CanClickButton = true;

        ButtonsLeft = 5;
        currentSelectedNumButton = null;
        currentSelectedNumButton2 = null;
        currentSelectedOpButton = null;

        numButton1.transform.position = NumBut1Pos;
        numButton2.transform.position = NumBut2Pos;
        numButton3.transform.position = NumBut3Pos;
        numButton4.transform.position = NumBut4Pos;
        numButton5.transform.position = NumBut5Pos;

        //make sure all borders not showing
        numButton1.GetComponent<Image>().enabled = false;
        numButton2.GetComponent<Image>().enabled = false;
        numButton3.GetComponent<Image>().enabled = false;
        numButton4.GetComponent<Image>().enabled = false;
        numButton5.GetComponent<Image>().enabled = false;

        AddBut.GetComponent<Image>().enabled = false;
        SubBut.GetComponent<Image>().enabled = false;
        MultBut.GetComponent<Image>().enabled = false;
        DivBut.GetComponent<Image>().enabled = false;
        FactBut.GetComponent<Image>().enabled = false;
        SqrtBut.GetComponent<Image>().enabled = false;
        PowBut.GetComponent<Image>().enabled = false;
        AbsValBut.GetComponent<Image>().enabled = false;

        // Returns an `int` value greater in the range 10 <= value < 50
        //int num = random.Next(10, 50)
        num1 = c1;
        num2 = c2;
        num3 = c3;
        num4 = c4;
        num5 = c5;
        solution = sol;
        //set texts on buttons
        numButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num1.ToString();
        numButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num2.ToString();
        numButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num3.ToString();
        numButton4.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num4.ToString();
        numButton5.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num5.ToString();
        SolutionText.text = solution.ToString();

        numButton1.GetComponent<TrailRenderer>().enabled = false;
        numButton2.GetComponent<TrailRenderer>().enabled = false;
        numButton3.GetComponent<TrailRenderer>().enabled = false;
        numButton4.GetComponent<TrailRenderer>().enabled = false;
        numButton5.GetComponent<TrailRenderer>().enabled = false;


        S1 = numButton1.gameObject.GetComponent<Animation>();
        S2 = numButton2.gameObject.GetComponent<Animation>();
        S3 = numButton3.gameObject.GetComponent<Animation>();
        S4 = numButton4.gameObject.GetComponent<Animation>();
        S5 = numButton5.gameObject.GetComponent<Animation>();
        /*
        S1.Play("spawnup1");
        S2.Play("spawnup2");
        S3.Play("spawnup3");
        S4.Play("spawnup4");
        S5.Play("spawnup5");
        */
        S1.Play("spawn1");
        S2.Play("spawn2");
        S3.Play("spawn3");
        S4.Play("spawn4");
        S5.Play("spawn5");


        Canvas.ForceUpdateCanvases();
        StartCoroutine(WaitToTrailRenderer(1f));
    }

    public void ResetMoves()
    {
        numButton1.gameObject.SetActive(true);
        numButton2.gameObject.SetActive(true);
        numButton3.gameObject.SetActive(true);
        numButton4.gameObject.SetActive(true);
        numButton5.gameObject.SetActive(true);
        //Debug.Log("reset moves");
        CanClickButton = true;

        ButtonsLeft = 5;
        currentSelectedNumButton = null;
        currentSelectedNumButton2 = null;
        currentSelectedOpButton = null;
        numButton1.transform.position = NumBut1Pos;
        numButton2.transform.position = NumBut2Pos;
        numButton3.transform.position = NumBut3Pos;
        numButton4.transform.position = NumBut4Pos;
        numButton5.transform.position = NumBut5Pos;

        //make sure all borders not showing
        numButton1.GetComponent<Image>().enabled = false;
        numButton2.GetComponent<Image>().enabled = false;
        numButton3.GetComponent<Image>().enabled = false;
        numButton4.GetComponent<Image>().enabled = false;
        numButton5.GetComponent<Image>().enabled = false;
        AddBut.GetComponent<Image>().enabled = false;
        SubBut.GetComponent<Image>().enabled = false;
        MultBut.GetComponent<Image>().enabled = false;
        DivBut.GetComponent<Image>().enabled = false;
        FactBut.GetComponent<Image>().enabled = false;
        SqrtBut.GetComponent<Image>().enabled = false;
        PowBut.GetComponent<Image>().enabled = false;
        AbsValBut.GetComponent<Image>().enabled = false;


        numButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num1.ToString();
        numButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num2.ToString();
        numButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num3.ToString();
        numButton4.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num4.ToString();
        numButton5.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = num5.ToString();
        SolutionText.text = solution.ToString();

        Canvas.ForceUpdateCanvases();
    }


    public void SelectButton(Button button)
    {
        //set border image visible
        button.GetComponent<Image>().enabled = true;
        //save current scale then make it bigger
        Vector3 originalScale = button.transform.localScale;
        button.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        StartCoroutine(WaitToDownsize(.25f, button, originalScale));
    }

    public void SetSelectedButtons(Button button)
    {
        //Debug.Log(currentSelectedNumButton);
        if (currentSelectedNumButton == null)
        {
            currentSelectedNumButton = button;
            //Debug.Log("#1 is: " + currentSelectedNumButton.GetComponentInChildren<Text>().text);
        }
        else if (currentSelectedNumButton == button)
        {
            //Debug.Log("selected button clicked so set to unselected");
            button.GetComponent<Image>().enabled = false;
            if (currentSelectedNumButton2 != null)
            {
                currentSelectedNumButton = currentSelectedNumButton2;
                currentSelectedNumButton2 = null;
            }
            else
            {
                currentSelectedNumButton = null;
            }
        }
        else if (currentSelectedNumButton2 == button)
        {
            //Debug.Log("selected button clicked so set to unselected");
            button.GetComponent<Image>().enabled = false;
            currentSelectedNumButton2 = null;
        }
        else if (currentSelectedNumButton != null && AllowTwoNumSelected == false)
        {
            //Debug.Log("! or Sqrt only one allowed be selected");
            currentSelectedNumButton.GetComponent<Image>().enabled = false;
            currentSelectedNumButton = button;
        }
        else if (currentSelectedNumButton2 == null && AllowTwoNumSelected)
        {
            currentSelectedNumButton2 = button;
            //Debug.Log("now #1 is: " + currentSelectedNumButton.GetComponentInChildren<Text>().text +
            //  " and #2 is: " + currentSelectedNumButton2.GetComponentInChildren<Text>().text);
        }
        else
        {
            //Debug.Log("Both are set"); //change 2nd selected to 1st  and this to 2nd
            currentSelectedNumButton.GetComponent<Image>().enabled = false;
            currentSelectedNumButton = currentSelectedNumButton2;
            currentSelectedNumButton2 = button;
            // Debug.Log("now #1 is: " + currentSelectedNumButton.GetComponentInChildren<Text>().text +
            //    " and #2 is: " + currentSelectedNumButton2.GetComponentInChildren<Text>().text);
        }
    }
    public void NumButtonClick(Button button)
    {
        if (CanClickButton)
        {
            SelectButton(button);
            SetSelectedButtons(button);
            //Debug.Log("Successful click");
        }
        else
        {
            //Debug.Log("click registered but button in movement");
        }
    }

    //coroutine to have button return to normal size after so many second  used in numbutton clicked
    private IEnumerator WaitToDownsize(float waitTime, Button button, Vector3 originalScale)
    {
        yield return new WaitForSeconds(waitTime);
        button.transform.localScale = originalScale;
        //Debug.Log("button returned to normal size");
    }

    public void OperatorClick(Button button)
    {
        //changed from Boolean  have in gamemanager
        bool con = true;
        if (currentSelectedOpButton == button)
        {
            con = false;
            //Debug.Log("current selected clicked");
            currentSelectedOpButton.GetComponent<Image>().enabled = false;
            currentSelectedOpButton = null;
        }
        else if (currentSelectedOpButton != null)
        {
            //take border off previously selected one
            currentSelectedOpButton.GetComponent<Image>().enabled = false;
        }
        if (con)
        {
            //set border image visible
            button.GetComponent<Image>().enabled = true;
            //save current scale then make it bigger
            Vector3 originalScale = button.transform.localScale;
            button.transform.localScale = new Vector3(1.5f, 1.5f, 0);
            StartCoroutine(WaitToDownsize(.25f, button, originalScale));

            currentSelectedOpButton = button;
            //Debug.Log("current selected num button set");
        }

        if (button.name == "FactButton")
        {
            AllowTwoNumSelected = false;
            if (currentSelectedNumButton != null && currentSelectedNumButton2 != null)
            {
                currentSelectedNumButton.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = currentSelectedNumButton2;
                currentSelectedNumButton2 = null;
            }
        }
        else if (button.name == "SqRtButton")
        {
            AllowTwoNumSelected = false;
            if (currentSelectedNumButton != null && currentSelectedNumButton2 != null)
            {
                currentSelectedNumButton.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = currentSelectedNumButton2;
                currentSelectedNumButton2 = null;
            }
        }
        else if (button.name == "AbsValButton")
        {
            AllowTwoNumSelected = false;
            if (currentSelectedNumButton != null && currentSelectedNumButton2 != null)
            {
                currentSelectedNumButton.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = currentSelectedNumButton2;
                currentSelectedNumButton2 = null;
            }
        }
        else
        {
            AllowTwoNumSelected = true;
        }
    }


    public void SetMovementDoneImage(UnityEngine.UI.Image i)
    {

    }

    //use a coroutine so we can pause this function and wait for the animation to complete
    public IEnumerator MoveNumAnimation(float solution)
    {
        //Debug.Log("Play movement");
        if (PlayerPrefs.GetInt("Sound", 0) == 0)
        {
            AudioManager.instance.Play("movement");
        }
        string n = currentSelectedNumButton.name + "To" + currentSelectedNumButton2.name;
        string n2 = "notset";
        float t = Anim[n].length * (1 / Anim[n].speed);
        //Debug.Log("waiting");
        yield return new WaitForSeconds(t);
        //done moving
        if (currentSelectedNumButton == numButton1)
        {
            //Debug.Log("show on 1");
            mdi = mdi1;
            //Debug.Log("operator showd");
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            n2 = "mdi1";
            Anim.Play("mdi1");
            StartCoroutine(FadeImage(true, mdi1));
        }
        else if (currentSelectedNumButton == numButton2)
        {
            //Debug.Log("show on 2");
            mdi = mdi2;
            // Debug.Log("operator showd");
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            n2 = "mdi2";
            Anim.Play("mdi2");
            StartCoroutine(FadeImage(true, mdi2));
        }
        else if (currentSelectedNumButton == numButton3)
        {
            //Debug.Log("show on 3");
            mdi = mdi3;
            //Debug.Log("operator showed");
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            n2 = "mdi3";
            Anim.Play("mdi3");
            StartCoroutine(FadeImage(true, mdi3));
        }
        else if (currentSelectedNumButton == numButton4)
        {
            //Debug.Log("show on 4");
            mdi = mdi4;
            //Debug.Log("operator showed");
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            n2 = "mdi4";
            Anim.Play("mdi4");
            StartCoroutine(FadeImage(true, mdi4));
        }
        else if (currentSelectedNumButton == numButton5)
        {
            //Debug.Log("show on 5");
            mdi = mdi5;
            //Debug.Log("operator showed");
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            n2 = "mdi5";
            Anim.Play("mdi5");
            StartCoroutine(FadeImage(true, mdi5));
        }
        if (currentSelectedNumButton2 == numButton1)
        {
           // Debug.Log("play1");
            Button1PS.Play();
        }
        else if (currentSelectedNumButton2 == numButton2)
        {
            //Debug.Log("play2");
            Button2PS.Play();
        }
        else if (currentSelectedNumButton2 == numButton3)
        {
            //Debug.Log("play3");
            Button3PS.Play();
        }
        else if (currentSelectedNumButton2 == numButton4)
        {
           // Debug.Log("play4");
            Button4PS.Play();
        }
        else if (currentSelectedNumButton2 == numButton5)
        {
            //Debug.Log("play5");
            Button5PS.Play();
        }
        t = Anim[n2].length * (1 / Anim[n2].speed);
        yield return new WaitForSeconds(t);
        //change text of second selected to solution

        //currentSelectedNumButton2.GetComponentInChildren<Text>().text = solution.ToString();
        currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
        //disable first selected
       // Debug.Log(currentSelectedNumButton.name + " set active false");
        currentSelectedNumButton.gameObject.SetActive(false);
        ButtonsLeft = ButtonsLeft - 1;
        //set selected buttons back to null
        currentSelectedNumButton2.GetComponent<Image>().enabled = false;
        currentSelectedNumButton = null;
        currentSelectedNumButton2 = null;
        //set operator back to null after operation
        currentSelectedOpButton.GetComponent<Image>().enabled = false;
        currentSelectedOpButton = null;
        CanClickButton = true;
        CheckOver();
        Canvas.ForceUpdateCanvases();
    }

    //use a coroutine so we can pause this function and wait for the animation to complete
    public IEnumerator MdiSingle(float solution)
    {
        string n2 = "mdi1";
        if (currentSelectedNumButton == numButton1)
        {
            mdi = mdi1;
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            Anim.Play("mdi1");
            n2 = "mdi1";
            Button1PS.Play();
            StartCoroutine(FadeImage(true, mdi1));
        }
        else if (currentSelectedNumButton == numButton2)
        {
            mdi = mdi2;
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            Anim.Play("mdi2");
            n2 = "mdi2";
            Button2PS.Play();
            StartCoroutine(FadeImage(true, mdi2));
        }
        else if (currentSelectedNumButton == numButton3)
        {
            mdi = mdi3;
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            Anim.Play("mdi3");
            n2 = "mdi3";
            Button3PS.Play();
            StartCoroutine(FadeImage(true, mdi3));
        }
        else if (currentSelectedNumButton == numButton4)
        {
            mdi = mdi4;
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            Anim.Play("mdi4");
            n2 = "mdi4";
            Button4PS.Play();
            StartCoroutine(FadeImage(true, mdi4));
        }
        else if (currentSelectedNumButton == numButton5)
        {
            mdi = mdi5;
            mdi.sprite = OperationImage;
            mdi.enabled = true;
            Anim.Play("mdi5");
            n2 = "mdi5";
            Button5PS.Play();
            StartCoroutine(FadeImage(true, mdi5));
        }

        float t = Anim[n2].length * (1 / Anim[n2].speed);
        yield return new WaitForSeconds(t);
        //change text of second selected to solution
        //currentSelectedNumButton.GetComponentInChildren<Text>().text = solution.ToString();
        currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
        //set selected buttons back to null
        currentSelectedNumButton.GetComponent<Image>().enabled = false;
        currentSelectedNumButton = null;
        currentSelectedNumButton2 = null;
        //set operator back to null after operation
        currentSelectedOpButton.GetComponent<Image>().enabled = false;
        currentSelectedOpButton = null;
        CheckOver();
        CanClickButton = true;
        Canvas.ForceUpdateCanvases();
    }

    //found at https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    IEnumerator FadeImage(bool fadeAway, Image im)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                im.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                im.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
    public void PerformOp()
    {
        if (currentSelectedOpButton.name == "AddButton")
        {
            OperationImage = AddImage;
            //Debug.Log("Add clicked");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            string n2 = currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            //string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            //string n2 = currentSelectedNumButton2.GetComponentInChildren<Text>().text;
            float but1num = float.Parse(n);
            float but2num = float.Parse(n2);
            float solution = but1num + but2num;
            //Debug.Log(n + " + " + n2 + " = " + solution);
            Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
            if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
            {
                //Debug.Log("Shouldnt show animation");
                currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                //disable first selected
                //Debug.Log(currentSelectedNumButton.name + " set active false");
                currentSelectedNumButton.gameObject.SetActive(false);
                ButtonsLeft = ButtonsLeft - 1;
                //set selected buttons back to null
                currentSelectedNumButton2.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = null;
                currentSelectedNumButton2 = null;
                //set operator back to null after operation
                currentSelectedOpButton.GetComponent<Image>().enabled = false;
                currentSelectedOpButton = null;
                CanClickButton = true;
                CheckOver();
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                Anim.Play(currentSelectedNumButton.name + "To" + currentSelectedNumButton2.name);
                StartCoroutine(MoveNumAnimation(solution));
            }
        }
        else if (currentSelectedOpButton.name == "SubButton")
        {
            OperationImage = SubImage;
            //Debug.Log("Subtraction performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            string n2 = currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            //string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            //string n2 = currentSelectedNumButton2.GetComponentInChildren<Text>().text;
            float but1num = float.Parse(n);
            float but2num = float.Parse(n2);
            float solution = but1num - but2num;
           // Debug.Log(n + " - " + n2 + " = " + solution);
            Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
            if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
            {
                //Debug.Log("Shouldnt show animation");
                currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                //disable first selected
                //Debug.Log(currentSelectedNumButton.name + " set active false");
                currentSelectedNumButton.gameObject.SetActive(false);
                ButtonsLeft = ButtonsLeft - 1;
                //set selected buttons back to null
                currentSelectedNumButton2.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = null;
                currentSelectedNumButton2 = null;
                //set operator back to null after operation
                currentSelectedOpButton.GetComponent<Image>().enabled = false;
                currentSelectedOpButton = null;
                CanClickButton = true;
                CheckOver();
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                Anim.Play(currentSelectedNumButton.name + "To" + currentSelectedNumButton2.name);
                StartCoroutine(MoveNumAnimation(solution));
            }

        }
        else if (currentSelectedOpButton.name == "MultButton")
        {
            OperationImage = MultImage;
            //Debug.Log("Multiplication performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            string n2 = currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            //string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            //string n2 = currentSelectedNumButton2.GetComponentInChildren<Text>().text;
            float but1num = float.Parse(n);
            float but2num = float.Parse(n2);
            float solution = but1num * but2num;
            //Debug.Log(n + " + " + n2 + " = " + solution);
            Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
            //Debug.Log("start animate courotine");
            if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
            {
                //Debug.Log("Shouldnt show animation");
                currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                //disable first selected
                //Debug.Log(currentSelectedNumButton.name + " set active false");
                currentSelectedNumButton.gameObject.SetActive(false);
                ButtonsLeft = ButtonsLeft - 1;
                //set selected buttons back to null
                currentSelectedNumButton2.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = null;
                currentSelectedNumButton2 = null;
                //set operator back to null after operation
                currentSelectedOpButton.GetComponent<Image>().enabled = false;
                currentSelectedOpButton = null;
                CanClickButton = true;
                CheckOver();
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                Anim.Play(currentSelectedNumButton.name + "To" + currentSelectedNumButton2.name);
                StartCoroutine(MoveNumAnimation(solution));
            }

        }
        else if (currentSelectedOpButton.name == "DivButton")
        {
            OperationImage = DivImage;
            //Debug.Log("Division performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            string n2 = currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            //string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            //string n2 = currentSelectedNumButton2.GetComponentInChildren<Text>().text;
            float but1num = float.Parse(n);
            float but2num = float.Parse(n2);
            //float but2num = System.Convert.ToInt32(n2);
            float solution = but1num / but2num;
            //Debug.Log(n + " / " + n2 + " = " + solution);
            //Debug.Log("f = solution%1 = " + solution % 1);
            float f = solution % 1;
            if (f == 0 || f == .25 || f == .5 || f == .75)
            {
                Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
               // Debug.Log("start animate courotine");
                if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
                {
                    //Debug.Log("Shouldnt show animation");
                    currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                    //disable first selected
                    //Debug.Log(currentSelectedNumButton.name + " set active false");
                    currentSelectedNumButton.gameObject.SetActive(false);
                    ButtonsLeft = ButtonsLeft - 1;
                    //set selected buttons back to null
                    currentSelectedNumButton2.GetComponent<Image>().enabled = false;
                    currentSelectedNumButton = null;
                    currentSelectedNumButton2 = null;
                    //set operator back to null after operation
                    currentSelectedOpButton.GetComponent<Image>().enabled = false;
                    currentSelectedOpButton = null;
                    CanClickButton = true;
                    CheckOver();
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    Anim.Play(currentSelectedNumButton.name + "To" + currentSelectedNumButton2.name);
                    StartCoroutine(MoveNumAnimation(solution));
                }

            }
            else
            {
               // Debug.Log("Not a clean division  dont do");
                //play sound
                //shake the card back and forth and dont change anything
                Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
                Anim2 = currentSelectedNumButton2.gameObject.GetComponent<Animation>();
                string s = currentSelectedNumButton.name + "Shake";
                Debug.Log(s);
                Anim.Play(currentSelectedNumButton.name + "Shake");
                Anim2.Play(currentSelectedNumButton2.name + "Shake");
                CanClickButton = true;
            }

        }
        else if (currentSelectedOpButton.name == "PowOfButton")
        {
            OperationImage = PowOfImage;
            //Debug.Log("Pow performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            string n2 = currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            //string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            //string n2 = currentSelectedNumButton2.GetComponentInChildren<Text>().text;
            float but1num = float.Parse(n);
            float but2num = float.Parse(n2);
            if (but1num % 1 != 0 || but2num % 1 != 0)
            {
                //not integers
                //play sound
                //shake the card back and forth and dont change anything
                Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
                string s = currentSelectedNumButton.name + "Shake";
                Debug.Log(s);
                Anim.Play(currentSelectedNumButton.name + "Shake");
                CanClickButton = true;
            }
            else
            {
                float solution = (int)Mathf.Pow(but1num, but2num);
                Debug.Log(n + " ^ " + n2 + " = " + solution);
                Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
                Debug.Log("start animate courotine");
                if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
                {
                    //Debug.Log("Shouldnt show animation");
                    currentSelectedNumButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                    //disable first selected
                    //Debug.Log(currentSelectedNumButton.name + " set active false");
                    currentSelectedNumButton.gameObject.SetActive(false);
                    ButtonsLeft = ButtonsLeft - 1;
                    //set selected buttons back to null
                    currentSelectedNumButton2.GetComponent<Image>().enabled = false;
                    currentSelectedNumButton = null;
                    currentSelectedNumButton2 = null;
                    //set operator back to null after operation
                    currentSelectedOpButton.GetComponent<Image>().enabled = false;
                    currentSelectedOpButton = null;
                    CanClickButton = true;
                    CheckOver();
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    Anim.Play(currentSelectedNumButton.name + "To" + currentSelectedNumButton2.name);
                    StartCoroutine(MoveNumAnimation(solution));
                }

            }

        }
        else if (currentSelectedOpButton.name == "FactButton")
        {
            Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
            OperationImage = FactImage;
            //Debug.Log("Fact op performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            //string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            float but1num = float.Parse(n);
            if (but1num % 1 != 0)
            {
                //not an integer
                //play sound
                //shake the card back and forth and dont change anything
                string s = currentSelectedNumButton.name + "Shake";
                //Debug.Log(s);
                Anim.Play(currentSelectedNumButton.name + "Shake");
                CanClickButton = true;
            }
            else
            {
                int n1 = System.Convert.ToInt32(n);
                for (int i = n1 - 1; i > 1; i--)
                {
                    //Debug.Log(n1 + " X " + i);
                    n1 = n1 * i;
                }
                float solution = n1;
                //Debug.Log("Fact performed on " + currentSelectedNumButton + " for answer of " + n1.ToString());
                if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
                {
                    currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                    //set selected buttons back to null
                    currentSelectedNumButton.GetComponent<Image>().enabled = false;
                    currentSelectedNumButton = null;
                    currentSelectedNumButton2 = null;
                    //set operator back to null after operation
                    currentSelectedOpButton.GetComponent<Image>().enabled = false;
                    currentSelectedOpButton = null;
                    CheckOver();
                    CanClickButton = true;
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    StartCoroutine(MdiSingle(solution));
                }

            }

        }
        else if (currentSelectedOpButton.name == "SqRtButton")
        {
            Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
            OperationImage = SqRtImage;
            //Debug.Log("Sqrt op performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            // string n = currentSelectedNumButton.GetComponentInChildren<Text>().text;
            float n1 = float.Parse(n);
            Debug.Log("sqrt of" + n1 + ": " + Mathf.Sqrt(n1));
            float solution = Mathf.Sqrt(n1);
            if (Mathf.Sqrt(n1) % 1 == 0)
            {
                if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
                {
                    currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                    //set selected buttons back to null
                    currentSelectedNumButton.GetComponent<Image>().enabled = false;
                    currentSelectedNumButton = null;
                    currentSelectedNumButton2 = null;
                    //set operator back to null after operation
                    currentSelectedOpButton.GetComponent<Image>().enabled = false;
                    currentSelectedOpButton = null;
                    CheckOver();
                    CanClickButton = true;
                    Canvas.ForceUpdateCanvases();
                }
                else
                {
                    StartCoroutine(MdiSingle(solution));
                }

            }
            else
            {
                //Debug.Log("Not a perfect square");
                //play sound
                //shake the card back and forth and dont change anything
                string s = currentSelectedNumButton.name + "Shake";
                //Debug.Log(s);
                Anim.Play(currentSelectedNumButton.name + "Shake");
                currentSelectedOpButton.GetComponent<Image>().enabled = false;
                currentSelectedOpButton = null;
                CanClickButton = true;
            }
        }
        else if (currentSelectedOpButton.name == "AbsValButton")
        {
            Anim = currentSelectedNumButton.gameObject.GetComponent<Animation>();
            OperationImage = AbsValImage;
            //Debug.Log("Abs val performed");
            string n = currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            float n1 = float.Parse(n);
            float solution = Mathf.Abs(n1);
            if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 1)
            {
                currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = solution.ToString();
                //set selected buttons back to null
                currentSelectedNumButton.GetComponent<Image>().enabled = false;
                currentSelectedNumButton = null;
                currentSelectedNumButton2 = null;
                //set operator back to null after operation
                currentSelectedOpButton.GetComponent<Image>().enabled = false;
                currentSelectedOpButton = null;
                CheckOver();
                CanClickButton = true;
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                StartCoroutine(MdiSingle(solution));
            }
        }
    }

    public void CheckIfShouldDoOp()
    {
        //Debug.Log("Should do op checked");
        if (CanClickButton)
        {
            if (AllowTwoNumSelected == false && currentSelectedNumButton != null && currentSelectedOpButton != null)
            {
                //Debug.Log("operation called");
                CanClickButton = false;
                PerformOp();
            }
            else if (AllowTwoNumSelected && currentSelectedNumButton != null & currentSelectedNumButton2 != null && currentSelectedOpButton != null)
            {
                //Debug.Log("operation called");
                CanClickButton = false;
                PerformOp();
            }
            else
            {
                //Debug.Log("Shouldnt do op yet");
            }
        }
        else
        {
            Debug.Log("clicked but cant do");
        }

    }

    public void CheckOver()
    {
        if (ButtonsLeft == 1)
        {
            //currentSelectedNumButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;  for tmp
            if (numButton1.gameObject.activeSelf == true)
            {
                if (numButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == solution.ToString())
                {
                    //Debug.Log("Match!  Game over!");
                    Anim = numButton1.gameObject.GetComponent<Animation>();
                    if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 0)
                    {
                        Anim.Play("Num1ButtonWin");
                    }
                    StartCoroutine(OnWin(numButton1));

                }
                else
                {
                    //Debug.Log("Num1 left but not a match");
                }
            }
            else if (numButton2.gameObject.activeSelf == true)
            {
                if (numButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == solution.ToString())
                {
                    //Debug.Log("Match!  Game over!");
                    Anim = numButton2.gameObject.GetComponent<Animation>();
                    if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 0)
                    {
                        Anim.Play("Num2ButtonWin");
                    }
                    StartCoroutine(OnWin(numButton2));
                }
                else
                {
                    //Debug.Log("Num2 left but not a match");
                }
            }
            else if (numButton3.gameObject.activeSelf == true)
            {
                if (numButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == solution.ToString())
                {
                    //Debug.Log("Match!  Game over!");
                    Anim = numButton3.gameObject.GetComponent<Animation>();
                    if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 0)
                    {
                        Anim.Play("Num3ButtonWin");
                    }
                    StartCoroutine(OnWin(numButton3));
                }
                else
                {
                   // Debug.Log("Num3 left but not a match");
                }
            }
            else if (numButton4.gameObject.activeSelf == true)
            {
                if (numButton4.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == solution.ToString())
                {
                   // Debug.Log("Match!  Game over!");
                    Anim = numButton4.gameObject.GetComponent<Animation>();
                    if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 0)
                    {
                        Anim.Play("Num4ButtonWin");
                    }
                    StartCoroutine(OnWin(numButton4));
                }
                else
                {
                    //Debug.Log("Num4 left but not a match");
                }
            }
            else if (numButton5.gameObject.activeSelf == true)
            {
                if (numButton5.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == solution.ToString())
                {
                   // Debug.Log("Match!  Game over!");
                    Anim = numButton5.gameObject.GetComponent<Animation>();
                    if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 0)
                    {
                        Anim.Play("Num5ButtonWin");
                    }
                    StartCoroutine(OnWin(numButton5));
                }
                else
                {
                   // Debug.Log("Num5 left but not a match");
                }
            }

        }
    }

    private void Awake()
    {
        //uncomment when ready to run game
        NumBut1Pos = numButton1.transform.position;
        NumBut2Pos = numButton2.transform.position;
        NumBut3Pos = numButton3.transform.position;
        NumBut4Pos = numButton4.transform.position;
        NumBut5Pos = numButton5.transform.position;
    }


    /*
    public void setBackgroundImage()
    {
        Debug.Log("Setting background image");
        int x = PlayerPrefs.GetInt("Background", 0);
        if (x == 0)
        {
            BackgroundImage.sprite = bg0;
        }
        else if (x == 1)
        {
            BackgroundImage.sprite = bg1;
        }
        else if (x == 2)
        {
            BackgroundImage.sprite = bg2;
        }
        else if (x == 3)
        {
            BackgroundImage.sprite = bg3;
        }
        else if (x == 4)
        {
            BackgroundImage.sprite = bg4;
        }
        else if (x == 5)
        {
            BackgroundImage.sprite = bg5;
        }
    }
    public void setCardImage()
    {
        Debug.Log("Setting card images");
        int x = PlayerPrefs.GetInt("Card", 0);
        if (x == 0)
        {
            Card1Image.sprite = c0;
            Card2Image.sprite = c0;
            Card3Image.sprite = c0;
            Card4Image.sprite = c0;
            Card5Image.sprite = c0;
            Solution.sprite = c0;
        }
        else if (x == 1)
        {
            Card1Image.sprite = c1;
            Card2Image.sprite = c1;
            Card3Image.sprite = c1;
            Card4Image.sprite = c1;
            Card5Image.sprite = c1;
            Solution.sprite = c1;
        }
        else if (x == 2)
        {
            Card1Image.sprite = c2;
            Card2Image.sprite = c2;
            Card3Image.sprite = c2;
            Card4Image.sprite = c2;
            Card5Image.sprite = c2;
            Solution.sprite = c2;
        }
        else if (x == 3)
        {
            Card1Image.sprite = c3;
            Card2Image.sprite = c3;
            Card3Image.sprite = c3;
            Card4Image.sprite = c3;
            Card5Image.sprite = c3;
            Solution.sprite = c3;
        }
        else if (x == 4)
        {
            Card1Image.sprite = c4;
            Card2Image.sprite = c4;
            Card3Image.sprite = c4;
            Card4Image.sprite = c4;
            Card5Image.sprite = c4;
            Solution.sprite = c4;
        }
        else if (x == 5)
        {
            Card1Image.sprite = c5;
            Card2Image.sprite = c5;
            Card3Image.sprite = c5;
            Card4Image.sprite = c5;
            Card5Image.sprite = c5;
            Solution.sprite = c5;
        }
    }
*/


    public IEnumerator OnWin(Button b)
    {
        Debug.Log("On win");
        if (PlayerPrefs.GetInt("OnlineAnimations", 1) == 0)
        {
            //playing win animation
            string n = b.name + "Win";
            float t = Anim[n].length * (1 / Anim[n].speed);
            //Debug.Log("waiting");
            yield return new WaitForSeconds(t);
        }

        //animation of card moving done     play fireworks and sound effect
        if (PlayerPrefs.GetInt("Sound", 0) == 0) //if sound == 0 play
        {
            AudioManager.instance.Play("win");
        }
        //add win into user stats
        //Debug.Log("previos hard wins: " + PlayerPrefs.GetInt("HardWins", 0));
        //PlayerPrefs.SetInt("MultiplayerWins", PlayerPrefs.GetInt("MultiplayerWins", 0) + 1);
        //PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 5);
        /*if (PlayerPrefs.GetString("Username", "GUEST") != "GUEST")
        {
            InfoManager.instance.CallGetWins();
        }*/
        //ScoreScreen.SetActive(true);
        ProblemCompleteScreen.SetActive(true);
        int s = int.Parse(time);
        int timebonus = 3 * (s - (s - (int)t));
        Debug.Log("Solved at " + t + " had " + s + " time bonus + " + timebonus);
        ProblemCompleteAddedScoreText.text = "Score +100 finished at time " + t + " Time bonus +" + timebonus;
        //PhotonNetwork.LocalPlayer.CustomProperties["score"] = (int)PhotonNetwork.LocalPlayer.CustomProperties["score"] + timebonus + 100;
        int sc = (int)PhotonNetwork.LocalPlayer.CustomProperties["score"] + timebonus + 100;
        Hashtable hash = new Hashtable();
        hash.Add("isAdmin", (bool)PhotonNetwork.LocalPlayer.CustomProperties["isAdmin"]);
        hash.Add("questions", (string)PhotonNetwork.LocalPlayer.CustomProperties["questions"]);
        hash.Add("onQuestion", (int)PhotonNetwork.LocalPlayer.CustomProperties["onQuestion"] + 1);
        hash.Add("score", sc);
        hash.Add("time", (string)PhotonNetwork.LocalPlayer.CustomProperties["time"]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        ProblemCompleteTotalScoreText.text = "Total Score: " + sc;
        didntwin = false;
    }
    void Start()
    {
        
        Debug.Log("Game screen lobby: " + PhotonNetwork.CurrentRoom + " playername " + PhotonNetwork.LocalPlayer.NickName);
        UserSettingText.text = "Lobby: " + PhotonNetwork.CurrentRoom.Name;

        //from game manager 

        //setBackgroundImage();
        //setCardImage();

        Button1PS.Stop();
        Button2PS.Stop();
        Button3PS.Stop();
        Button4PS.Stop();
        Button5PS.Stop();
        mdi1.enabled = false;
        mdi2.enabled = false;
        mdi3.enabled = false;
        mdi4.enabled = false;
        mdi5.enabled = false;
        numButton1.GetComponent<TrailRenderer>().enabled = false;
        numButton2.GetComponent<TrailRenderer>().enabled = false;
        numButton3.GetComponent<TrailRenderer>().enabled = false;
        numButton4.GetComponent<TrailRenderer>().enabled = false;
        numButton5.GetComponent<TrailRenderer>().enabled = false;
        ProblemCompleteScreen.SetActive(false);
        SetUpGame();
    }



    public void HomeButtonClick()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Disconnected from photon");
        SceneManager.LoadScene("MainMenu");
    }

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (countdown && t > 0)
        {
            t -= Time.deltaTime;
            int t1 = (int)t;
            TimerText.text = t1.ToString();
        }
        else if(t <= 0)
        {
            TimerText.text = "";
            if (changescene)
            {
                if (didntwin)
                {
                    int sc = (int)PhotonNetwork.LocalPlayer.CustomProperties["score"];
                    Hashtable hash = new Hashtable();
                    hash.Add("isAdmin", (bool)PhotonNetwork.LocalPlayer.CustomProperties["isAdmin"]);
                    hash.Add("questions", (string)PhotonNetwork.LocalPlayer.CustomProperties["questions"]);
                    hash.Add("onQuestion", (int)PhotonNetwork.LocalPlayer.CustomProperties["onQuestion"] + 1);
                    hash.Add("score", sc);
                    hash.Add("time", (string)PhotonNetwork.LocalPlayer.CustomProperties["time"]);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    ProblemCompleteTotalScoreText.text = "Total Score: " + sc;
                }
                PhotonNetwork.LoadLevel("MultiLeaderboard");
                changescene = false;
            }
        }
    }

    public void SetUpGame()
    {
        questions = (string)PhotonNetwork.MasterClient.CustomProperties["questions"];
        time = (string)PhotonNetwork.MasterClient.CustomProperties["time"];
        onQuestion = (int)PhotonNetwork.MasterClient.CustomProperties["onQuestion"];
        //Debug.Log("Questions: " + questions + " Time to solve: " + time);
        //StartGame(questions,time);
        t = float.Parse(time);
        questionarray = questions.Split(' ');
        Debug.Log("question  " + questions);
        numQuestions = questionarray.Length / 6;
        QuesNumText.text = "Question " + onQuestion + "/" + numQuestions;
        int a = 6 * (onQuestion-1);
        int b = 6 * (onQuestion - 1) + 1;
        int c = 6 * (onQuestion - 1) + 2;
        int d = 6 * (onQuestion - 1) + 3;
        int e = 6 * (onQuestion - 1) + 4;
        int f = 6 * (onQuestion - 1) + 5;
        ResetGame(int.Parse(questionarray[a]), int.Parse(questionarray[b]), int.Parse(questionarray[c]),
            int.Parse(questionarray[d]), int.Parse(questionarray[e]), int.Parse(questionarray[f]));
        //Debug.Log("2 3 6 7 5 23 9 3 7 6 8 33 1 2 3 4 5 15");
        //int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }


    //when master client is switched
    public override void OnMasterClientSwitched(Player newMasterClient)
    {

    }

}
