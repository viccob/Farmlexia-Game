using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locomotive : MonoBehaviour {
    Vector3 puntoParada;
    wagon nextWagon;
    Animator trainAnim;
    string locomotiveState;
    trainLevelManager manager;
    GameObject puntoSalida;
    float timerToCome;

    void Start () {
        puntoSalida = GameObject.FindGameObjectWithTag("startWagonPoint");
        trainAnim = GetComponent<Animator>(); trainAnim.SetBool("Moving", true);
        puntoParada =  Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/ 5f,0,0));
        manager = FindObjectOfType<trainLevelManager>();
        timerToCome = 0f;

        locomotiveState = "Init";
    }
	
	void Update () {

        switch (locomotiveState) {

            case "Init":
                timerToCome += Time.deltaTime;
                if (timerToCome > 0.2f) {
                    locomotiveState = "Coming"; 
                    //Insertar sonido aviso tren                   
                }
                break;


            case "Coming":

                if (transform.position.x > puntoParada.x)
                {
                    transform.position = transform.position - new Vector3(6f, 0, 0) * Time.deltaTime;
                    if (transform.position.y != puntoSalida.transform.position.y ) {
                        transform.position = new Vector3(transform.position.x, puntoSalida.transform.position.y, transform.position.z);
                    }
                }
                else{
                    trainAnim.SetBool("Moving", false);                   
                    manager.trainIsMoving = false;

                    locomotiveState = "Stopped";
                }
                break;


            case "Stopped":

                break;


            case "Leaving":
               
                transform.position = transform.position - new Vector3(9.5f, 0, 0) * Time.deltaTime;                

                break;
        }
	}

    public void TrainDeparture() {
        
        manager.trainIsMoving = true;
        locomotiveState = "Leaving";
        trainAnim.SetBool("Moving", true);
    }

    public void destroyTrain() {
        nextWagon.DestroyWagons();
        Destroy(gameObject);
    }


    //SETTER
    public void setNextWagon(wagon wn) { nextWagon = wn; }
}
