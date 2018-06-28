using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wagonBox : MonoBehaviour {

    public wagon wagon, posibleWagon;
    public string boxState, boxLetter;
    public bool changePossible, changeBoxes;
    
    
    

    trainGenerator generator;
    
    TextMesh boxText;

    void Start()
    {
        
        generator = FindObjectOfType<trainGenerator>();
        wagon = generator.Wagon;

        transform.position = wagon.getBoxPosition();
        boxState = "Waiting";
        changeBoxes = false; changePossible = false;

        //boxLetter = "A";
        boxText = GetComponentInChildren<TextMesh>();
        boxText.text = boxLetter;
    }


    void Update()
    {      
        switch (boxState) {

            case "Waiting":

                if (transform.localPosition != new Vector3(wagon.getBoxPosition().x, wagon.getBoxPosition().y))
                {
                    moveBoxToWagon(wagon);
                }
                else { transform.localPosition = wagon.getBoxPosition(); }


                /*if (transform.localPosition.y > wagon.getBoxPosition().y)
                {
                    transform.localPosition -= new Vector3(0f, 10f) * Time.deltaTime;
                }
                else { transform.localPosition = wagon.getBoxPosition();  }*/

                
                break;


            case "Selected":
                Vector2 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                transform.position = newPosition;
  
                break;


            case "Drop":
                moveBoxToWagon(wagon);
  
                break;


            case "Host":

                if (transform.localPosition.y < wagon.getBoxPosition().y + 2f)
                {
                    transform.localPosition += new Vector3(0f, 10f) * Time.deltaTime;
                }
                else if (transform.localPosition.y > wagon.getBoxPosition().y + 2f) {
                    transform.localPosition = new Vector3(transform.localPosition.x, wagon.getBoxPosition().y + 2f, transform.localPosition.z);
                }
                break;
        }
        //INTERCAMBIO DE VAGONES ENTRE CAJAS
        if (changeBoxes) {

            wagon auxW = wagon;
            setWagonToBox(posibleWagon.GetComponent<wagon>());
            wagon.getWagonBox().setWagonToBox(auxW);
            auxW.setBoxToWagon(wagon.getWagonBox());
            wagon.setBoxToWagon(this);

            auxW.getWagonBox().boxState = "Drop";
            boxState = "Drop";

            FindObjectOfType<AudioScript>().MakePopSound(); //genero sonido al colocar una caja.

            changeBoxes = false;
        }
      

     /*   if (changeBoxes)
        {
            fixTimer += Time.deltaTime;
            if (fixTimer >= 0.5f) { boxState = "Drop"; changeBoxes = false; }
        }
        else { fixTimer = 0f;}*/
    }


    void OnMouseDown()
    {       
        boxState = "Selected";

        FindObjectOfType<AudioScript>().MakePopInvSound(); //genero sonido que ocurre al coger una caja.
    }


    void OnMouseUp()
    {
        if (changePossible)
        {
            changeBoxes = true;
        }
        else { boxState = "Drop"; }
    }


    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.tag == "wagon" && boxState == "Selected" && otro.GetComponent<wagon>() != wagon)
        {
            changePossible = true;
            posibleWagon = otro.GetComponent<wagon>();
            posibleWagon.GetComponent<wagon>().getWagonBox().boxState = "Host";
        }
    }

    void OnTriggerStay2D(Collider2D otro) {

        //Intercambio caja de un vagon a otro

        if (otro.tag == "wagon" && boxState == "Selected" && otro.GetComponent<wagon>() != wagon)
        {
            changePossible = true;
            posibleWagon = otro.GetComponent<wagon>();
            posibleWagon.GetComponent<wagon>().getWagonBox().boxState = "Host";
            /*otro.GetComponent<wagon>().getWagonBox().boxState = "Host";

          
            ///
            textBugs.GetComponent<Text>().text = "Letra: " + boxLetter + "\nBoxState: " + boxState + "\nchangePosible: " + changePossible + "\nchangeboxes: " + changeBoxes+"*";
            ///
            if (changeBoxes)
            {

                wagon auxW = wagon;
                setWagonToBox(otro.GetComponent<wagon>());
                wagon.getWagonBox().setWagonToBox(auxW);
                auxW.setBoxToWagon(wagon.getWagonBox());
                wagon.setBoxToWagon(this);
                              
                auxW.getWagonBox().boxState = "Drop";
                boxState = "Drop";

                changeBoxes = false;
                //auxW.getWagonBox().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            ///
            textBugs.GetComponent<Text>().text = "Letra: " + boxLetter + "\nBoxState: " + boxState + "\nchangePosible: " + changePossible + "\nchangeboxes: " + changeBoxes+"*";
            ///*/
        }
    }


    void OnTriggerExit2D(Collider2D otro)
    {

        if (otro.tag == "wagon"  && otro.GetComponent<wagon>() != wagon)
        {
            // otro.GetComponent<wagon>().getWagonBox().transform.position -= new Vector3(0, 1f);
            // otro.GetComponent<wagon>().getWagonBox().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //changePossible = false;
            //otro.GetComponent<wagon>().getWagonBox().boxState = "Waiting";
            otro.GetComponent<wagon>().getWagonBox().boxState = "Drop";
        }
        changePossible = false;
    }



    void moveBoxToWagon(wagon wn) {
        var heading = new Vector3(wagon.getBoxPosition().x, wagon.getBoxPosition().y) - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        int itera = 0;

        if (distance >= 0.5f)
        {
            transform.Translate(direction*2/3);
            Debug.Log(itera+=1);
        }
        else {
            transform.localPosition = wagon.getBoxPosition();
            boxState = "Waiting";
            Debug.Log("Hecho");
            
        }
    }

    public void DestroyBox() { Destroy(gameObject); }

    //SETTERS & GETTERS

    public string getboxLetter() { return boxLetter; }


    public void setWagonToBox(wagon wn)
    {
        wagon = wn;
    }

    
   
}
