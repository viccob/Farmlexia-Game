using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppleTree : MonoBehaviour
{

    string treeState;

    public GameObject trunk, leaves, shadow, leavesWithApples, alertIcon, rainPrefab;
    //public Text applesTimerText;
    public Sprite appleAlertSprite, waterAlertSprite;
    ParticleSystem treeLights;

    MainSceneManager manager;
    MovableObject movObject;

    bool selected;

    public bool withApples;

    public float timeToCollect = -1;
    float timer;
    
    Animator anim;

    public static float waitingSeconds = 120f; //Tiempo de espera para recolectar tras regado.

    void Start()
    {
        manager = FindObjectOfType<MainSceneManager>();
        movObject = GetComponent<MovableObject>();
        anim = GetComponent<Animator>();

        treeLights = GetComponentInChildren<ParticleSystem>();

        selected = false;
        
        if (timeToCollect == -1) { setTreeState("Collectable"); }

        updateVisualProperties(); //actualizamos valores visuales.
        // timeToCollect = -1f;
        //withApples = true;
        //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    void Update()
    {
        switch (treeState) {

            case "NeedWater":
                if (leavesWithApples.activeSelf) {
                    leavesWithApples.SetActive(false); //Desactivo sprite arbol con manzanas.
                    leaves.GetComponent<SpriteRenderer>().enabled = true;
                }

                if(!treeLights.isStopped) treeLights.Stop(); //Desactivo destellos

                if (manager.getSceneState() != "Placing" && !alertIcon.activeSelf && GetComponent<MovableObject>().objectState != "Selected" ||
                    manager.getSceneState() != "Placing" && alertIcon.GetComponent<SpriteRenderer>().sprite != waterAlertSprite)
                {
                    //Activo icono alerta y establezco el sprite de alerta para riego.
                    alertIcon.GetComponent<SpriteRenderer>().sprite = waterAlertSprite;
                    alertIcon.SetActive(true);
                }
                if (manager.getSceneState() == "Placing" || GetComponent<MovableObject>().objectState == "Selected") { alertIcon.SetActive(false); }

                break;


            case "WaitingApples":

                if (treeLights.isStopped) treeLights.Play(); //Activo destellos.

                if (leavesWithApples.activeSelf)
                {
                    leavesWithApples.SetActive(false);  //Desactivo sprite arbol con manzanas.
                    leaves.GetComponent<SpriteRenderer>().enabled = true;
                }

                if(alertIcon.activeSelf) alertIcon.SetActive(false); //Oculto icono de alerta.

                timeToCollect += Time.deltaTime;
                Debug.Log(timeToCollect);
                if (timeToCollect >= waitingSeconds)
                {                    
                    timeToCollect = -1;

                    anim.SetBool("ApplesON", true); //animacion aparicion manzanas.

                    treeState = "Collectable"; //Cuando el contador alcanza el tiempo de espera cambio a estado 'Collectable'.

                    manager.SaveGame(); //Guardado estado del juego automatico.
                }

                break;


            case "Collectable":

                if (treeLights.isStopped) treeLights.Play(); //Activo destellos.

                if (anim.GetBool("ApplesON"))
                {
                    timer += Time.deltaTime;
                    if (timer >= 1f)
                    {
                        leavesWithApples.SetActive(true); //activo sprite arbol con manzanas.

                        anim.SetBool("ApplesON", false); //fin animacion aparicion manzanas 
                        timer = 0f;
                        //leaves.GetComponent<SpriteRenderer>().sprite = spriteWithApples; //cambio a sprite arbol con manzanas.
                    }
                }
                else if (!leavesWithApples.activeSelf) {
                    //activa sprite arbol con manzanas en casos en los que no ocurre animacion de aparicion de manzanas (carga de escenario guardado).
                    leavesWithApples.SetActive(true);
                    leaves.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (manager.getSceneState() != "Placing" && !alertIcon.activeSelf && GetComponent<MovableObject>().objectState != "Selected" ||
                    manager.getSceneState() != "Placing" && alertIcon.GetComponent<SpriteRenderer>().sprite != appleAlertSprite)
                {
                    //Activo icono alerta y establezco el sprite de alerta para manzanas.
                    alertIcon.GetComponent<SpriteRenderer>().sprite = appleAlertSprite;
                    alertIcon.SetActive(true);
                }

                if (manager.getSceneState() == "Placing" || GetComponent<MovableObject>().objectState == "Selected") { alertIcon.SetActive(false); }
                break;
         
        }
        /*

        if (!withApples)
        {
            if(leavesWithApples.activeSelf)leavesWithApples.SetActive(false); //Desactivo sprite arbol con manzanas.
            timeToCollect += Time.deltaTime;
            Debug.Log(timeToCollect);
            if (timeToCollect >= 20f)
            {
                withApples = true;
                timeToCollect = -1;
                
                anim.SetBool("ApplesON", true); //animacion aparicion manzanas.
            }
        }
        else {
            if (anim.GetBool("ApplesON"))
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    leavesWithApples.SetActive(true); //activo sprite arbol con manzanas.
                    anim.SetBool("ApplesON", false); //fin animacion aparicion manzanas 
                    timer = 0f;
                    //leaves.GetComponent<SpriteRenderer>().sprite = spriteWithApples; //cambio a sprite arbol con manzanas.
                }
            }
            else if(!leavesWithApples.activeSelf){ leavesWithApples.SetActive(true); }
        }
        */

        //Comprobación para la actualizacion de las propiedades visuales.
        if (manager.objectToMove == gameObject || selected
            ||GetComponent<MovableObject>().GetComponent<SpriteRenderer>().color != trunk.GetComponent<SpriteRenderer>().color 
            || GetComponent<MovableObject>().GetComponent<SpriteRenderer>().sortingOrder != trunk.GetComponent<SpriteRenderer>().sortingOrder)
        {
            updateVisualProperties();
        }   
          

        //Muestra y oculta elementos dependiendo del estado del objeto.
        if (GetComponent<MovableObject>().objectState == "Waiting") { shadow.SetActive(true); }
        else { shadow.SetActive(false); }

        treeMovementAnimation(); //animacion arrastre del arbol.

        /*switch (treeState)
        {
            case "Touched":
               manager.zoomIn(transform.position);
               manager.selectionScreen.SetActive(true);
               break;

        }*/


        if (selected)
        {

            if (manager.getSceneState() == "DecisionBox")
            {
                anim.SetBool("Selected", true);
            }
            else
            {
                selected = false;
                anim.SetBool("Selected", false);
            }
        }
       
    }


    public bool collectApples() {
        /*if (withApples) //Si el arbol dispone de manzanas que recolectar.
        {
            timeToCollect = 0f;
            withApples = false;
            Debug.Log("Manzanas recogidas");
            Debug.Log(timeToCollect);
            return true;
        }
        else { return false; }*/

        if (treeState == "Collectable") //Si el arbol dispone de manzanas que recolectar.
        {
            timeToCollect = -2;
            //withApples = false;
            treeState = "NeedWater";
            Debug.Log("Manzanas recogidas");
            Debug.Log(timeToCollect);
            return true;
        }
        else { return false; }
        
    }


    void OnMouseDown() {
        
        if (manager.getSceneState() == "InGame")
        {
            if (treeState == "NeedWater")
            {
                FindObjectOfType<AudioScript>().MakeWaterSound();

                GameObject rain = Instantiate(rainPrefab);
                rain.GetComponentInChildren<ParticleSystem>().GetComponent<Transform>().localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                rain.transform.position = transform.position;

                timeToCollect = 0;
                setTreeState("WaitingApples");

                manager.SaveGame(); //Guardado estado del juego automatico
            }
           /* else
            {
                manager.startDecisionBox(gameObject);
                selected = true;
            }*/
            manager.startDecisionBox(gameObject);
            selected = true;
        }
    }

    //ANIMACION ARBOL EN EL ARRASTRE POR EL ESCENARIO.
    void treeMovementAnimation() {

        if (movObject.objectState == "Selected" && !Input.GetMouseButtonDown(0))
        {
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > (transform.position.x + movObject.getCollider().size.x / 8))
            {
                anim.SetInteger("direction", 1);
            }
            else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < (transform.position.x - movObject.getCollider().size.x / 8))
            {
                anim.SetInteger("direction", 2);
            }
            else { anim.SetInteger("direction", 0); }
        }
        else { anim.SetInteger("direction", 0); }
    }


    //ACTUALIZA CUANDO ES NECESARIO LAS DISTINTAS PROPIEDADES DEL ASPECTO VISUAL DEL OBJETO.
    void updateVisualProperties() {
        trunk.GetComponent<SpriteRenderer>().sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder;
        leaves.GetComponent<SpriteRenderer>().sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder + 1;
        leavesWithApples.GetComponent<SpriteRenderer>().sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder + 2;
        alertIcon.GetComponent<SpriteRenderer>().sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder + 3;
        treeLights.GetComponent<Renderer>().sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder + 3;

        trunk.GetComponent<SpriteRenderer>().color = GetComponentInParent<SpriteRenderer>().color;
        leaves.GetComponent<SpriteRenderer>().color = GetComponentInParent<SpriteRenderer>().color;
        leavesWithApples.GetComponent<SpriteRenderer>().color = GetComponentInParent<SpriteRenderer>().color;

        treeLights.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
    }

    public float getTimeToCollect() {
        return timeToCollect;
    }

    public void setWithApples(bool with) { withApples = with; }
    public bool getWithApples() { return withApples; }

    public void setTreeState(string state) { treeState = state; }
    public string getTreeState() { return treeState; }

}
