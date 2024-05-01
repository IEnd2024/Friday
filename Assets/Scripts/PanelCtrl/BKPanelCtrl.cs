using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BKPanelCtrl : BasePanel
{
    // 静态实例变量  
    private static BKPanelCtrl _instance;
    // 私有构造函数，防止外部直接实例化  
    private BKPanelCtrl() { }
    // 静态属性，用于获取单例实例  
    public static BKPanelCtrl Instance
    {
        get
        {
            return _instance;
        }
    }
    //唤醒时获取单例实例
    private void OnEnable()
    {
        _instance = this;
    }

    private Image startAdvPoint;
    private Image freeBatPoint;
    private Image batPoint;
    private Button endRound_btn;
    private Button left_btn;
    private Button right_btn;

    public string left_text;
    public string right_text;
    public UnityAction leftAction;
    public UnityAction rightAction;
    
    void Start()
    { 
        Init();
    }

    public void Init()
    {
        startAdvPoint = GetControl<Image>("StartAdvPoint");
        freeBatPoint = GetControl<Image>("FreeBatPoint");
        batPoint= GetControl<Image>("BatPoint");
        endRound_btn = GetControl<Button>("EndRound");
        left_btn = GetControl<Button>("Left_btn");
        right_btn = GetControl<Button>("Right_btn");
        left_text = "使用技能";
        right_text = "取消";
        left_btn.GetComponentInChildren<TextMeshProUGUI>().text = left_text;
        right_btn.GetComponentInChildren<TextMeshProUGUI>().text = right_text;
        endRound_btn.gameObject.SetActive(false);
        right_btn.gameObject.SetActive(false);
        left_btn.gameObject.SetActive(false);
        EventCenter.GetInstance().addEventListener<AdvCardView>("StartAdv", (obj) =>
        {
            //固定使用的冒险卡位置
            BaseCard.GetInstance().TurnOverCard(obj, obj.IsEnable, startAdvPoint.transform);
            //显示回合结束按钮
            endRound_btn.gameObject.SetActive(true);
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
                endRound_btn.gameObject.SetActive(false);
                 
                break;
            case "Left_btn":
                leftAction();
                break;
            case "Right_btn":
                rightAction();
                break;
        }
    }

}
