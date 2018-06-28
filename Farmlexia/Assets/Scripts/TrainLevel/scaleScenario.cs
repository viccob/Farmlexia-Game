using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleScenario : MonoBehaviour {

    trainLevelManager manager;
    int trainSize;
    public Vector3 normalScale, newScale;
    float porcent;


    void Start () {
        manager = FindObjectOfType<trainLevelManager>();

        normalScale = transform.localScale;
        newScale = new Vector3(0,0,0);

        porcent = 1f;
    }
	

	void Update () {

        trainSize = manager.getWagons().Length;


        switch (trainSize)
        {
            case 5:               
                newScale = new Vector3(normalScale.x * 0.8f, normalScale.y * 0.8f, normalScale.z);
                if (porcent < 0.83f) { porcent += 0.005f; }
                else if(porcent > 0.87f){ porcent -= 0.005f; }
                else { porcent = 0.85f; }

                transform.localScale = normalScale * porcent;   

                break;

            case 6:

                newScale = new Vector3(normalScale.x * 0.7f, normalScale.y * 0.7f, normalScale.z);

                if (porcent < 0.73f) { porcent += 0.005f; }
                else if (porcent > 0.77f) { porcent -= 0.005f; }
                else { porcent = 0.75f; }

                transform.localScale = normalScale * porcent;

                break;

            case 7:

                newScale = new Vector3(normalScale.x * 0.7f, normalScale.y * 0.7f, normalScale.z);

                if (porcent < 0.73f) { porcent += 0.005f; }
                else if (porcent > 0.77f) { porcent -= 0.005f; }
                else { porcent = 0.75f; }

                transform.localScale = normalScale * porcent;

                break;


            default:

                if (porcent < 0.98f) { porcent += 0.005f; }
                else if (porcent > 1.02f) { porcent -= 0.005f; }
                else { porcent = 1f; }

                transform.localScale = normalScale * porcent;

                break;
        }
    }
}
