using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainGenerator : MonoBehaviour {

    public int numWagons;
    public GameObject wagonPrefab, boxPrefab, locomotivePrefab;
    public wagon Wagon;
    public string[] lettersList;
    public string wagonLetter;

    public Transform puntoSalida;

    public wagon[] wagonsList;

	void Start () {
        wagonsList = new wagon[] { };

    }
	


    public void Generate() {
       
        numWagons = lettersList.Length;
        wagonsList = new wagon[numWagons];

        Wagon = null;
       

        GameObject locomotiveGObject = Instantiate(locomotivePrefab, puntoSalida.position, Quaternion.identity);

        for (int i = 0; i < numWagons; i++)
        {

            GameObject wn = Instantiate(wagonPrefab, puntoSalida.position, Quaternion.identity);
            wn.GetComponent<wagon>().setNumberWagon(i+1);
            wagonsList[i]=wn.GetComponent<wagon>();
            if (i == 0) { wn.GetComponent<wagon>().Locomotive = locomotiveGObject.GetComponent<locomotive>(); }
                       
            wagonLetter = lettersList[i];

            wn.GetComponent<wagon>().box = Instantiate(boxPrefab).GetComponent<wagonBox>();
            wn.GetComponent<wagon>().box.boxLetter = wagonLetter;
        }

    }

    //SETTERS

    public void setWord(string [] letters) {
        lettersList = letters;
    }

    public wagon[] getWagonList() { return wagonsList; }

}
