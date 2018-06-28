using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wagon : MonoBehaviour {

    public int numberWagon;
    public wagonBox box;
    public Transform boxPosition;
    public wagon wBefore, wNext;

    Animator wagonAnim;
    public locomotive Locomotive;

    trainGenerator generator;
    string wagonState;

    trainLevelManager manager;

    void Start() {

        manager = FindObjectOfType<trainLevelManager>();

        wagonAnim = GetComponent<Animator>(); wagonAnim.SetBool("Moving", true);
        generator = FindObjectOfType<trainGenerator>();
        wBefore = generator.Wagon;
        generator.Wagon = this;

        wagonState = "Moving";

        if (wBefore != null) { wBefore.wNext = this; }
        else { Locomotive.setNextWagon(this); }

    }

    void Update() {

        switch (wagonState) {

            case "Moving":

                moveWagon();

                if (manager.trainIsMoving == false) {
                    wagonAnim.SetBool("Moving", false);
                    wagonState = "Stopped";
                }
                break;


            case "Stopped":
                if (manager.trainIsMoving == true)
                {
                    wagonAnim.SetBool("Moving", true);
                    wagonState = "Moving";            
                }
                break;
        }   
    }


    void moveWagon() {
        if (wBefore != null)
        {
            transform.position = new Vector3(wBefore.GetComponent<Transform>().position.x + GetComponent<BoxCollider2D>().size.x * transform.localScale.x, transform.position.y, transform.position.z);
        }
        else { transform.position = new Vector3(Locomotive.GetComponent<Transform>().position.x + Locomotive.GetComponent<BoxCollider2D>().size.x * transform.localScale.x, transform.position.y, transform.position.z); }
    }


    public void setBoxToWagon(wagonBox wBox) {
        box = wBox;
    }


    public void DestroyWagons() {
        if (wNext != null)
        {
            wNext.DestroyWagons();
        }
        box.DestroyBox();
        Destroy(gameObject);
    }

   

    //Getters & setters
    public void setNumberWagon(int number) { numberWagon = number; }

    public Vector2 getBoxPosition() { return boxPosition.position; }
    public wagonBox getWagonBox() { return box; }
    public int getNumberWagon() { return numberWagon; }
}
