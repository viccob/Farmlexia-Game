using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finalScreenLevel : MonoBehaviour {

    public Text coinsText, finalHitsText, finalFailsText;
    appleLevelManager manager;
    
	void Start () {

        manager = FindObjectOfType<appleLevelManager>();       
    }
	

	void Update () {

        finalHitsText.text = "X" + manager.getHits(); 
        finalFailsText.text = "Fallos: " + manager.getFails();

        coinsText.text = "Monedas: " + manager.getFinalScore() ;
    }
}
