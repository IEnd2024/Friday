using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetAdvCtrl : BasePanel
{
    private Image leftGetPoint;
    private Image rightGetPoint;
    private Button pass;

    void Start()
    {
        leftGetPoint = GetControl<Image>("LeftGetPoint");
        rightGetPoint = GetControl<Image>("RightGetPoint");
        pass = GetControl<Button>("Pass_btn");

        EventListener();
        
        this.gameObject.SetActive(false);
        pass.gameObject.SetActive(false);
    }
    private void EventListener()
    {
        EventCenter.GetInstance().addEventListener<List<AdvCardView>>("GetAdvCard", (objList) =>
        {
            this.gameObject.SetActive(true);
            switch (objList.Count)
            {
                case >= 2:
                    objList[objList.Count - 1].ActiveUpdata(true);
                    BaseCard.GetInstance().TurnOverCard(objList[objList.Count - 1], 
                        objList[objList.Count - 1].IsEnable, leftGetPoint.transform);
                    objList[objList.Count - 2].ActiveUpdata(true);
                    BaseCard.GetInstance().TurnOverCard(objList[objList.Count - 2],
                        objList[objList.Count - 2].IsEnable, rightGetPoint.transform);
                    break;
                case 1:
                    pass.gameObject.SetActive(true);
                    objList[objList.Count - 1].ActiveUpdata(true);
                    BaseCard.GetInstance().TurnOverCard(objList[objList.Count - 1],
                        objList[objList.Count - 1].IsEnable, this.transform);          
                    break;
            }
        });
        EventCenter.GetInstance().addEventListener<AdvCardView>("StartAdv", (obj) =>
        {
            this.gameObject.SetActive(false);
            pass.gameObject.SetActive(false);
        });
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Pass_btn":
                //将冒险卡放入弃牌堆
                EventCenter.GetInstance().EventTrigger("SaveToDiscardOfAdv", this.GetComponentsInChildren<AdvCardView>()[0]);
                //重新变成回合开始阶段
                GameCtrl.nowState=Game_State.Begin;
                EventCenter.GetInstance().EventTrigger("AdvShuffle");
                this.gameObject.SetActive(false);
                pass.gameObject.SetActive(false);
                break;
            
        }
    }


}

    
    
        
    


