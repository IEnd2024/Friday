
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PirateCardView : BasePanel
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI freeCardValue;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI advValue1;
    public Image myself;
    public Image advBK;
    

    private bool isEnable=false;
    private int myId;

    public bool IsEnable { get => isEnable; set => isEnable = value; }
    public int MyId { get => myId; set => myId = value; }

    private void Start()
    {
        myId=this.GetComponent<PirateCardModel>().MyId;
        EventListener();
    }
    /// <summary>
    /// 更新控件数据
    /// </summary>
    /// <param name="date"></param>
    public void UpdateInfo(CardData date)
    {
        this.gameObject.name = date.advName;
        nameText.text = date.advName;
        freeCardValue.text = date.freeCardValue.ToString();
        skillName.text = date.skillName;
        advValue1.text = date.advValue1.ToString();
    }
    /// <summary>
    /// 更新卡牌状态
    /// </summary>
    /// <param name="o"></param>
    public void ActiveUpdata(bool o)
    {
        EventCenter.GetInstance().EventTrigger(myId + "PirateActiveUpdata", o);
    }
    private void EventListener()
    {
        //初始化数据
        EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "PirateInfoInit", UpdateInfo);
        EventCenter.GetInstance().addEventListener<bool>(myId + "PirateActiveUpdata", (o) =>
        {
            isEnable = o;
        });
        EventCenter.GetInstance().addEventListener<CardData>(myId + "PirateInfoUpdata", UpdateInfo);
        ActiveUpdata(isEnable);
        //开始海盗阶段逻辑
        EventCenter.GetInstance().addEventListener("StartPirate", () =>
        {
            EventCenter.GetInstance().EventTrigger(myId + "PirateActiveUpdata", true);
        });
        //添加UI事件
        BaseCard.GetInstance().EnterAndExitCard(myself);
        UIManager.AddCustomEventListener(myself, EventTriggerType.PointerClick, (obj) =>
        {
            if (GameCtrl.TotalState == Game_State.State_Pirate && isEnable
            && GameCtrl.nowState==Game_State.Begin)
            {
                EventCenter.GetInstance().EventTrigger("SelectPirate", this);
                EventCenter.GetInstance().EventTrigger("ShowText", "决战海盗");
                GameCtrl.nowState = Game_State.GetFreeBatCard;
            }
        });
    }
}
