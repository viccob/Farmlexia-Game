using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceControl : MonoBehaviour {


    public GameObject cesta, goodAppleIndicator;
    public Text TotalHitsText, countdownTimerText, goodSyllableText;
    Animator cestaAnim;
    bool isGotApple;

    public Image diffFlame;
    public GameObject bienImage;

    appleLevelManager levelManager;
    AppleGenerator generator;

    float timer;

    // Use this for initialization
    void Start () {

        //cesta = GameObject.FindGameObjectWithTag("cesta");
        cestaAnim = cesta.GetComponent<Animator>();
        isGotApple = false;
        levelManager = GetComponent<appleLevelManager>();
        generator = GetComponent<AppleGenerator>();      

        goodSyllableText.text = "Busca\n" + levelManager.getGoodSyllable();

        goodAppleIndicator.GetComponentInChildren<Text>().text = levelManager.getGoodSyllable();

    }
	
	// Update is called once per frame
	void Update () {

        actualizaMarcador();
        actualizaCountdoenTimer();
        //updateDifficultyBar();      

        if (isGotApple)
        {
            cestaAnimOn();
            //timer = 0;
        }
        else { cestaAnimOff(); }

        if (timer > 2f) { isGotApple = false; }
        else { timer += Time.deltaTime; }
	}


    void cestaAnimOn() { cestaAnim.SetBool("gotApple", true); }
    void cestaAnimOff() { cestaAnim.SetBool("gotApple", false); }

    void actualizaMarcador() { TotalHitsText.text = ""+levelManager.getHits(); }
    void actualizaCountdoenTimer() { countdownTimerText.text = "" + levelManager.getCountdownTimer(); }
    //void updateDifficultyBar() { difficultyBar.fillAmount = generator.difficultyLevel * 0.1f; }

    public void activateBienMessage() { Instantiate(bienImage, FindObjectOfType<Canvas>().transform); }

    public void gotApple() { isGotApple = true; }
}
