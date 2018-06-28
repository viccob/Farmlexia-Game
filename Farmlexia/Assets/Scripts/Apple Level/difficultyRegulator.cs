using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class difficultyRegulator : MonoBehaviour {


    int fails, hits, totalApples, temporalApples, temporalFails, temporalHits;
    float appleVelocity;

    AppleGenerator generator;

    appleLevelManager manager;
    
	
	void Start () {
        fails = 0; hits = 0; totalApples = 0; temporalApples = 0;
        appleVelocity = 2f;

        manager = GetComponent<appleLevelManager>();
        generator = GetComponent<AppleGenerator>();
	}
	

	void Update () {
        


	}

    public void RegulateDifficulty() {
        
       // totalApples = generator.getTotalApples(); Debug.Log(totalApples);

        if (temporalApples < 5) {

            temporalApples += 1; Debug.Log(temporalApples);
        }
        else {

            switch (manager.getDifficultyLevel()) {
                case 1 :  
                    if (temporalFails < 1 && temporalHits >= 1) { increaseDiffLevel(); Debug.Log("incrementa"); }
                    else { Debug.Log("No entinc " + totalApples * 10 / 100); }
                    break;
                case 2:
                    if (temporalFails > 0 && temporalHits < 3) { reduceDiffLevel(); Debug.Log("reduce"); }
                    else if (temporalFails < 1 && temporalHits >= 2) { increaseDiffLevel(); Debug.Log("incrementa"); }
                    else { Debug.Log("No entinc " + totalApples * 10 / 100); }
                    break;

                case 3:
                    if (temporalFails > 0 && temporalHits < 3) { reduceDiffLevel(); Debug.Log("reduce"); }
                    else if (temporalFails < 1 && temporalHits >= 2) { increaseDiffLevel(); Debug.Log("incrementa"); }
                    else { Debug.Log("No entinc " + totalApples * 10 / 100); }
                    break;

                case 4:
                    if (temporalFails > 0 && temporalHits < 3) { reduceDiffLevel(); Debug.Log("reduce"); }
                    else if (temporalFails < 1 && temporalHits >= 3) { increaseDiffLevel(); Debug.Log("incrementa"); }
                    else { Debug.Log("No entinc " + totalApples * 10 / 100); }
                    break;

                default:

                    Debug.Log("temporalfails " + temporalFails);
                    if (temporalFails > 0 && temporalHits < 3) { reduceDiffLevel(); Debug.Log("reduce"); }
                    else if (temporalFails < 1 && temporalHits >= 4) { increaseDiffLevel(); Debug.Log("incrementa"); }
                    else { Debug.Log("Permanece"); }
                    break;
            }
            temporalApples = 0;
            temporalFails = 0;
            temporalHits = 0;
        }

    }


    void reduceDiffLevel() {
        if (generator.difficultyLevel > 1) {  manager.setDifficultyLevel(generator.difficultyLevel -= 1); }
    }

    void increaseDiffLevel() {
        if (generator.difficultyLevel < 7) { manager.setDifficultyLevel(generator.difficultyLevel += 1); }
    }


    //SETTERS
    public void addFail() { fails += 1; temporalFails +=1; }
    public void addHit() { hits += 1; temporalHits += 1; }
    public void increaseTotalAppes() { totalApples += 1; }
}
