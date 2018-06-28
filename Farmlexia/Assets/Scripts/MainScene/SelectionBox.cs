using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionBox : MonoBehaviour {

    Text nameText;
    public Text appleText, collectText;


    public Button playButton;

    MainSceneManager manager;
    //LoadingBar levelLoader;
   

	void Start () {

        manager = FindObjectOfType<MainSceneManager>();
        //levelLoader = FindObjectOfType<LoadingBar>();

        nameText = GetComponentInChildren<Text>();        
        //playButton = GetComponentInChildren<Button>();

        //transform.position = Camera.main.WorldToScreenPoint(new Vector3(0f, 0f, 0f));
        //transform.position = Camera.main.WorldToScreenPoint(manager.decisionObject.transform.position);
        //transform.localScale = new Vector2(0.1f, 0.1f);
        appleText.text = "";
        collectText.text = "";

        switch (manager.decisionObject.tag)
        {

            case "Tree":
                /* if (!manager.decisionObject.GetComponent<AppleTree>().getWithApples()) {
                     playButton.image.enabled = false;
                     //;
                     appleText.text = "" + (int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect() ;
                 }*/
                if (manager.decisionObject.GetComponent<AppleTree>().getTreeState()!="Collectable")
                {
                    playButton.image.enabled = false;
                    //;
                    appleText.text = "" + (int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect();
                    collectText.text = "Tiempo para recolectar";
                }

                nameText.text = "Juego 'El manzano'";
                break;

            case "Station":
                nameText.text = "Juego 'La estación'";
                break;

            default:
                nameText.text = "";
                break;

        }

    }  


	void Update () {
        /*
        if (manager.decisionObject.tag == "Tree" && !manager.decisionObject.GetComponent<AppleTree>().getWithApples()) {
            appleText.text = "Quedan " + (20 - (int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect()) + " segundos para poder recolectar.";
        }*/

        if (manager.decisionObject.tag == "Tree" && manager.decisionObject.GetComponent<AppleTree>().getTreeState() != "Collectable")
        {
            Debug.Log((20-(int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect())/60 +":"+ (20 - (int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect()) % 60);            
            appleText.text = getTimerFromSeconds();
        }
        else if (playButton.image.enabled == false) {
            playButton.image.enabled = true;
            appleText.text = "";
            collectText.text = "";
        }

        //Escala box de 0.1 a 1
        /*  if (transform.localScale.x < 0.97f)
          {
              transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
          }
          else { transform.localScale = new Vector3(1f, 1f, 1f); }
          */
        /*
        //Desplazamiento box desde objeto seleccionado a centro de pantalla en X e Y
        Vector3 centerPoint = Camera.main.WorldToScreenPoint(new Vector3(0f, 0f, 0f));
        Debug.Log(centerPoint); Debug.Log(transform.position.x);
        if (transform.position.x > centerPoint.x + 70f) { transform.position -= new Vector3(1500f, 0f, 0f) * Time.deltaTime; }
        else if (transform.position.x < centerPoint.x - 70f) { transform.position += new Vector3(1500f, 0f, 0f) * Time.deltaTime; }
        else { transform.position = new Vector3(centerPoint.x, transform.position.y, transform.position.z); }

        Debug.Log(centerPoint); Debug.Log(transform.position.x);
        if (transform.position.y > centerPoint.y + 30f) { transform.position -= new Vector3(0, 1000f, 0f) * Time.deltaTime; }
        else if (transform.position.y < centerPoint.y - 30f) { transform.position += new Vector3(0f, 1000f, 0f) * Time.deltaTime; }
        else { transform.position = new Vector3(transform.position.x , centerPoint.y, transform.position.z ); }
        */
    }

    //OBTIENE UN STRING CON MINUTOS Y SEGUNDOS EN FORMATO MM:SS A PARTIR DE LOS SEGUNDOS DE ESPERA DE RECOLECCION DEL ARBOL.
    string getTimerFromSeconds() {
        string min = "00";
        int minInt = ((int)AppleTree.waitingSeconds - (int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect()) / 60;

        if (minInt < 10) { min = "0" + minInt; }
        else { min = "" + minInt; }

        string sec = "00";
        int secInt = ((int)AppleTree.waitingSeconds - (int)manager.decisionObject.GetComponent<AppleTree>().getTimeToCollect()) % 60;

        if (secInt < 10) { sec = "0" + secInt; }
        else { sec = "" + secInt; }

        return  min + ":" + sec;
    }

    public void TakeDecision() { manager.ChargeSelectedLevel(); }

    public void FinishDecisionBox() {

        Destroy(gameObject);
        manager.setSceneState("DecisionBoxEnd");
    }
}
