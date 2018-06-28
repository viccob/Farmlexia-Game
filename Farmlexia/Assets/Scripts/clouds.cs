using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clouds : MonoBehaviour {

    public float velocity;
    Vector2 WorldUnitsInCamera;

    void Start () {
        WorldUnitsInCamera.y = Camera.main.orthographicSize ;
        WorldUnitsInCamera.x = WorldUnitsInCamera.y * Screen.width / Screen.height;
        
    }

	void Update () {

        transform.position = transform.position + new Vector3(velocity, 0);

        //CONTROL RECOLOCACION NUBES
        if (transform.position.x < -WorldUnitsInCamera.x*1.36f)
        {
            transform.position = new Vector3(WorldUnitsInCamera.x*1.36f, transform.position.y);

        }
		
	}
}
