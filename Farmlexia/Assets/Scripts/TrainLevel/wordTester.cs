using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wordTester : MonoBehaviour {

    trainLevelManager manager;
    wagon[] wagons;
    
	
	void Start () {
        manager = GetComponent<trainLevelManager>();
        
	}

	void Update () {
        wagons = manager.getWagons();
       // if (isCorrectWord()) { Debug.Log("BIEN"); }
    }

    public bool isCorrectWord() {
        for (int i = 0; i < wagons.Length; i++)
        {
            if (!wagons[i].box.getboxLetter().Equals(manager.getOrderedWord()[i]))
            {
                return false;
            }
        }
        return true;
    }
}
