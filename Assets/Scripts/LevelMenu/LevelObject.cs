using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelObject 
{
    public bool isAd = false;

    public int level = 1;
    public string type = "";
    public int numbersUsed = 1;
    public int combinationLength = 1;
    public float timeStep = 1.0f;
    public bool finished = false;
}
