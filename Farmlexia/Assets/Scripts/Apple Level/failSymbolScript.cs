using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class failSymbolScript : MonoBehaviour {

    float timer;

	void Start () {
        timer = 0;
	}
	
	
	void Update () {
        timer += Time.deltaTime;

        if (timer >= 1.5f) Destroy(gameObject);
	}
}
