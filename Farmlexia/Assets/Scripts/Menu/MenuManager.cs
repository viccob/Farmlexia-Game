using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {


    LoadingBar loader;
    public GameObject PlayButton, newGameButton, InfoText, inputField;
    public Text askName;

    AudioScript audioScript;

	void Start () {

        audioScript = FindObjectOfType<AudioScript>();
        loader = GetComponent<LoadingBar>();

        if (GameData.currentGame.newGame) //Si no habia partida guardada
        {
            PlayButton.SetActive(false);
            newGameButton.SetActive(true);
            inputField.SetActive(false);
            InfoText.SetActive(false);
            
        }
        else { //si habia partida guardada.
            PlayButton.SetActive(true);
            newGameButton.SetActive(false);
            inputField.SetActive(false);
            InfoText.SetActive(true);

            InfoText.GetComponentInChildren<Text>().text = "de " 
                + GameData.currentGame.playerName + "\n" + GameData.currentGame.lastTimeMainScene.ToString("dd/MM/yyyy");
        }
	}

	void Update () {
        if (GameData.currentGame.newGame) {
            if (inputField.GetComponent<InputField>().text == "")
            {
                PlayButton.GetComponent<Image>().color = new Vector4(1, 1, 1, 0.8f);
            }
            else { PlayButton.GetComponent<Image>().color = Color.white; }
        }
	}

    public void newGameButtonPressed() {
        PlayButton.SetActive(true);
        newGameButton.SetActive(false);
        inputField.SetActive(true);
        InfoText.SetActive(false);

    }


    public void PlayButtonPressed() {

        if (GameData.currentGame.newGame)
        {
            if (inputField.GetComponent<InputField>().text != "")
            {
                audioScript.MakeClickSound(); //Genera sonido de click al clicar boton play cuando este permite crear partida

                Debug.Log(inputField.GetComponent<InputField>().text);
                GameData.currentGame.playerName = inputField.GetComponent<InputField>().text;
                loader.startLoad(1);
            }
            else { audioScript.MakeErrorSound(); }//Genera sonido de error al clicar boton play no funcional
                Debug.Log(inputField.GetComponent<InputField>().text);
        }
        else
        {
            loader.startLoad(1);

            audioScript.MakeClickSound(); //Genera sonido de click al clicar boton play
        }
    }

    public void DeleteGameButton() {
        GameData.currentGame.DeleteSavedGame(); //Borra archivo de datos guardados.

        //Actualizamos visualizacion interfaz.
        PlayButton.SetActive(false);
        newGameButton.SetActive(true);
        inputField.SetActive(false);
        InfoText.SetActive(false);

    }

    public void CloseGameApp() { Application.Quit(); }


}
