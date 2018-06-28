using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class difficultyLevels : MonoBehaviour {


    private string[][] difLevel = new string[12][];
    

    void Awake () {

        difLevel[0] = new string[] { "ver", "ser", "ber", "mar", "bur" };
        difLevel[1] = new string[] { "p", "d", "q", "b", "a" };
        difLevel[2] = new string[] { "si", "su", "ni", "nu", "is", "so", "in" };
        difLevel[3] = new string[] { "dar", "par", "bat", "pie", "sol", "sal", "las", "los", "ten" };
        difLevel[4] = new string[] { "dri", "dro", "dru", "dra", "dir", "der", "dur", "dor", "dar" };
        difLevel[5] = new string[] { "bro", "bre", "cre", "ble", "bru", "dre", "tre", "cro", "blo", "flo" };
        difLevel[6] = new string[] { "op", "os", "ol", "es", "om", "on", "ob"};
        difLevel[7] = new string[] { "ver", "vez", "cer", "ber", "del", "dre", "ble" };
        difLevel[8] = new string[] { "id", "di", "be", "ed", "ad", "da", "do", "od", "du", "ud" };
        difLevel[9] = new string[] { "m", "n", "ñ", "h", "k" };
        difLevel[10] = new string[] { "o", "i", "e", "a", "u", "á", "ú", "é", "ó"};
        difLevel[11] = new string[] { "la", "le", "li", "lo", "lu", "lé", "al", "el"};

    }

    

    public string[] getLevelList() {
        int pos = Random.Range(0, difLevel.Length);

        return difLevel[pos];
    }

}
