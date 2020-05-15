using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationNumber : MonoBehaviour
{
    public SpriteRenderer visual;
    string type = "";
    float scale = 0.2f;

    public void Init(string Type)
    {
        type = Type;
        string directory = Helper.GetDirectory(type);

        Sprite sprite = Resources.Load<Sprite>("Assets/Sprites/" + directory + "/Default");
        visual.sprite = sprite;
        visual.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetNumber(int val)
    {
        string directory = Helper.GetDirectory(type);
        string spriteName = Helper.GetType(type);

        Sprite sprite = Resources.Load<Sprite>("Assets/Sprites/" + directory + "/" + spriteName + "" + val.ToString());
        visual.sprite = sprite;
    }

    public void Reset()
    {
        Init(type);
    }
}
