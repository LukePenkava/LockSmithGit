using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public GameObject uiWin;
    public GameObject uiLose;


    private void Start()
    {
        Reset();
    }

    public void ShowWinUI(bool show)
    {
        uiWin.SetActive(show);
    }

    public void ShowLoseUI(bool show)
    {
        uiLose.SetActive(show);
    }

    public void Reset()
    {
        ShowWinUI(false);
        ShowLoseUI(false);
    }
}
