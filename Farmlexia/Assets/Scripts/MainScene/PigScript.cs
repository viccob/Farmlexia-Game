using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigScript : MonoBehaviour
{
    public GameObject pigHead, pigBody, pigShadow;

    Animator anim;

    MainSceneManager manager;
    AudioScript audioScript;

    void Start()
    {
        anim = GetComponent<Animator>();

        anim.Play(0, gameObject.layer, Random.Range(0, 0.9f)); //Inicializa animacion en distintas posiciones de tiempo.

        manager = FindObjectOfType<MainSceneManager>();
        audioScript = FindObjectOfType<AudioScript>();

    }

    void Update()
    {
        updateVisualProperties();
    }

    //ACTUALIZA LAS DISTINTAS PROPIEDADES DEL ASPECTO VISUAL DEL OBJETO.
    void updateVisualProperties()
    {
        pigBody.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        pigHead.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 2;
        pigShadow.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder -  1;

        pigBody.GetComponent<SpriteRenderer>().color = GetComponentInParent<SpriteRenderer>().color;
        pigHead.GetComponent<SpriteRenderer>().color = GetComponentInParent<SpriteRenderer>().color;
        pigShadow.GetComponent<SpriteRenderer>().color = GetComponentInParent<SpriteRenderer>().color;

        //oculta sombra cuando se arrastra.
        if (GetComponent<MovableObject>().objectState == "Selected") { pigShadow.GetComponent<SpriteRenderer>().enabled = false; }
        else { pigShadow.GetComponent<SpriteRenderer>().enabled = true; }
        
    }

    void OnMouseDown() {
        if (manager.getSceneState() != "Placing") {
            anim.SetInteger("PigState", 1);
            if (!audioScript.pigAudio.isPlaying) { audioScript.MakePigSound(); }
        }
        else {
            anim.SetInteger("PigState", 2);           
        }

    }

    void OnMouseUp() {
        anim.SetInteger("PigState", 0);
        if (audioScript.pigAudio.isPlaying) { audioScript.pigAudio.Stop() ; }
    }
}
