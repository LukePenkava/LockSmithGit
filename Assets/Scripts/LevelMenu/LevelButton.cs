using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    MenuManager menuManager;
    public GameObject visualLevel;
    public GameObject visualAd;
    public GameObject visualNumber;

    public Color activeColor;
    public Color defaultColor;

    public Sprite levelActiveSprite;
    public Sprite levelDefaultSprite;
    public Material grey;
    public Material gold;
    public Material goldAd;
   

    public Material fontDarkMat;

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
        //visualNumber.color = data.finished ? defaultColor : activeColor;
        visualNumber.GetComponent<TextMeshPro>().text = data.level.ToString();
        if(menuManager.CurrentLevel == data.level)
        {
            //visualLevel.GetComponent<SpriteRenderer>().sprite = levelActiveSprite;
            visualLevel.GetComponent<SpriteRenderer>().material = gold;
            visualAd.GetComponent<Renderer>().material = goldAd;
        }
        else
        {
            visualLevel.GetComponent<SpriteRenderer>().sprite = levelActiveSprite;
            visualLevel.GetComponent<SpriteRenderer>().material = grey;
            visualNumber.gameObject.GetComponent<Renderer>().material = fontDarkMat;
            visualAd.GetComponent<Renderer>().material = grey;
        }

        if (data.isAd)
        {
            visualNumber.SetActive(false);
            visualAd.SetActive(true);
        }
        else
        {
            visualNumber.SetActive(true);
            visualAd.SetActive(false);
        }
    }

    // Update is called once per frame
    public void Selected()
    {
        menuManager.SelectLevel(levelData);       
    }
}
