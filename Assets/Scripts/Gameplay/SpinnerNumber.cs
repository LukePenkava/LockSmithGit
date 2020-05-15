using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpinnerNumber : MonoBehaviour
{
    public SpriteRenderer visual;

    [SerializeField] Color colorNormal;
    [SerializeField] Color colorHighlight;

    string type;
    int index;
    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public void Init(int num, string Type)
    {
        type = Type;
        string directory = Helper.GetDirectory(type);
        string spriteName = Helper.GetType(type);
    
        Sprite sprite = Resources.Load<Sprite>("Assets/Sprites/" + directory + "/" + spriteName + "" + num.ToString());
        visual.sprite = sprite;   
        visual.color = colorNormal;    
        index = num;
    }

    public void Locked()
    {
        visual.color = colorHighlight;
        StartCoroutine("ResetColor");
    }

    IEnumerator ResetColor()
    {
        while(visual.color != colorNormal)
        {
            visual.color = Color.Lerp(visual.color, colorNormal, Time.deltaTime * 1f);
            yield return null;
        }
    }
}
