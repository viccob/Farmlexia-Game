using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainFinalScreen : MonoBehaviour {

    public Text coinsText, finalHitsText, comboDobleText, comboTripleText;
    trainLevelManager manager;

    void Start()
    {

        manager = FindObjectOfType<trainLevelManager>();
    }


    void Update()
    {

        finalHitsText.text = "X" + manager.getTrainsCompleted();       

        coinsText.text = "Monedas:\n " + manager.getFinalScore();

        comboDobleText.text = "Doble x " + manager.getComboDoble();

        comboTripleText.text = "Triple x " + manager.getComboTriple();
    }
}
