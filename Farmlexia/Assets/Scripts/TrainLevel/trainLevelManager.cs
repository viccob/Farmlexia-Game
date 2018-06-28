using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainLevelManager : MonoBehaviour {

    string levelState;

    public ParticleSystem firework01, firework02;

    trainGenerator TrainGenerator;
    wordTester WordTester;
    TrainDifficultylevels dlevels;
    grandmaMessage messageGrandma;
    public GameObject gameFinal;

    string[][] lettersList; //= new string[5][];
    string[] wordList, orderedWord;

    public wagon[] wagonsList;

    float timer, countdownTimer;
    public bool trainIsMoving;
    int numberTrainsCompleted, levelScore, comboDoble, comboTriple;
   
    private int level;
    

    void Start () {

        /*lettersList[0]= new string [] { "H", "O", "L", "A" };
        lettersList[1] = new string[] { "P", "A", "L", "O" };
        lettersList[2] = new string[] { "A", "L", "T", "O" };
        lettersList[3] = new string[] { "A", "R", "B", "O","L" };
        lettersList[4] = new string[] { "M", "A", "L", "E", "T", "A" };*/

        //Paraliza el juego cuando la app queda en segundo plano.
        Application.runInBackground = false;

        level = 1;

        dlevels = GetComponent<TrainDifficultylevels>();
        lettersList = dlevels.getLevelList(level);

        TrainGenerator = GetComponent<trainGenerator>();
        WordTester = GetComponent<wordTester>();
        messageGrandma = FindObjectOfType<grandmaMessage>();
        countdownTimer = 70f;

        levelState = "Initial";
    }
	

	void Update () {

        switch (levelState) {

            case "Initial":
                if (messageGrandma.isMessageEnded())
                {
                    levelState = "NewTrain";
                }
                break;


            case "NewTrain":
                
                refreshWord();          
                TrainGenerator.lettersList = disorderWord();
                TrainGenerator.Generate();
                trainIsMoving = true;

                wagonsList = TrainGenerator.getWagonList();

                FindObjectOfType<AudioScript>().MakeTrainSound();

                levelState = "InGame";
                break;


            case "InGame":

                if (WordTester.isCorrectWord()) {
                    levelState = "EndTrain";
                    numberTrainsCompleted += 1;

                    firefireworks();
                    FindObjectOfType<AudioScript>().MakeSuccessSound(); //Genera audio de felicitación al ordenar una palabra.
                }

                countdownTimer -= Time.deltaTime;
                //Debug.Log(countdownTimer);
                if ((int)countdownTimer <= 0f) {
                    levelState = "GameOFF";
                    calculateScore();
                }
                break;


            case "EndTrain":

                locomotive train = FindObjectOfType<locomotive>();
                train.TrainDeparture();

                timer += Time.deltaTime;
                if (timer > 2f) {
                    train.destroyTrain();
                    timer = 0f;

                    levelState = "NewTrain";
                }
                break;


            case "GameOFF":

                //calculateScore();
                gameFinal.SetActive(true); //Activa pantalla de resultados con boton para salir del nivel.
                /*
                if (Input.GetMouseButtonDown(0)) {

                    FinishLevel();
                }*/
                break;
        }    
	}



    void refreshWord() {

        int i= Random.Range(0, lettersList.Length);

        wordList = (string[])lettersList[i].Clone();
        orderedWord = (string[])lettersList[i].Clone();
        
    }


    string[] disorderWord()
    {
        int pos1 = Random.Range(0, getWordList().Length);
        int pos2 = Random.Range(0, getWordList().Length);

        if (pos1 != pos2)
        {
            string aux = wordList[pos1];
            wordList[pos1] = wordList[pos2];
            wordList[pos2] = aux;

            if (Random.Range(0, 2) == 1)
            {
                return disorderWord();
            }
            else
            {
                for (int i = 0; i < wordList.Length; i++)
                {
                    Debug.Log(wordList[i] + " == " + orderedWord[i]);
                    if (wordList[i] != orderedWord[i]) {
                        return wordList;
                    }
                }
                return disorderWord();
                /*
                if (wordList == orderedWord) { Debug.Log("iguales");  return disorderWord(); }
                //if (wordList != orderedWord) { Debug.Log(wordList+" "+orderedWord); return wordList;  }
                //else { return disorderWord(); } 
                else
                {
                    
                    return wordList;
                }*/
            }
        }
        else { return disorderWord(); }
    }

    
    void firefireworks() {

        firework01.Play();
        firework02.Play();
        //firework03.Play();
    }


    public void changeDificultyLevel(int newDifficultylevel) {
        if (newDifficultylevel >= 1 && newDifficultylevel <= 3) { 
            level = newDifficultylevel;
            lettersList = dlevels.getLevelList(level);
        }
    }

    public void FinishLevel()
    {
        //CHARGE MAIN LEVEL
        LoadingBar lbar = GetComponent<LoadingBar>();
        lbar.startLoad(1);
    }


    public int getDifficultyLevel() { return level; }


    void calculateScore() {
        levelScore = getTrainsCompleted() + comboTriple * 9 + comboDoble * 4;

        if (levelScore > 0)
        {
            GameData.currentGame.coins += levelScore; //Suma del resultado a las monedas totales del jugador.
        }
    }

    public int getFinalScore() { return levelScore; }


    public string[] getWordList() {
        return wordList;
    }

    public string[] getOrderedWord()
    {       
        return orderedWord;
    }

    public wagon[] getWagons() { return wagonsList; }

    public int getTrainsCompleted() { return numberTrainsCompleted; }

    public int getCountdownTimer() { return (int)countdownTimer; }

    public string getLevelState() { return levelState; }

    public void setLevelState(string state) { levelState = state; }

    public void addComboDouble() { comboDoble += 1; }

    public void addComboTriple() { comboTriple += 1;  comboDoble -= 1; }

    public int getComboDoble() { return comboDoble; }

    public int getComboTriple() { return comboTriple; }
}
