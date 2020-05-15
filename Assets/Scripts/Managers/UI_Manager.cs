using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public GameObject uiWin;
    public GameObject uiLose;
    public GameObject uiFinished;


    private void Start()
    {
        Reset();
    }

    public void ShowWinUI(bool show)
    {
        uiWin.SetActive(show);
        uiFinished.SetActive(true);
    }

    public void ShowLoseUI(bool show)
    {
        uiLose.SetActive(show);
        uiFinished.SetActive(true);
    }

    public void Reset()
    {
        ShowWinUI(false);
        ShowLoseUI(false);
        uiFinished.SetActive(false);
    }
}
