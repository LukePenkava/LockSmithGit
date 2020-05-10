using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static string GetDirectory(Game_Manager.Type type)
    {
        string val = "";

        switch(type)
        {
            case Game_Manager.Type.Numbers:
                val = "Numbers";
                break;
            case Game_Manager.Type.Symbols:
                val = "Symbols";
                break;
            default:
                val = "";                
                break;
        }

        return val;
    }

    public static string GetType(Game_Manager.Type type)
    {
        string val = "";

        switch (type)
        {
            case Game_Manager.Type.Numbers:
                val = "Number";
                break;
            case Game_Manager.Type.Symbols:
                val = "Symbol";
                break;
            default:
                val = "";
                break;
        }

        return val;
    }
}
