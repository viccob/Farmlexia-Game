using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {


    public AudioSource clickAudio, popAudio, popInvAudio, pigAudio, errorAudio, successAudio, trainAudio, waterAudio; 
    public AudioSource[] grandmaVoices;

    //METODOS PUBLICOS QUE REPRODUCEN LOS AUDIOS, listos para ser llamados por otras clases

    public void MakeClickSound() { clickAudio.Play(); }

    public void MakePopSound() { popAudio.Play(); }

    public void MakePopInvSound() { popInvAudio.Play(); }

    public void MakePigSound() { pigAudio.Play(); }

    public void MakeErrorSound() { errorAudio.Play(); }

    public void MakeSuccessSound() { successAudio.Play(); }

    public void MakeTrainSound() { trainAudio.Play(); }

    public void MakeWaterSound() { waterAudio.Play(); }

    public void MakeGrandmaSound() {
        int voice = Random.Range(0, 3);
        grandmaVoices[voice].Play();
    }


}
