using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStageShowCtrl : BasePanel
{
    private TextMeshProUGUI text;
    void Start()
    {
        Init();
    }

    private void Init()
    {
        text = GetControl<TextMeshProUGUI>("ShowText");

        EventCenter.GetInstance().addEventListener<string>("ShowText", ShowLogic); 
        Hide();

    }
    private void ShowLogic(string value)
    {
        this.gameObject.SetActive(true);
        text.text = value;
        Invoke("Hide", 1.5f);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

   
}
