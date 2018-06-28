using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class appleLevelManager : MonoBehaviour {

    InterfaceControl interfControl;
    difficultyRegulator diffRegulator;
    difficultyLevels dLevels;
    grandmaMessage messageGrandma;
    AudioScript audioScript;

    public GameObject[] puntosSalida;
    public GameObject ApplePrefab, suelo, fondo, arbol, gameFinal;

    Animator treeAnim, shadowAnim;


    public int numApples, levelScore;
    // bool gameOn, zoomOn;
    float timer, appleVelocity;


    private int diffLevel;
    private string[] syllablesList;
    private Dictionary<string, float> probabilityDictionary;

    string goodSyllable;

    int fails, hits, applesSinceLastGood;
    //public Text marcador;

    Dictionary<string, float> syllablesDictionary;
    public string nextSyllaba;

    float countdownTimer; //Cuenta atrás juego

    string gameState = "Initial";


    void Start() {

        audioScript = FindObjectOfType<AudioScript>();
        countdownTimer = 70;

        applesSinceLastGood = 0;
        //Paraliza el juego cuando la app queda en segundo plano.
        Application.runInBackground = false;

        dLevels = GetComponent<difficultyLevels>();
        messageGrandma = FindObjectOfType<grandmaMessage>();
        diffRegulator = GetComponent<difficultyRegulator>();

        //carga el nivel de difficultad guardado y le restamos uno para comenzar más ligero.
        if (GameData.currentGame.diffLevelOflevels[1] > 1)
        {
            diffLevel = GameData.currentGame.diffLevelOflevels[1] - 1;
        } 
        else { diffLevel = GameData.currentGame.diffLevelOflevels[1]; }

        syllablesList = dLevels.getLevelList();

        goodSyllable = syllablesList[Random.Range(0, syllablesList.Length)];


        probabilityDictionary = new Dictionary<string, float>();
        syllablesDictionary = new Dictionary<string, float>();

        float probabilityUni = 60f / (syllablesList.Length - 1); //Debug.Log("syllables: "+ syllablesList.Length+ " prob: "+probabilityUni);
        float probability = 0;

        foreach (string syll in syllablesList) {

            if (syll == goodSyllable)
            {
                probability += 40f;
                syllablesDictionary.Add(syll, probability);
                probabilityDictionary.Add(syll, 40f);

            }
            else
            {
                probability += probabilityUni;
                syllablesDictionary.Add(syll, probability);
                probabilityDictionary.Add(syll, probabilityUni);

            }

            Debug.Log("Silaba: " + syll + " Probabilidad/100: " + syllablesDictionary[syll] + " Probab: " + probabilityDictionary[syll]);
        }
        Debug.Log("  ");
        setNextSyllable();



        appleVelocity = 2f;
        timer = 0f;
        numApples = 0;
        //gameOn = false;
        // zoomOn = false;

        treeAnim = GameObject.FindGameObjectWithTag("tree").GetComponent<Animator>();
        shadowAnim = GameObject.FindGameObjectWithTag("shadow").GetComponent<Animator>();

        interfControl = GetComponent<InterfaceControl>();

        //gameFinal= GameObject.FindGameObjectWithTag("Finish") ;
        //camera = Camera.main;

    }
    

    void Update() {

        switch (gameState) {

            case "Initial":
                //interfControl.goodAppleIndicator.SetActive(false);
                if (messageGrandma.isMessageEnded())
                {
                    timer += Time.deltaTime;
                    interfControl.goodAppleIndicator.SetActive(true);

                    if (timer >= 1f && Input.GetMouseButtonDown(0))
                    {
                        //gameOn = true;
                        interfControl.goodAppleIndicator.SetActive(false);
                        gameState = "Zoom";
                        timer = 4f;
                    }
                }
                break;

            case "Zoom":
                scenarioZoom();
                break;

            case "GameON":

                //Cuenta atrás juego
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0) {
                    calculateScore();
                    gameState = "GameOFF";
                    break;
                }


                /*
                if (timer > 4)
                {
                    int pos = Random.Range(0, puntosSalida.Length); //Escoge al azar una posición en el arbol para expulsar la manzana.

                    setNextSyllable(); //Escoge una silaba para asignar a la manzana caida.

                    Instantiate(ApplePrefab, puntosSalida[pos].transform.position, Quaternion.identity);
                    treeAnim.SetBool("appleFalling", true);
                    shadowAnim.SetBool("appleFalling", true);
                    diffRegulator.increaseTotalAppes(); //aumenta contador manzanas caidas totales.
                    numApples += 1;
                    timer = Random.Range(0, 3.5f);
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer < 3.5)
                    {
                        treeAnim.SetBool("appleFalling", false);
                        shadowAnim.SetBool("appleFalling", false);
                    }
                }*/
                break;

            case "GameOFF":

                //calculateScore();
                GameData.currentGame.diffLevelOflevels[1] = diffLevel; //Almacenamos el nivel de dificultad del jugador para este nivel.

                gameFinal.SetActive(true); //Activa pantalla de resultados con boton para salir del nivel.
                /*
                if (Input.GetMouseButtonDown(0))
                {
                    FinishLevel();
                }*/

                break;

        }
        /*
        marcador.text = "Busca \n" + goodSyllable + "\n Fallos: " + fails;

        if (!gameOn)
        {
            if (Input.GetMouseButtonDown(0) && messageGrandma.isMessageEnded()) { gameOn = true; }
        }
        else if (!zoomOn)
        {
            scenarioZoom();

        }
        else
        {
            countdownTimer -= Time.deltaTime;

            if (timer > 4)
            {
                int pos = Random.Range(0, puntosSalida.Length);

                setNextSyllable();

                Instantiate(ApplePrefab, puntosSalida[pos].transform.position, Quaternion.identity);
                treeAnim.SetBool("appleFalling", true);
                shadowAnim.SetBool("appleFalling", true);
                diffRegulator.increaseTotalAppes();
                numApples += 1;
                timer = Random.Range(0, 3.5f);
            }
            else
            {
                timer += Time.deltaTime;
                if (timer < 3.5)
                {
                    treeAnim.SetBool("appleFalling", false);
                    shadowAnim.SetBool("appleFalling", false);
                }
            }

        }*/
    }


    void scenarioZoom() {

        if (fondo.transform.localScale.x < 1.2f)
        {
            fondo.transform.localScale = fondo.transform.localScale + new Vector3(0.01f, 0.01f);
        }
        if (arbol.transform.localScale.x < 1.5f)
        {
            arbol.transform.localScale = arbol.transform.localScale + new Vector3(0.01f, 0.01f);
            arbol.transform.position = arbol.transform.position + new Vector3(0f, 0.04f);
        }
        else
        {
            //zoomOn = true;
            gameState = "GameON";
        }
    }

    void calculateScore()
    {
        levelScore = getHits() - getFails();

        if (levelScore > 0)
        {
            GameData.currentGame.coins += levelScore; //Suma del resultado a las monedas totales del jugador.
        }
        else { levelScore = 0; }
    }


    public int getFinalScore() { return levelScore; }


    public void badSelection() {
        fails += 1;
        diffRegulator.addFail();

        audioScript.MakeErrorSound(); //sonido de fallo al seleccionar manzana incorrecta.
    }

    public void goodSelection()
    {
        interfControl.activateBienMessage();
        hits += 1;
        diffRegulator.addHit();
        interfControl.gotApple();

        audioScript.MakeSuccessSound(); //Sonido positivo al acertar manzana.
    }

    public void changeProbability(string syllable) {

        if (probabilityDictionary[syllable] < 50f)
        {

            float variacion = 10f / (syllablesList.Length - 1);
            float ajuste = 0f;
            float probMinima = 2.5f;

            //ACTUALIZA DICCIONARIO PROBABILIDADES

            foreach (string syll in syllablesList)
            {
                if (syll == goodSyllable) { probMinima = 15f; }
                else { probMinima = 2.5f; }

                if (syll == syllable)
                {
                    probabilityDictionary[syll] += 10;
                }
                else if (probabilityDictionary[syll] - variacion < probMinima)
                {

                    ajuste += variacion - (probabilityDictionary[syll] - probMinima);
                    probabilityDictionary[syll] = probMinima;

                }
                else
                {
                    probabilityDictionary[syll] -= variacion;
                }
            }

            probabilityDictionary[syllable] -= ajuste;

            //ACTUALIZA DICCIONARIO PROBABILIDADES SOBRE 100
            float cumulativeProbability = 0; ;

            foreach (string syll in syllablesList)
            {
                cumulativeProbability += probabilityDictionary[syll];
                syllablesDictionary[syll] = cumulativeProbability;

                Debug.Log("Silaba: " + syll + " Probabilidad/100: " + syllablesDictionary[syll] + " Probab: " + probabilityDictionary[syll]);
            }
            Debug.Log("  ");
        }

        /*if (probabilityDictionary[syllable] < 50f) {

            float variacion = 10f / (syllablesList.Length - 1);
            float prob = 0f;

            foreach (string syll in syllablesList)
            {

                Debug.Log("Silaba ANTES: " + syll + " Probabilidad: " + syllablesDictionary[syll]);

                if (syll == syllable)
                {
                    syllablesDictionary[syll] += prob + 10f;
                    prob += 10;
                    probabilityDictionary[syll] += 10f;
                }
                else {

                    if (probabilityDictionary[syll] - variacion > 2.5f || prob - variacion < 97.5)
                    {
                        syllablesDictionary[syll] += prob - variacion;
                        prob -= variacion;
                        probabilityDictionary[syll] -= variacion;
                    }
                    else {

                    }
                }*/


        //Debug.Log("Silaba DESPUES: " + syll + " Probabilidad: " + syllablesDictionary[syll]);
        // }

    }

    //GETTERS

    public string[] getSyllablesList()
    {
        return syllablesList;
    }

    public int getDifficultyLevel()
    {
        return diffLevel;
    }

    public void setDifficultyLevel(int level) { diffLevel = level; }

    public string getGoodSyllable()
    {
        return goodSyllable;
    }

    public int getHits() { return hits; }

    public int getFails() { return fails; }

    public float getAppleVelocity() { return appleVelocity; }

    public int getCountdownTimer() { return (int)countdownTimer; }

    public int getNumApples() { return numApples; }

    public string getGameState() { return gameState; }

    //SETTERS

    public void setAppleVelocity(float velocity) { appleVelocity = velocity; }

    
    //ESCOGE UNA SILABA PARA SER EXPULSADA ATENDIENDO A SU PROBABILIDAD DE SALIR
    public void setNextSyllable()
    {
        if (applesSinceLastGood >= 5) //Obliga a aparecer la manzana buena cada 5 manzanas como minimo.
        {
            nextSyllaba = goodSyllable;
            applesSinceLastGood = 0;
        }
        else
        {
            int probab = Random.Range(0, 100);

            foreach (var syll in syllablesDictionary)
            {
                if (probab <= syll.Value)
                {
                    nextSyllaba = syll.Key;

                    break;
                }
            }
            if (nextSyllaba != goodSyllable) { applesSinceLastGood += 1; }
            else { applesSinceLastGood = 0; }
        }
    }

    public void FinishLevel()
    { 
        //CHARGE MAIN LEVEL
        LoadingBar lbar = GetComponent<LoadingBar>();
        lbar.startLoad(1);
    }


}
