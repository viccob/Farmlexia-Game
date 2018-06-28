using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public int idCell;

    public Sprite green, normal, red;
    public SpriteRenderer objectSprite;

    bool occupied;

    MainSceneManager manager;
    
	void Start () {
        objectSprite = GetComponent<SpriteRenderer>();
        manager = FindObjectOfType<MainSceneManager>();
	}

    void Update() {

        if (occupied && manager.getSceneState()=="Placing") { setRedSprite(); }

        adjustCellTransparency(); //Varia la transparencia de la celda en funcion a su cercania al area de seleccion del jugador.

    }


    void adjustCellTransparency() {

        if (objectSprite.sprite != green)
        {
            if (manager.cellToOcupate)
            {
                int IDOcupateCell = manager.cellToOcupate.GetComponent<Cell>().getIDCell();

                if (IDOcupateCell == idCell + 1 || IDOcupateCell == idCell - 1 || IDOcupateCell == idCell + 9 || IDOcupateCell == idCell - 9)
                {
                    if (objectSprite.sprite == red) { objectSprite.color = setAlphaColor(0.9f); }
                    else { objectSprite.color = setAlphaColor(1f); }
                }
                else if (IDOcupateCell == idCell + 10 || IDOcupateCell == idCell - 10 || IDOcupateCell == idCell + 8 || IDOcupateCell == idCell - 8)
                {
                    if (objectSprite.sprite == red) { objectSprite.color = setAlphaColor(0.9f); }
                    else { objectSprite.color = setAlphaColor(0.7f); }
                }
                else if (objectSprite.sprite == normal)
                {
                    objectSprite.color = setAlphaColor(0.2f);
                }
                else { objectSprite.color = setAlphaColor(0.6f); }

            }
            else
            {
                if (objectSprite.sprite == normal)
                {
                    objectSprite.color = setAlphaColor(0.3f);
                }
                else { objectSprite.color = setAlphaColor(0.6f); }
            }
        }
        else { objectSprite.color = setAlphaColor(1f); }

    }

    Vector4 setAlphaColor(float a) { return new Vector4(objectSprite.color.r, objectSprite.color.g, objectSprite.color.b, a); }


    void OnMouseOver() {
        if (manager && manager.getSceneState() == "Placing" && !occupied && manager.objectToMove && Input.GetMouseButton(0)) {

            if (manager.cellToOcupate) { manager.cellToOcupate.GetComponent<Cell>().setNormalSprite(); }
            manager.cellToOcupate = gameObject;
                
         }
    }
     
    

    //GETTERS & SETTERS

    public void setIDCell(int id) {idCell = id;}

    public int getIDCell() { return idCell; }

    public void setGreenSprite() { objectSprite.sprite = green; }
    public void setRedSprite() { objectSprite.sprite = red; }
    public void setNormalSprite() { objectSprite.sprite = normal; }

    public void setOccupied(bool occd) { occupied = occd; }
    public bool isOccupied() { return occupied; }
}
