using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceMSControl : MonoBehaviour {

    MainSceneManager manager;

    public GameObject coinsImage;

    //botones
    public GameObject btnMove, btnBuild;

    Animator coinAnim;

    float timer =  0f;
	
	void Start () {
        manager = GetComponent<MainSceneManager>();
        coinAnim = coinsImage.GetComponent<Animator>();

	}
	

	void Update () {
        if (timer <= 1f) timer += Time.deltaTime;

        if (timer > 1f) {
            coinAnim.SetBool("Increase", false);
        }

        RefreshButtonSize();
	}


    void RefreshButtonSize() { //Actualiza tamaño botones cuando son pulsados.

        if (manager.getSceneState() == "Placing")
        {
            btnMove.transform.localScale = new Vector3(1.27f, 1.27f, 1.27f);
            btnBuild.transform.localScale = new Vector3(1.06f, 1.06f, 1.06f);
        }
        else if (manager.getSceneState() == "Building")
        {
            btnBuild.transform.localScale = new Vector3(1.27f, 1.27f, 1.27f);
            btnMove.transform.localScale = new Vector3(1.06f, 1.06f, 1.06f);
        }
        else
        {
            btnMove.transform.localScale = new Vector3(1.06f, 1.06f, 1.06f);
            btnBuild.transform.localScale = new Vector3(1.06f, 1.06f, 1.06f);
        }
    }


    public void StartCoinAnimation() {
        timer = 0f;
        coinAnim.SetBool("Increase", true);

    }


    public void SetActiveGridButtons(bool activate)
    {
        btnBuild.SetActive(activate);
        btnMove.SetActive(activate);        
    }
}
