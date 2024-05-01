using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestoryCardCrtl : BasePanel
{
    private TextMeshProUGUI hp;
    void Start()
    {
        init();
    }
    private void init()
    {
        
        hp = GetControl<TextMeshProUGUI>("HP");
        EventCenter.GetInstance().addEventListener<int>("DestoryHP", (value) =>
        {
            this.gameObject.SetActive(true);
            hp.text = value.ToString();
            if (value == 0)
            {
                
                GameCtrl.nowState=Game_State.Begin;
                EventCenter.GetInstance().EventTrigger("EndFailRound");
                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId+"EndFailRound");
                this.gameObject.SetActive(false);
            }
        });
        this.gameObject.SetActive(false);
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "EndDestory":
                GameCtrl.nowState = Game_State.Begin;
                EventCenter.GetInstance().EventTrigger("EndFailRound");
                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "EndFailRound");
                this.gameObject.SetActive(false);
                break;
            
        }
    }
}
