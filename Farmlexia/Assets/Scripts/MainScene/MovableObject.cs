using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {


    public string objectState;
    BoxCollider2D objectCollider;

    public Cell objectCell, newCell;
    public bool changePossible;

    SpriteRenderer objectSprite;

    MainSceneManager manager;

    AudioScript audioScript;

	void Start () {
        objectState = "Selected";

        changePossible = false;

        objectCollider = GetComponent<BoxCollider2D>();
        manager = FindObjectOfType<MainSceneManager>();

        objectSprite = GetComponent<SpriteRenderer>();

        audioScript = FindObjectOfType<AudioScript>();
        

        if (objectCell)
        {
            transform.position = objectCell.transform.position;
            objectState = "Waiting";
        }
        else
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetComponent<SpriteRenderer>().sortingOrder = 51;
        }
    }
	

	void Update () {

        if (manager.getSceneState() == "Placing")
        {
            switch (objectState)
            {
                case "Selected":

                    Vector2 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                    transform.position = newPosition;

                    objectSprite.color = new Vector4(objectSprite.color.r, objectSprite.color.g, objectSprite.color.b, 1f);
                    objectCollider.enabled = false;


                    if (objectCell) { objectCell.setOccupied(false); objectCell.GetComponent<BoxCollider2D>().enabled = true; objectCell.setNormalSprite(); }

                    //transform.localScale = new Vector2(1.1f, 1.1f);

                    break;


                case "Waiting":

                    
                    if (objectCell.objectSprite.color.a == 0.9f) {
                        objectSprite.color = new Vector4(objectSprite.color.r, objectSprite.color.g, objectSprite.color.b, 0.8f);
                        objectCollider.enabled = false;
                    }
                    else {
                        objectSprite.color = new Vector4(objectSprite.color.r, objectSprite.color.g, objectSprite.color.b, 1f);
                        objectCollider.enabled = true;
                    }                    
                    break;
            }
        }
        else {
            objectSprite.color = new Vector4(objectSprite.color.r, objectSprite.color.g, objectSprite.color.b, 1f);
            objectCollider.enabled = true;

            if (objectCell == null) { Destroy(gameObject); } }
	}


    void changeObjectPosition() {

        if (objectCell != null) { objectCell.setOccupied(false); }
        objectCell = newCell;
        objectCell.setOccupied(true);
        objectCell.setNormalSprite();

        adjustToPosition();
    }


    public void adjustToPosition() {

        //Ajuste Z position en funcion de las celdas en que se coloque el objeto
        GetComponent<SpriteRenderer>().sortingOrder = 45 - objectCell.getIDCell();

        //Ajuste escala del objeto en funcion celdas donde se coloque (simulación distancia)
        transform.localScale = new Vector2(1f - (int)(objectCell.getIDCell() / 9.1f)  / 10f, 1f - (int)(objectCell.getIDCell() / 9.1f ) / 10f);
       
    }


    void OnMouseDown()
    {
        if (manager.getSceneState() == "Placing" && !manager.objectToMove) {
            objectState = "Selected";
            transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            manager.objectToMove = gameObject;

            audioScript.MakePopInvSound(); //genera sonido al seleccionar objeto para mover
        }
    }

    public BoxCollider2D getCollider() { return objectCollider; }
}
