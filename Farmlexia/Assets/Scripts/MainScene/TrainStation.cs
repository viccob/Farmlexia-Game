using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStation : MonoBehaviour {

    MainSceneManager manager;

    void Start() {
        manager = FindObjectOfType<MainSceneManager>();

    }


    void OnMouseDown()
    {
        if (manager.getSceneState() == "InGame")
        {
            manager.startDecisionBox(gameObject);
            // FindObjectOfType<LoadingBar>().startLoad(2);
        }
    }
}
