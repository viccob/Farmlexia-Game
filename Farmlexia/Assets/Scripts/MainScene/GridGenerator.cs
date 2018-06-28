using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour {

    public GameObject cell;

    List<GameObject> cellList;

   
	void Start () {

        
        //generateGrid();


    }

	
	// Update is called once per frame
	void Update () {
		
	}


    public void generateGrid() {

        cellList = new List<GameObject>();

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));

        Vector3 AnchoAlto = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)) * 2f;
        float cellWidht = AnchoAlto.x * 9/10f / 9f;
        float cellHeight = AnchoAlto.y * 2 / 3 / 5f;


        float newScaleX = 1f * cellWidht / cell.GetComponent<BoxCollider2D>().size.x;
        float newScaleY = 1f * cellHeight / cell.GetComponent<BoxCollider2D>().size.y;

        float posX = transform.position.x + cellWidht / 2;
        float posY = transform.position.y + cellHeight / 2;
        int numCells = 1;

        for (int c = 0; c < 5; c++)
        {
            for (int f = 0; f < 9; f++)
            {

                GameObject cll = Instantiate(cell, new Vector3(posX, posY, 0f), Quaternion.identity, gameObject.transform);
                cll.transform.localScale = new Vector2(newScaleX, newScaleY);
                cll.GetComponent<Cell>().setIDCell(numCells);

                cellList.Add(cll);

                posX += cellWidht;
                numCells += 1;
            }
            posX = transform.position.x + cellWidht / 2;
            posY += cellHeight;
        }

    }


    public List<GameObject> getCellList() { return cellList; }
}
