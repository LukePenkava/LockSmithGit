using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    //Level Setup
    public static string[] Types = { "Numbers", "Symbols" };
    public static int minNumbers = 4;
    public static int maxNumbers = 6; //7
    public static int minCombinationLength = 4;
    public static int maxCombinationLength = 11;//11
    public static float minTimeStep = 0.8f;
    public static float maxTimeStep = 1.6f;



    public static string GetDirectory(string type)
    {
        string val = "";

        switch(type)
        {
            case "Numbers":
                val = "Numbers";
                break;
            case "Symbols":
                val = "Symbols";
                break;
            default:
                val = "";                
                break;
        }

        return val;
    }

    public static string GetType(string type)
    {
        string val = "";

        switch (type)
        {
            case "Numbers":
                val = "Number";
                break;
            case "Symbols":
                val = "Symbol";
                break;
            default:
                val = "";
                break;
        }

        return val;
    }  
}
