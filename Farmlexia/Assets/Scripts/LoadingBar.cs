using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour {

    public GameObject barraCarga;
    public Slider sliderBar;

    private AsyncOperation asyn;

    public void startLoad(int level) {
        barraCarga.SetActive(true);

        StartCoroutine(LoadLevelSlider(level));
    }

    IEnumerator LoadLevelSlider(int lvl) {
        asyn = SceneManager.LoadSceneAsync(lvl);
        while (!asyn.isDone /*&& barraCarga.GetComponent<RectTransform>().position.y < 0f*/) {
            //Debug.Log("Scena completada");
            sliderBar.value = asyn.progress;
            

            yield return null;
        }
        
    }
}
