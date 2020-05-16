using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    MenuManager menuManager;
    public GameObject visualLevel;
    public TextMeshPro visualNumber;

    public Material mat_Default;
    public Material mat_Finished;

    LevelObject levelData;
    public LevelObject LevelData
    {
        get { return levelData; }
        set { levelData = value; }
    }

    // Start is called before the first frame update
    public void Init(LevelObject data, MenuManager script)
    {
        menuManager = script;
        levelData = data;
        visualLevel.GetComponent<Renderer>().material = data.finished ? mat_Finished : mat_Default;
        visualNumber.text = data.level.ToString();

        if (data.isAd)
        {
            visualNumber.text = "AD";
        }
    }

    // Update is called once per frame
    public void Selected()
    {
        menuManager.SelectLevel(levelData);       
    }
}
