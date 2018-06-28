using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour {

    public string sceneState;

    InterfaceMSControl sceneInterface;
    AudioScript audioScript;

    GridGenerator grid;
    List<GameObject> cellList;
    public List<AppleTree> gridTrees;

    //List<GameObject> gridObjects;
    Dictionary<int, GameObject> gridObjects;
    //Dictionary<int, float> timeToCollectTrees;

    public GameObject cellToOcupate, objectToMove, btRemove, decisionObject;

    public GameObject tree, station, pigPrefab, buildingBar, selectionScreen, sueloFondo, fireworks, selectionBox, shadowSelection, newspaper;

    //Building Bar Buttons
    public GameObject TreeBtn, stationBtn, pigBtn;

    public Text moneyText;

    private int treePrize, stationPrize, pigPrize;

    bool visibleGrid;
    //public bool isRemovableObj;

    LoadingBar levelLoader;

    float ScreenWidth, ScreenHeight;
    int originalOrderSltdObj;

    public static float treeWaitingSeconds = 90f;

    void Awake() {
        //Inicializa el estado del juego dependiendo de si es la primera vez que se juega o no.
        if (GameData.currentGame.newGame) { sceneState = "Newspaper"; }
        else { sceneState = "Initial"; }

        //Permite continuar la ejecucion del juego mientras se tiene la app en segundo plano. Los cronometros no se paran.
        Application.runInBackground = true;

        
        audioScript = FindObjectOfType<AudioScript>();
    }

	void Start () {
        /*
        //Inicializa el estado del juego dependiendo de si es la primera vez que se juega o no.
        if (GameData.currentGame.newGame) { sceneState = "Newspaper"; }
        else { sceneState = "Initial"; }  */      

        grid = FindObjectOfType<GridGenerator>();        

        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        grid.generateGrid();
        cellList = grid.getCellList();
        Camera.main.transform.position = new Vector3(0f, 3.6f, -10f);

        gridTrees = new List<AppleTree>();
        //timeToCollectTrees = new Dictionary<int, float>();

        LoadSavedGrid(); //Carga de objetos salvados en la cuadricula si los hubiera

        visibleGrid = false;
        //isRemovableObj = false;

        cellToOcupate = null;
        objectToMove = null;

        levelLoader = GetComponent<LoadingBar>();

        if (GameData.currentGame.stationActive) { station.SetActive(true); }

        //Precios objetos
        treePrize = 10;
        stationPrize = 75;
        pigPrize = 35;

        sceneInterface = GetComponent<InterfaceMSControl>();

        SaveGame();
    }

	void Update () {

        moneyText.text = "" + GameData.currentGame.coins;

        switch (sceneState) {
            case "Newspaper":
                newspaper.SetActive(true);

                break;

            case "Initial":
                if (Camera.main.transform.position.y > 0f)
                {
                    Camera.main.transform.position -= new Vector3(0f, 1f, 0f) * Time.deltaTime;
                    sueloFondo.transform.position-= new Vector3(0f, 0.3f, 0f) * Time.deltaTime;
                }
                else {
                    Camera.main.transform.position = new Vector3(0f, 0f, Camera.main.transform.position.z);
                   
                    sceneInterface.SetActiveGridButtons(true); //Habilito botones de gestión de la cuadricula        

                    SetActiveGrid(false); //Desactivamos las celdas cuando no tienen funcionalidad                     

                    setSceneState("InGame");
                }
                break;


            case "InGame":

                if (visibleGrid) { showGrid(false); SetActiveGrid(false); }
                buildingBar.SetActive(false);

                break;


            case "Placing":

                if (!visibleGrid) { SetActiveGrid(true); showGrid(true); }
                buildingBar.SetActive(false);


                if (cellToOcupate && objectToMove)
                {
                    adjustToTemporalCell(); //Ajusta objeto a la Z posicion de celda actual y le modifica el color.

                    if (Input.GetMouseButtonUp(0) )
                    {                      
                        changeObjectCell();
                        
                    }
                }
               
                CheckObjectOutOfGrid(); //Función getiona las acciones en objetos arrastrados fuera de la cuadricula. (Papelera, reubicacion, destruccion)

                
                if (objectToMove) {

                    adjustToTemporalScale(); //Ajusta el tamaño de los objetos a su posicion en el escenario.

                    if (!btRemove.activeSelf) //Condicion que habilita e inhabilita boton papelera de objetos.
                    {
                        btRemove.SetActive(true);
                        btRemove.GetComponent<RemoveGridObject>().removable = false;
                    }
                }
                else { btRemove.SetActive(false); }

                break;


            case "Building":

                if (visibleGrid) { showGrid(false); }
                buildingBar.SetActive(true);

                //Salida del modo 'Building' al tocar pantalla fuera del area de la barra de construccion.
                if (Input.GetMouseButtonDown(0) && (Input.mousePosition.y > Screen.height /2f && Input.mousePosition.x < Screen.width * 9/10f)) {
                    setSceneState("InGame");
                }
                break;



            case "DecisionBox":

                if (!shadowSelection.activeSelf) {

                    audioScript.MakeClickSound(); //Genera sonido al salir cuadro de acceder minijuego

                    originalOrderSltdObj = decisionObject.GetComponent<SpriteRenderer>().sortingOrder;
                    decisionObject.GetComponent<SpriteRenderer>().sortingOrder = shadowSelection.GetComponent<SpriteRenderer>().sortingOrder + 1;
                   shadowSelection.transform.position = decisionObject.transform.position;

                    if (decisionObject.tag == "Station") { shadowSelection.transform.localScale = new Vector3(3.88f, 3.88f, 3.88f); }
                    else { shadowSelection.transform.localScale = new Vector3(3.35f, 3.35f, 3.35f); }

                    shadowSelection.SetActive(true);

                    //Inhabilito botones de gestión de la cuadricula
                    sceneInterface.SetActiveGridButtons(false);                    
                    btRemove.SetActive(false);
                }    

                break;

            case "DecisionBoxEnd":
                
                decisionObject.GetComponent<SpriteRenderer>().sortingOrder = originalOrderSltdObj;
                shadowSelection.SetActive(false);

                
                sceneInterface.SetActiveGridButtons(true); //Habilito botones de gestión de la cuadricula.                                                                      

                setSceneState("InGame");

                break;
        }
	}

    //COLOCA OBJETOS EN UNA CELDA DE LA CUADRICULA.
    void changeObjectCell() { //Función que asigna una celda de la cuadricula a un objeto nuevo o la reasigna cuando este ya estaba en la cuadricula.

        setSceneState("InGame");

        MovableObject movObject = objectToMove.GetComponent<MovableObject>();

        bool isNew = true;
        if (movObject.objectCell)
        {            
            movObject.objectCell.setOccupied(false);
            isNew = false;
        }

        //Guardamos en una lista el objeto colocado en escena si no estaba ya en ella.
        if (isNew)
        {
            if (movObject.gameObject.tag == "Tree")
            {
                gridObjects.Add(cellToOcupate.GetComponent<Cell>().getIDCell(), tree);
                GameData.currentGame.gridObjects = gridObjects;

                payTree(); //Se decrementa recursos por el precio del nuevo objeto Tree.
            }
            else if (movObject.gameObject.tag == "Pig") {
                gridObjects.Add(cellToOcupate.GetComponent<Cell>().getIDCell(), pigPrefab);
                GameData.currentGame.gridObjects = gridObjects;

                payPig(); //Se decrementa recursos por el precio del nuevo objeto Pig.
            }
        }
        else {
            if (gridObjects.Remove(movObject.objectCell.getIDCell()))
            {
                if (movObject.gameObject.tag == "Tree") gridObjects.Add(cellToOcupate.GetComponent<Cell>().getIDCell(), tree);            
                else if (movObject.gameObject.tag == "Pig") gridObjects.Add(cellToOcupate.GetComponent<Cell>().getIDCell(), pigPrefab);

                GameData.currentGame.gridObjects = gridObjects;
            }
        }

        movObject.objectCell = cellToOcupate.GetComponent<Cell>();
        movObject.objectCell.setOccupied(true);

        movObject.objectCell.GetComponent<BoxCollider2D>().enabled = false;        
        movObject.transform.position = movObject.objectCell.transform.position;        

        Debug.Log("Hecho");

        cellToOcupate = null; objectToMove = null;

        movObject.adjustToPosition();
        movObject.objectState = "Waiting";
        movObject.getCollider().enabled = true;

        audioScript.MakePopSound(); //Genera sonido al colocar un objeto en la malla

        SaveGame(); //Guardado estado del juego automatico.
    }


    //GESTIONA CASOS CON OBJETOS FUERA DE LA CUADRICULA
    void CheckObjectOutOfGrid() {
        if ((Input.mousePosition.y > Screen.height * 2 / 3f || Input.mousePosition.x > Screen.width * 9 / 10f))
        {
            if (cellToOcupate != null)
            {
                cellToOcupate.GetComponent<Cell>().setNormalSprite();
                cellToOcupate = null;
            }
            else
            {
                if (Input.GetMouseButtonUp(0) && objectToMove)
                {
                    //Caso objeto se tira a la papelera y es eliminado.
                    if (btRemove.GetComponent<Image>().color==Color.red &&  objectToMove.GetComponent<MovableObject>().objectCell) {
                        RemoveObject();
                    }
                    //Caso objeto que estaba en la cuadricula es tirado fuera. Regresa a su sitio.
                    else if (objectToMove.GetComponent<MovableObject>().objectCell)
                    {
                            cellToOcupate = objectToMove.GetComponent<MovableObject>().objectCell.gameObject;
                            changeObjectCell();
                        
                    }
                    else { //Objeto recien creado es soltado fuera de la cuadricula o a la papelera y es eliminado.
                            if (objectToMove.tag == "Tree") { gridTrees.Remove(objectToMove.GetComponent<AppleTree>()); }
                            Destroy(objectToMove);
                            objectToMove = null;
                            setSceneState("InGame");
                        }
                }
            }
        }
    }


    void adjustToTemporalCell()
    {
        objectToMove.GetComponent<SpriteRenderer>().sortingOrder = 45 - cellToOcupate.GetComponent<Cell>().getIDCell();
        cellToOcupate.GetComponent<Cell>().setGreenSprite();        
    }


    //ESCALA EL OBJETO A POSICION EN EL ESCENARIO.
    void adjustToTemporalScale() {

        float movObjScale = ((Input.mousePosition.y * -0.875f / Screen.height) + 1.25f) ; //calcula escala objeto para su posicion.       

        if (objectToMove.transform.localScale.x > movObjScale + 0.02f)
        {
            objectToMove.transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime;
        }
        else if (objectToMove.transform.localScale.x < movObjScale - 0.02f)
        {
            objectToMove.transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime;

        }
    }

    //MUESTRA/OCULTA CUADRICULA DE ESCENARIO.
    void showGrid(bool show) {

        foreach (GameObject cell in cellList) {
            cell.GetComponent<SpriteRenderer>().enabled = show;          
        }
        visibleGrid = show;
    }

    //MUESTRA/OCULTA BARRA DE CONSTRUCCION.
    public void showBuildingBar(bool show) {
        buildingBar.SetActive(show);
    }



    //FUNCIONES CREACION/PAGO DE OBJETOS//

    public void createTree() {
        if (GameData.currentGame.coins >= treePrize)
        {
            sceneState = "Placing";

            GameObject t = Instantiate(tree);
            objectToMove = t;


            //Establezco valores para arboles recien creados.
            objectToMove.GetComponent<AppleTree>().setWithApples(true);
            objectToMove.GetComponent<AppleTree>().timeToCollect = -1;

            gridTrees.Add(objectToMove.GetComponent<AppleTree>()); //Introduce arbol en lista de arboles en escena.

        }
        else { audioScript.MakeErrorSound(); } //Genera sonido de error al clicar sin recursos suficientes
    }

    void payTree() {
        GameData.currentGame.coins -= treePrize;
        sceneInterface.StartCoinAnimation();

        BuildingBarRefresh(); //Actualizamos estado BuildingBar
    }

    public void createPig() {
        if (GameData.currentGame.coins >= pigPrize && station.activeSelf)
        {
            audioScript.MakePopInvSound(); //Genera sonido al crear el objeto

            sceneState = "Placing";

            GameObject t = Instantiate(pigPrefab);
            objectToMove = t;
        }
        else { audioScript.MakeErrorSound(); } //Genera sonido de error al clicar sin recursos suficientes
    }
    void payPig()
    {
        GameData.currentGame.coins -= pigPrize;
        sceneInterface.StartCoinAnimation();

        BuildingBarRefresh(); //Actualizamos estado BuildingBar

    }


    public void activateStation() {
        
        if (GameData.currentGame.coins >= stationPrize && !station.activeSelf && gridTrees.Count > 0)
        {
            station.SetActive(true);
            fireworks.GetComponent<ParticleSystem>().Play();            

            GameData.currentGame.stationActive = true; //Almaceno estado de estación.
            GameData.currentGame.coins -= stationPrize;
            sceneInterface.StartCoinAnimation();

            BuildingBarRefresh(); //Actualizamos estado BuildingBar

            setSceneState("InGame");

            SaveGame(); //Guardado estado del juego automatico.

            audioScript.MakeSuccessSound(); //Genera sonido de exito al construir la estación.
        }
        else { audioScript.MakeErrorSound(); } //Genera sonido de error al clicar sin recursos suficientes
    }
    //-//

    void BuildingBarRefresh() {

        TreeBtn.GetComponent<BuildButton>().refreshButton();
        stationBtn.GetComponent<BuildButton>().refreshButton();
        pigBtn.GetComponent<BuildButton>().refreshButton(); 
        /*//Requisitos manzano se cumplen?
        if (GameData.currentGame.coins >= treePrize) { TreeBtn.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f); }
        else {
            TreeBtn.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0.6f);
            //TreeBtn.GetComponentInChildren<Text>().color = Color.red;
        }

        //Requisitos estacion de tren se cumplen?
        if (GameData.currentGame.coins >= stationPrize) { stationBtn.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f); }
        else {
            stationBtn.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0.6f);
        }*/
    }


    public void startDecisionBox(GameObject decisionObj) {

        decisionObject = decisionObj;
        Instantiate(selectionBox, FindObjectOfType<Canvas>().transform);
        setSceneState("DecisionBox");
    }


    //METODO FUNCIONAMIENTO: PULSAR BOTON DE DESPLAZAMIENTO DE OBJETOS
    public void butonMovement() {
        if (sceneState != "Initial")
        {
            if (sceneState == "Placing") {
                CheckObjectOutOfGrid(); //Comprueba si al cambiar de estado teniamos un objeto arrastrado y actua con el.
                sceneState = "InGame"; }
            else { sceneState = "Placing"; }

            audioScript.MakeClickSound(); //Genera sonido click al tocar boton
        }
    }


    //METODO FUNCIONAMIENTO: PULSAR BOTON DE CONSTRUCCION
    public void buttonBuild()
    {
        if (sceneState != "Initial")
        {
            if (sceneState == "Building") { sceneState = "InGame"; }
            else {
                CheckObjectOutOfGrid(); //Comprueba si al cambiar de estado teniamos un objeto arrastrado y actua con el.
                sceneState = "Building";
            }

            audioScript.MakeClickSound(); //Genera sonido click al tocar boton
        }
    }


    //ELIMINA OBJETOS EN PAPELERA.
    public void RemoveObject() { //Función que elimina objeto localizado en la cuadrícula al arrastrarse a la papelera.
      
        if (sceneState == "Placing" && objectToMove != null) {

            gridObjects.Remove(objectToMove.GetComponent<MovableObject>().objectCell.getIDCell());
            GameData.currentGame.gridObjects = gridObjects;

            if (objectToMove.tag == "Tree") { gridTrees.Remove(objectToMove.GetComponent<AppleTree>()); } //Elimina el arbol de la lista de arboles de escena.

            Destroy(objectToMove);
            objectToMove = null;
                       
            btRemove.SetActive(false);
            //isRemovableObj = false;

            //Al eliminar el ultimo objeto y no tener recursos minimos, se establecen los recursos suficientes para poder comprar la unidad mas barata.
            if (GameData.currentGame.coins < 10 && gridTrees.Count < 1 && !station.activeSelf) {
                GameData.currentGame.coins = 10;
                BuildingBarRefresh();
            } 
           
            setSceneState("InGame");

            SaveGame(); //Guardado estado del juego automatico.
        }
    }


    //CARGA CUADRICULA
    void LoadSavedGrid() { //Función que carga los objetos guardados de la cuadricula cada vez que se inicia la escena.

        if (GameData.currentGame.gridObjects.Count > 0) //Si hay objetos guardados, los carga.
        {
            gridObjects = GameData.currentGame.gridObjects;
            foreach (KeyValuePair<int, GameObject> elemento in GameData.currentGame.gridObjects)
            {
                GameObject obj = Instantiate(elemento.Value);
                obj.GetComponent<MovableObject>().objectCell = cellList[elemento.Key - 1].GetComponent<Cell>();
                obj.GetComponent<MovableObject>().objectCell.setOccupied(true);
                obj.GetComponent<MovableObject>().objectCell.GetComponent<BoxCollider2D>().enabled = false;
                obj.GetComponent<MovableObject>().adjustToPosition();

                //Debug.Log(obj.GetComponent<MovableObject>().objectCell.getIDCell() + " = "+GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()]);
                //Para ARBOLES : Actualiza su estado al cargase (tiempo para recoleccion).
                if (obj.tag == "Tree") { restartTreeProperties(obj); }
            }
        }
        else
        {
            gridObjects = new Dictionary<int, GameObject>();            
        }
        
    }

    //RESTAURA PROPIEDADES ARBOLES CARGADOS.
    void restartTreeProperties(GameObject obj) {

        //Debug.Log(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") - new string());
        // Debug.Log("Entra" + GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()]);

        if (GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()] == -1)
        {
            //obj.GetComponent<AppleTree>().setWithApples(true);
            obj.GetComponent<AppleTree>().setTreeState("Collectable");
            obj.GetComponent<AppleTree>().timeToCollect = -1;
        }
        else if (GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()] == -2)
        {
            obj.GetComponent<AppleTree>().setTreeState("NeedWater");
            obj.GetComponent<AppleTree>().timeToCollect = -2;
        }
        else {

            System.TimeSpan diff = System.DateTime.Now.Subtract(GameData.currentGame.lastTimeMainScene);
            Debug.Log(diff);
            Debug.Log((GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()] + (float)diff.TotalSeconds));


            if ((GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()] + (float)diff.TotalSeconds) >= AppleTree.waitingSeconds)
            {
                //obj.GetComponent<AppleTree>().setWithApples(true);
                obj.GetComponent<AppleTree>().setTreeState("Collectable");
                obj.GetComponent<AppleTree>().timeToCollect = -1;
            }
            else
            {
                //obj.GetComponent<AppleTree>().setWithApples(false);
                obj.GetComponent<AppleTree>().setTreeState("WaitingApples");
                obj.GetComponent<AppleTree>().timeToCollect = GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()] + (float)diff.TotalSeconds;

            }
        }

        //obj.GetComponent<AppleTree>().timeToCollect = GameData.currentGame.timeToCollect[obj.GetComponent<MovableObject>().objectCell.getIDCell()];
        gridTrees.Add(obj.GetComponent<AppleTree>()); //introduzco arbol en lista de arboles de escena.

    }


    //CARGA OTRO ESCENARIO.
    public void ChargeSelectedLevel()
    {

        FindObjectOfType<SelectionBox>().gameObject.SetActive(false); //Oculta pantalla selección 
        switch (decisionObject.tag)
        {

            case "Tree":

                if (decisionObject.GetComponent<AppleTree>().collectApples()) //Recolecta manzanas del arbol seleccionado al cargar nivel.
                {

                    /* Dictionary<int, float> timeToCollectTrees = new Dictionary<int, float>();

                     foreach (AppleTree tree in gridTrees)
                     {
                         timeToCollectTrees.Add(tree.GetComponent<MovableObject>().objectCell.getIDCell(), tree.getTimeToCollect());
                         GameData.currentGame.timeToCollect = timeToCollectTrees;
                     }

                     //Se almacena ultimo momento en mainScene para ajustar propiedades temporales de algunos objetos al recargar escenario.
                     GameData.currentGame.lastTimeMainScene = System.DateTime.Now; */
                    
                    levelLoader.startLoad(2); //Carga nivel 'El manzano'
                }
                break;

            case "Station":
               
                levelLoader.startLoad(3); //Carga nivel 'La estacion'
                break;

        }
        //Actualiza timers recoleccion de arboles.
        Dictionary<int, float> timeToCollectTrees = new Dictionary<int, float>();

        foreach (AppleTree tree in gridTrees)
        {
            timeToCollectTrees.Add(tree.GetComponent<MovableObject>().objectCell.getIDCell(), tree.getTimeToCollect());
            GameData.currentGame.timeToCollect = timeToCollectTrees;
        }

        //Se almacena ultimo momento en mainScene para ajustar propiedades temporales de algunos objetos al recargar escenario.
        GameData.currentGame.lastTimeMainScene = System.DateTime.Now;

        GameData.currentGame.Save();//Guarda estado escena.
    }


    //GUARDA EL ESTADO DEL ESCENARIO
    public void SaveGame() {

        //Actualiza timers recoleccion de arboles.
        Dictionary<int, float> timeToCollectTrees = new Dictionary<int, float>();

        foreach (AppleTree tree in gridTrees)
        {
            timeToCollectTrees.Add(tree.GetComponent<MovableObject>().objectCell.getIDCell(), tree.getTimeToCollect());
            GameData.currentGame.timeToCollect = timeToCollectTrees;
        }

        //Se almacena ultimo momento en mainScene para ajustar propiedades temporales de algunos objetos al recargar escenario.
        GameData.currentGame.lastTimeMainScene = System.DateTime.Now;

        GameData.currentGame.Save();//Guarda estado escena.
    }

    //Cierra el periodico y comienza el juego
    public void CloseNewspaper() {
        GameData.currentGame.newGame = false; //El juego ya no es nuevo.
        newspaper.SetActive(false);
        setSceneState("Initial");

        audioScript.MakeClickSound(); //Genera sonido click al tocar boton
    }

    //FUNCION ACTIVA O DESACTIVA LA CUADRICULA
    void SetActiveGrid(bool isActive) {
        foreach (GameObject cell in cellList) {
            cell.SetActive(isActive);
        }
    }


    //GETTERS & SETTERS
    public void setDescisionObject(GameObject obj) { decisionObject = obj; }

    public void setSceneState(string state) { sceneState = state; }

    public string getSceneState() { return sceneState; }

    public List<GameObject> getCellList() { return cellList; }

    public int GetTreePrize() { return treePrize; }

    public int GetStationPrize() { return stationPrize; }

    public int GetPigPrize() { return pigPrize; }
}
