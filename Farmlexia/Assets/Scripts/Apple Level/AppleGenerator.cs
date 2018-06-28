using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGenerator : MonoBehaviour {

    float timeBetweenApples, timer;

    public GameObject applePrefab;
    public GameObject[] treePositions;

    Animator treeAnim, shadowAnim;

    public int difficultyLevel , totalApples;
    int lastPos;

    appleLevelManager manager;
    difficultyRegulator regulator;

	void Start () {


        manager = GetComponent<appleLevelManager>();
        regulator = GetComponent<difficultyRegulator>();
        
        treeAnim = GameObject.FindGameObjectWithTag("tree").GetComponent<Animator>();
        shadowAnim = GameObject.FindGameObjectWithTag("shadow").GetComponent<Animator>();

        difficultyLevel = 1;
        totalApples = 0;
        lastPos = -1;
    }
	

	void Update () {

        if (manager.getGameState() == "GameON")
        {
            


            timer += Time.deltaTime;
            CreateApple();

        }
		
	}

    void CreateApple() {
        if (timer >= timeBetweenApples)
        {
            
            int pos = Random.Range(0, treePositions.Length); //Escoge al azar una posición en el arbol para expulsar la manzana.
            if (pos != lastPos)
            {
                manager.setNextSyllable(); //Escoge una silaba para asignar a la manzana caida.
                Instantiate(applePrefab, treePositions[pos].transform.position, Quaternion.identity); //Instancia de la manzana.
                totalApples += 1;

                //Animaciones arbol al expulsar manzana.
                treeAnim.SetBool("appleFalling", true); shadowAnim.SetBool("appleFalling", true);
                // diffRegulator.increaseTotalAppes(); //aumenta contador manzanas caidas totales.

                UpdateTBAandSpeed();
                timer = 0f;

                lastPos = pos;

                FindObjectOfType<AudioScript>().MakePopSound(); //Genera ruido al salir cada manzana.
            }
            else { CreateApple(); }
        }
        else if (timer >= 0.5f) {
            treeAnim.SetBool("appleFalling", false);
            shadowAnim.SetBool("appleFalling", false);
        }
        
    }


    //ACTUALIZA LA VELOCIDAD Y TIEMPO DE SALIDA (TBA) DE CADA MANZANA en cada llamada.
    void UpdateTBAandSpeed() {

        regulator.RegulateDifficulty(); //llamada a un metodo la clase que regula la dificultad para actualizar diff. Level
        difficultyLevel = manager.getDifficultyLevel();

        switch (difficultyLevel)
        {
            case 1:
                Debug.Log("diff. 1");
                timeBetweenApples = Random.Range(3f, 6f);
                manager.setAppleVelocity(2f);

                break;

            case 2:
                Debug.Log("diff. 2");
                timeBetweenApples = timeBetweenApples = Random.Range(2f, 4f);
                manager.setAppleVelocity(3f);

                break;

            case 3:
                Debug.Log("diff. 3");
                timeBetweenApples = timeBetweenApples = Random.Range(0f, 3f);
                manager.setAppleVelocity(3f);

                break;

            case 4:
                Debug.Log("diff. 4");
                timeBetweenApples = timeBetweenApples = Random.Range(0f, 2f);
                manager.setAppleVelocity(4f);

                break;

            case 5:
                Debug.Log("diff. 5");
                timeBetweenApples = timeBetweenApples = Random.Range(0f, 2f);
                manager.setAppleVelocity(5f);

                break;

            case 6:
                Debug.Log("diff. 6");
                timeBetweenApples = timeBetweenApples = Random.Range(0f, 1.5f);
                manager.setAppleVelocity(5.5f);

                break;

            case 7:
                Debug.Log("diff. 7");
                timeBetweenApples = timeBetweenApples = Random.Range(0f, 1f);
                manager.setAppleVelocity(7f);

                break;

        }


    }

    public void SetDifficulty(int dLevel) { difficultyLevel = dLevel; }

    public int getTotalApples() { return totalApples; }

}
