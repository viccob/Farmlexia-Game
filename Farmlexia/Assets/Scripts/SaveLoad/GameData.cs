using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



public class GameData : MonoBehaviour {

    public List<GameObject> gridGameObjects;
    public List<int> diffLevelOflevels;

    public string playerName;
    public int coins = 20;
    public Dictionary <int, GameObject> gridObjects;
    public Dictionary<int, float> timeToCollect;
    public bool stationActive, newGame;

    public System.DateTime lastTimeMainScene; //Ultima vez en escenario principal.


    public static GameData currentGame;

    private string filePath;

    void Awake() {

        filePath = Application.persistentDataPath + "/savedGames.gd";


        if (currentGame == null)
        {
            currentGame = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (currentGame != this){
            Destroy(gameObject);
        }
    }

    void Start() {
        Debug.Log(Application.persistentDataPath + "/savedGames.gd");
        Load();
    }


    //GUARDA EL ESTADO DEL JUEGO EN UN ARCHIVO
    public void Save() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        DataToSave data = new DataToSave();

        //DATOS QUE SE ALMACENAN

        data.playerName = playerName;
        data.coins = coins;
        // data.gridObjects = gridObjects; //error con diccionarios
        data.stationActive = stationActive;
        //data.timeToCollect = timeToCollect; //error con diccionarios 
        data.lastTimeMainScene = lastTimeMainScene;

        data.diffLevelOflevels = diffLevelOflevels;

        serializeDictionaries(data); //funcion que permite serializar los diccionarios.


        bf.Serialize(file, data);

        file.Close();

        Debug.Log("Juego guardado");
    }


    //CARGA ARCHIVO DE PARTIDA GUARDADA, SI LA HUBIERA
    void Load() {

        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            DataToSave data = (DataToSave)bf.Deserialize(file);

            //DATOS QUE SE CARGAN
            newGame = false;

            playerName = data.playerName;
            coins = data.coins;
            //gridObjects = data.gridObjects; //error con diccionarios
            stationActive = data.stationActive;
            //timeToCollect = data.timeToCollect; //error con diccionarios
            lastTimeMainScene = data.lastTimeMainScene;

            diffLevelOflevels = data.diffLevelOflevels;

            deserializeDictionaries(data);//funcion que permite deserializar los diccionarios.
                                          //Solucion temporal
                                          // gridObjects = new Dictionary<int, GameObject>();
           // timeToCollect = new Dictionary<int, float>();
            
            file.Close();
        }
        else {

            playerName = "";
            newGame = true;
            coins = 30;
            gridObjects = new Dictionary<int, GameObject>();
            timeToCollect = new Dictionary<int, float>();
            stationActive = false;
            diffLevelOflevels = new List<int>(){ 1, 1, 1};
            //lastTimeMainScene = System.DateTime.Parse("0000-00-00T00:00:00-0:00");
        }
    }


    //ELIMINA ARCHIVO DE PARTIDA GUARDADA
    public void DeleteSavedGame()
    {
        if (File.Exists(filePath)) {
            File.Delete(filePath);
            Load();
        }       
    }


    //PERMITE SERIALIZAR LOS DICCIONARIOS
    void serializeDictionaries(DataToSave data)
    {
        List<int> gridObjectsKeys = new List<int>();
        List<string> gridObjectsValues = new List<string>();
        foreach ( KeyValuePair<int, GameObject> pair in gridObjects) {
            gridObjectsKeys.Add(pair.Key);
            gridObjectsValues.Add(pair.Value.tag);
        }

        List<int> timeToCollectKeys = new List<int>();
        List<float> timeToCollectValues = new List<float>();
        foreach (KeyValuePair<int, float> pair in timeToCollect)
        {
            timeToCollectKeys.Add(pair.Key);
            timeToCollectValues.Add(pair.Value);
        }

         data.gridObjectsKeys = gridObjectsKeys;
         data.gridObjectsValues = gridObjectsValues;
        data.timeToCollectKeys = timeToCollectKeys;
        data.timeToCollectValues = timeToCollectValues;
    }

    //RECUPERA LOS DICCIONARIOS SERIALIZADOS COMO LISTAS
    void deserializeDictionaries(DataToSave data)
    {
        gridObjects = new Dictionary<int, GameObject>();
        timeToCollect = new Dictionary<int, float>();        

        for (int pos=0; pos< data.gridObjectsKeys.Count; pos++)
        {
            foreach (GameObject gObj in gridGameObjects) {
                if (data.gridObjectsValues[pos] == gObj.tag) {
                    gridObjects[data.gridObjectsKeys[pos]] = gObj;
                    
                    break;
                }
            }           
        }

        for (int pos = 0; pos < data.timeToCollectKeys.Count; pos++)
        {
            timeToCollect[data.timeToCollectKeys[pos]] = data.timeToCollectValues[pos];
        }        
    }
}

[System.Serializable]
class DataToSave
{
    public string playerName;
    public int coins;
   // public Dictionary<int, GameObject> gridObjects;
   // public Dictionary<int, float> timeToCollect;
    public bool stationActive;

    public List<int> diffLevelOflevels;

    public List<int> gridObjectsKeys;
    public List<string> gridObjectsValues;
    public List<int> timeToCollectKeys;
    public List<float> timeToCollectValues;

    public System.DateTime lastTimeMainScene;

    /*
        public DataToSave(int coins) {
            this.coins = coins;
        }*/
}

