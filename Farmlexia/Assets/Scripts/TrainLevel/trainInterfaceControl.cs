using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class trainInterfaceControl : MonoBehaviour {

    public Text TotalHitsText, countdownTimerText;
    public GameObject bienImage, comboImage, x2Image, x3Image;

    trainLevelManager manager;

    void Start()
    {
        manager = GetComponent<trainLevelManager>();
    }

    void Update()
    {

        TotalHitsText.text = "" + manager.getTrainsCompleted();

        countdownTimerText.text = "" + manager.getCountdownTimer();

    }

    public void activateBienMessage() { Instantiate(bienImage, FindObjectOfType<Canvas>().transform); }
    public void activateComboMessage() { Instantiate(comboImage, FindObjectOfType<Canvas>().transform); }
    public void activateX2Message() { Instantiate(x2Image, FindObjectOfType<Canvas>().transform); }
    public void activateX3Message() { Instantiate(x3Image, FindObjectOfType<Canvas>().transform); }
}
