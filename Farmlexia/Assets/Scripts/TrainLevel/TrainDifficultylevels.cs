using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDifficultylevels : MonoBehaviour
{


    string[][] lowLevelList = new string[18][];
    string[][] intermediateLevelList = new string[19][];
    string[][] highLevelList = new string[22][];

    void Start()
    {

        lowLevelList[0] = new string[] { "H", "O", "L", "A" };
        lowLevelList[1] = new string[] { "P", "A", "L", "O" };
        lowLevelList[2] = new string[] { "C", "A", "M", "A" };
        lowLevelList[3] = new string[] { "R", "A", "N", "A"};
        lowLevelList[4] = new string[] { "A", "Ñ", "O" };
        lowLevelList[5] = new string[] { "M", "A", "R" };
        lowLevelList[6] = new string[] { "P", "E", "Z" };
        lowLevelList[7] = new string[] { "P", "A", "R" };
        lowLevelList[8] = new string[] { "P", "A", "Z" };
        lowLevelList[9] = new string[] { "P", "A", "N" };
        lowLevelList[10] = new string[] { "C", "O", "L" };
        lowLevelList[11] = new string[] { "C", "A", "L" };
        lowLevelList[12] = new string[] { "A", "S", "A" };
        lowLevelList[13] = new string[] { "M", "Á", "S" };
        lowLevelList[14] = new string[] { "N", "O", "T", "A" };
        lowLevelList[15] = new string[] { "T", "O", "S" };
        lowLevelList[16] = new string[] { "I", "S", "L", "A" };
        lowLevelList[17] = new string[] { "A", "G", "U", "A" };


        intermediateLevelList[0] = new string[] { "N", "O", "T", "A" };
        intermediateLevelList[1] = new string[] { "P", "A", "L", "O" };
        intermediateLevelList[2] = new string[] { "A", "L", "T", "O" };
        intermediateLevelList[3] = new string[] { "Á", "R", "B", "O", "L" };
        intermediateLevelList[4] = new string[] { "M", "E", "N", "T", "A" };
        intermediateLevelList[5] = new string[] { "M", "A", "L", "E", "T", "A" };
        intermediateLevelList[6] = new string[] { "C", "A", "M", "I", "Ó", "N" };
        intermediateLevelList[7] = new string[] { "R", "E", "L", "O", "J" };
        intermediateLevelList[8] = new string[] { "J", "A", "M", "Ó", "N" };
        intermediateLevelList[9] = new string[] { "Q", "U", "E", "S", "O" };
        intermediateLevelList[10] = new string[] { "C", "A", "N", "O", "A" };
        intermediateLevelList[11] = new string[] { "S", "U", "E", "Ñ", "O" };
        intermediateLevelList[12] = new string[] { "I", "S", "L", "A" };
        intermediateLevelList[13] = new string[] { "A", "G", "U", "A" };
        intermediateLevelList[14] = new string[] { "R", "A", "T", "Ó", "N" };
        intermediateLevelList[15] = new string[] { "S", "U", "E", "R", "T", "E" };
        intermediateLevelList[16] = new string[] { "A", "R", "A", "Ñ", "A" };
        intermediateLevelList[17] = new string[] { "L", "E", "C", "H", "E" };
        intermediateLevelList[18] = new string[] { "L", "I", "B", "R", "O" };



        highLevelList[0] = new string[] { "A", "L", "T", "O" };
        highLevelList[1] = new string[] { "P", "A", "L", "O" };
        highLevelList[2] = new string[] { "Á", "R", "B", "O", "L" };
        highLevelList[3] = new string[] { "C", "O", "C", "I", "N", "A" };
        highLevelList[4] = new string[] { "M", "A", "L", "E", "T", "A" };
        highLevelList[5] = new string[] { "C", "A", "N", "O", "A" };
        highLevelList[6] = new string[] { "R", "E", "L", "O", "J"};
        highLevelList[7] = new string[] { "S", "U", "E", "R", "T", "E" };
        highLevelList[8] = new string[] { "A", "R", "A", "Ñ", "A" };
        highLevelList[9] = new string[] { "Z", "A", "P", "A", "T", "O" };
        highLevelList[10] = new string[] { "L", "E", "C", "H", "E" };
        highLevelList[11] = new string[] { "A", "R", "M", "A", "R", "I", "O" };
        highLevelList[12] = new string[] { "C", "O", "L", "E", "G", "I", "O" };
        highLevelList[13] = new string[] { "M", "O", "C", "H", "I", "L", "A" };
        highLevelList[14] = new string[] { "P", "L", "Á", "T", "A", "N", "O" };
        highLevelList[15] = new string[] { "H", "E", "R", "M", "A", "N", "O" };
        highLevelList[16] = new string[] { "D", "I", "B", "U", "J", "O" };
        highLevelList[17] = new string[] { "L", "I", "B", "R", "O" };
        highLevelList[18] = new string[] { "A", "L", "T", "O" };
        highLevelList[19] = new string[] { "P", "E", "Q", "U", "E", "Ñ", "O" };
        highLevelList[20] = new string[] { "G", "A", "L", "L", "E", "T", "A" };
        highLevelList[21] = new string[] { "H", "U", "E", "R", "T", "A" };
    }


    public string[][] getLevelList(int level)
    {

        switch (level)
        {
            case 1:
                return lowLevelList;


            case 2:
                return intermediateLevelList;


            case 3:
                return highLevelList;

            default:
                return lowLevelList;

        }
    }
}
