using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMessage : MonoBehaviour {

    float lifeTimer;

	void Start () {
        lifeTimer = 0f;
	}


	void Update () {
        lifeTimer += Time.deltaTime;
        if (lifeTimer>2) { Destroy(gameObject); }
	}
}
