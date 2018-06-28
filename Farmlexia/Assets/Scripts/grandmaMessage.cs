using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class grandmaMessage : MonoBehaviour {

    public string Level;
    bool messageEnded;
    public Text messageText;
    int sentencePos;
    float timer;

    Animator GMessageAnim;

    string[] message;
    string goodSyllable;

    AudioScript audioScript;

    void Awake() { audioScript = FindObjectOfType<AudioScript>(); }

	void Start () {
        messageEnded = false;
        timer = 0f;

        audioScript.MakeGrandmaSound(); //Genera sonido efecto abuela hablando al entrar en escena.

        switch (Level) {

            case "AppleLevel":
                                
                message = new string[] {"Necesito manzanas para mi tarta. \nRecoge las que caigan del árbol. ","Pero recoge solo aquellas manzanas \nque tengan la sílaba correcta.", "Las demás manzanas estarán malas."};
                break;

            case "TrainLevel":

                message = new string[] { "¡Chu-chú! Está a punto de llegar un tren de mercancías.", "Va a ir cargado de cajas con nuestros productos.", "Las cajas tienen letras asignadas. \nPero hay un problema... Están desordenadas.", "ORDENA las cajas para FORMAR PALABRAS.", "Así el tren podrá partir con nuestra \nmercancía y la podremos vender fuera." };
                break;

            case "CakeLevel":

                message = new string[] { "", "" };
                break;
        }

        messageText.text = message[sentencePos];

        GMessageAnim = GetComponent<Animator>();
        
    }


    void Update() {       

        if (Input.GetMouseButtonDown(0) && timer >= 1.5f)
        {           
            sentencePos++;
            if (sentencePos < message.Length )
            {
                messageText.text = message[sentencePos];
                GMessageAnim.SetBool("speakAgain", true);

                audioScript.MakeGrandmaSound(); //Genera sonido efecto abuela hablando.
            }
            else
            {                
                GMessageAnim.SetBool("isEnded", true);
                messageText.text = "";
                messageEnded = true;
                timer = 0f;
                
            }
        }
        else { GMessageAnim.SetBool("speakAgain", false); }



        timer += Time.deltaTime;
        if (messageEnded)
        {            
            if (timer >= 2.9f)
            {
                Destroy(gameObject);
            }
        }
       
    }

    
    //GETTER
    public bool isMessageEnded() { return messageEnded; }
}
