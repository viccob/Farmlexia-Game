using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainDifficultyRegulator : MonoBehaviour {

    trainLevelManager manager;
    trainInterfaceControl interfaceControl;
    float timerPerTrain, MaxTimeToComplete, MaxTimeToCombo ;

    int numberBadCombos, numberGoodCombos;


	void Start () {
        manager = GetComponent<trainLevelManager>();
        interfaceControl = GetComponent<trainInterfaceControl>();
        timerPerTrain = 0f;

        MaxTimeToCombo = 10f;
        MaxTimeToComplete = 20f;

	}
	

	void Update () {

        if (manager.getLevelState() != "GameOFF")
        {
            if (manager.getLevelState() == "InGame")
            {

                timerPerTrain += Time.deltaTime;

                if (timerPerTrain >= MaxTimeToComplete)
                {
                    numberBadCombos += 1;
                    numberGoodCombos = 0;
                    manager.setLevelState("EndTrain");

                    FindObjectOfType<AudioScript>().MakeErrorSound(); //Genera sonido de error al sobrepasar limite de tiempo y perder el tren.
                }
            }
            else
            {
                if (timerPerTrain > 0f) {

                    if (timerPerTrain <= MaxTimeToCombo)
                    {
                        numberGoodCombos += 1;                       
                     
                        switch (numberGoodCombos) {
                            case 1:
                                interfaceControl.activateBienMessage();
                                break;
                            case 2:
                                manager.addComboDouble(); //añade un combo doble al total

                                interfaceControl.activateX2Message();
                                interfaceControl.activateComboMessage();
                                break;
                            case 3:

                                manager.addComboTriple(); //añade combo triple al total

                                interfaceControl.activateX3Message();
                                interfaceControl.activateComboMessage();
                                break;
                        }                       
                    }
                    else if (timerPerTrain < MaxTimeToComplete) {

                        numberGoodCombos = 0;
                        interfaceControl.activateBienMessage();
                    }
                }
                timerPerTrain = 0f;
            }


            if (numberBadCombos >= 3)
            {
                manager.changeDificultyLevel(manager.getDifficultyLevel() - 1);
                numberBadCombos = 0;
            }
            if (numberGoodCombos >= 3)
            {
                manager.changeDificultyLevel(manager.getDifficultyLevel() + 1);
                numberGoodCombos = 0;
            }
        }
        else {
           
        }
	}
}
