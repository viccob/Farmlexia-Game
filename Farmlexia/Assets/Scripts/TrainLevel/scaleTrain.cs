using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleTrain : MonoBehaviour {

    trainLevelManager manager;
    int trainSize;

    void Start()
    {
        manager = FindObjectOfType<trainLevelManager>();

        trainSize = manager.getWagons().Length;


        switch (trainSize)
        {
            case 5:
                transform.localScale = new Vector3(transform.localScale.x * 0.8f, transform.localScale.y * 0.8f, transform.localScale.z);
                break;
            case 6:
                transform.localScale = new Vector3(transform.localScale.x * 0.7f, transform.localScale.y * 0.7f, transform.localScale.z);
                break;
            case 7:
                transform.localScale = new Vector3(transform.localScale.x * 0.65f, transform.localScale.y * 0.65f, transform.localScale.z);
                break;
        }
    }

}
