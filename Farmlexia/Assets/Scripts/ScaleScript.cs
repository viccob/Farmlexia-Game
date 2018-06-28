using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleScript : MonoBehaviour {

    public bool scaleHeight = true, scaleWidht = true;
	
	void Start () {

        updateScale();
    }

    //FUNCION ESCALA OBJETOS EN EJES DE X E Y AJUSTANDO A DISTINTAS MEDIDAS DE PANTALLA
    public void updateScale() {

        float newCoef = (float)Screen.width / Screen.height;
        float modelCoef = 1920f / 1080f;

        float widht = transform.localScale.x;
        float height = transform.localScale.y;

        if (scaleWidht) { widht = (transform.localScale.x * newCoef) / modelCoef; }
        if (scaleHeight) { height = (transform.localScale.y * newCoef) / modelCoef; }

        transform.localScale = new Vector3(widht, height, transform.localScale.z);
    }
}
