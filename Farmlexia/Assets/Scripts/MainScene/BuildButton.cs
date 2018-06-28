using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {

    public string kind;

    MainSceneManager manager;

    public Image letrero, moneyBackground, conditionBackground;
    public Text nameText, moneyText, conditionText;


	void Start () {

        manager = FindObjectOfType<MainSceneManager>();

        refreshButton();

	}

    public void refreshButton() {

        switch (kind)
        {

            case "treeBtn":

                moneyText.text = "" + manager.GetTreePrize();

                if (GameData.currentGame.coins >= manager.GetTreePrize())
                {
                    GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
                    moneyText.color = Color.white;
                    moneyBackground.color = Color.white;
                }
                else
                {
                    GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0.6f);
                    //moneyBackground.GetComponent<Ima>
                    moneyText.color = Color.red;
                    moneyBackground.color = Color.red;
                }
                break;

            case "stationBtn":

                moneyText.text = "" + manager.GetStationPrize();

                if (GameData.currentGame.coins >= manager.GetStationPrize() && !manager.station.activeSelf && manager.gridTrees.Count > 0)
                {
                    GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
                    moneyText.color = Color.white;
                    moneyBackground.color = Color.white;
                    
                }
                else
                {
                    GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0.6f);
                    //moneyBackground.GetComponent<Ima>
                    if (GameData.currentGame.coins < manager.GetStationPrize())
                    {
                        moneyText.color = Color.red;
                        moneyBackground.color = Color.red;
                    }
                    else {
                        moneyText.color = Color.white;
                        moneyBackground.color = Color.white;
                    }

                    if (manager.gridTrees.Count < 1)
                    {
                        conditionText.color = Color.red;
                        conditionBackground.color = Color.red;
                    }
                    else {
                        conditionText.color = Color.white;
                        conditionBackground.color = Color.white;
                    }
                }
                break;

            case "pigBtn":

                moneyText.text = "" + manager.GetPigPrize();

                if (GameData.currentGame.coins >= manager.GetPigPrize())
                {
                    
                    moneyText.color = Color.white;
                    moneyBackground.color = Color.white;

                    if (!manager.station.activeSelf)
                    {
                        GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0.6f);
                        conditionText.color = Color.red;
                        conditionBackground.color = Color.red;
                    }
                    else {
                        GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
                        conditionText.color = Color.white;
                        conditionBackground.color = Color.white;
                    }
                    
                }
                else
                {
                    GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0.6f);
                    //moneyBackground.GetComponent<Ima>
                    moneyText.color = Color.red;
                    moneyBackground.color = Color.red;

                    if (!manager.station.activeSelf)
                    {                       
                        conditionText.color = Color.red;
                        conditionBackground.color = Color.red;
                    }
                    else
                    {
                        conditionText.color = Color.white;
                        conditionBackground.color = Color.white;
                    }

                }
                break;
        }
    }
}
