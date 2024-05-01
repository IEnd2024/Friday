using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BKPanelCtrl : BasePanel
{
    private Image startAdvPoint;
    private Image freeBatPoint;
    private Image batPoint;
    private Button endRound;
    void Start()
    { 
        Init();
    }

    public void Init()
    {
        startAdvPoint = GetControl<Image>("StartAdvPoint");
        freeBatPoint = GetControl<Image>("FreeBatPoint");
        batPoint= GetControl<Image>("BatPoint");
        endRound = GetControl<Button>("EndRound");
        endRound.gameObject.SetActive(false);
        
        EventCenter.GetInstance().addEventListener<AdvCardView>("StartAdv", (obj) =>
        {
            //固定使用的冒险卡位置
            BaseCard.GetInstance().TurnOverCard(obj, obj.IsEnable, startAdvPoint.transform);
            //显示回合结束按钮
            endRound.gameObject.SetActive(true);
        });
        UIManager.GetInstance().ShowPanel<FreeBatPanelCtrl>("Panel/FreeBatPanel", E_UI_Layer.Mid, (panel) =>
        {
            panel.transform.position = freeBatPoint.transform.position;
            EventCenter.GetInstance().addEventListener<BatCardView>("GetFreeBatCard", (obj) =>
            {
                obj.ActiveUpdata(true);
                BaseCard.GetInstance().TurnOverCard(obj, obj.isEnable, panel.transform);

            });
            
        });
        UIManager.GetInstance().ShowPanel<BatPanelCtrl>("Panel/BatPanel", E_UI_Layer.Mid, (panel) =>
        {
            panel.transform.position = batPoint.transform.position;
            EventCenter.GetInstance().addEventListener<BatCardView>("GetBatCard", (obj) =>
            {
                obj.ActiveUpdata(true);
                BaseCard.GetInstance().TurnOverCard(obj, obj.isEnable, panel.transform);

            });

        });
        
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "EndRound":
                //结算回合 计算血量 是否销毁卡牌 是否获得战斗卡
                EventCenter.GetInstance().EventTrigger("EndRoundOfHp");
                endRound.gameObject.SetActive(false);

                break;
            case "AdvDiscardName_btn":

                break;
        }
    }

}
