using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BatCardView :BasePanel
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI combatValue;
    public Image myself;
    public Image bk;
    public GameObject switchObj;

    public int myId;
    public bool isEnable=false;
    private void Start()
    {
        myId = this.GetComponent<BatCardModel>().MyId;
        EventListener();
    }
    //数据面板状态更新
    public void ActiveUpdata(bool o)
    {
        EventCenter.GetInstance().EventTrigger(myId + "BatActiveUpdata", o);
    }
    //更新控件数据
    public void UpdateInfo(CardData date)
    {
        this.gameObject.name = date.batName;
        nameText.text = date.batName;
        skillName.text = date.skillName;
        hp.text = date.hp.ToString();
        combatValue.text = date.combatValue.ToString();
    }
    //维持卡牌的自动布局
    private void RestorePostion()
    {
        if (this.transform.parent != null)
        {
            Transform defaultPos = this.transform.parent;
            this.transform.SetParent(null);
            this.transform.SetParent(defaultPos);
        }
    }
    private void EventListener()
    {
        //初始化卡牌状态和数据
        EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "BatInfoInit", UpdateInfo);
        EventCenter.GetInstance().addEventListener<bool>(myId + "BatActiveUpdata", (o) =>
        {
            isEnable = o;
            switchObj.SetActive(o);
        });
        EventCenter.GetInstance().addEventListener<CardData>(myId + "BatInfoUpdata", UpdateInfo);
        ActiveUpdata(isEnable);
        //添加UI事件
        //鼠标进入退出放大缩小
        BaseCard.GetInstance().EnterAndExitCard(myself);
        //拖拽效果
        UIManager.AddCustomEventListener(myself, EventTriggerType.Drag, (obj) =>
        {
            if(isEnable)
            {
                this.transform.position = Input.mousePosition;
            }
        });
        //技能释放逻辑 回合结束摧毁卡牌阶段
        int hpValue = 0;
        EventCenter.GetInstance().addEventListener<int>("DestoryHP", (value) =>
        {
            hpValue = value;
        });
        UIManager.AddCustomEventListener(myself, EventTriggerType.EndDrag, (obj) =>
        {
            if(this.transform.position.y > Screen.height * 2 / 3)
            {
                //释放技能
                UseSkill();
                //摧毁卡牌 
                if (hpValue >= int.Parse(hp.text) && GameCtrl.nowState == Game_State.DestroyBatCard)
                {
                    EventCenter.GetInstance().EventTrigger("DestroyCard", this);
                    EventCenter.GetInstance().EventTrigger("DestoryHP", hpValue - int.Parse(hp.text));
                }
            }
            if (SkillMgr.GetInstance().isSkill)
                EventCenter.GetInstance().EventTrigger("DragTarget", this);
            RestorePostion();


        });
        //结束技能
        EventCenter.GetInstance().addEventListener("EndSkill", () =>
        {
            bk.GetComponent<Outline>().effectColor = Color.black;
        });
        //回合结束冒险失败
        EventCenter.GetInstance().addEventListener("EndFailRound", () =>
        {
            if (isEnable)
            {
                EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "BatInfoInit", UpdateInfo);
                EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", this);
            }
            //重置技能
            bk.color = Color.gray;
        });
        //回合结束冒险成功
        EventCenter.GetInstance().addEventListener("EndWinRound", () =>
        {
            if (isEnable)
            {
                EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "BatInfoInit", UpdateInfo);
                EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", this);
                //重置技能
                bk.color = Color.gray;
            }
        });
    }
    /// <summary>
    /// 技能逻辑
    /// </summary>
    private void UseSkill()
    {
        if (bk.color != Color.white && GameCtrl.nowState != Game_State.DestroyBatCard
            &&!SkillMgr.GetInstance().isSkill)
        {
            bk.color = Color.white;
            switch (skillName.text)
            {
                case "+1生命值":
                    EventCenter.GetInstance().EventTrigger("HP", 1);
                    break;
                case "+2生命值":
                    EventCenter.GetInstance().EventTrigger("HP", 2);
                    break;
                case "+1张牌":
                    EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                    EventCenter.GetInstance().EventTrigger("HP", 1);
                    break;
                case "+2张牌":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                    EventCenter.GetInstance().EventTrigger("HP", 1);
                    SkillMgr.GetInstance().Init(skillName.text);
                    break;
                case "1*摧毁":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    SkillMgr.GetInstance().Init(skillName.text);
                    break;
                case "1*加倍":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    SkillMgr.GetInstance().Init(skillName.text);
                    break;
                case "1*复制":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    SkillMgr.GetInstance().Init(skillName.text);
                    EventCenter.GetInstance().EventTrigger("Cardskilling", this);
                    break;
                case "阶段-1":
                    if (GameCtrl.TotalState != Game_State.State_Green && GameCtrl.TotalState != Game_State.State_Pirate)
                    {
                        EventCenter.GetInstance().EventTrigger("TotalStageChange", --GameCtrl.TotalState);
                        ++GameCtrl.TotalState;
                    } 
                    break;
                case "看3张牌":
                    break;
                case "1*交换":
                    break;
                case "2*交换":
                    break;
                case "1*牌库底":
                    break;
                default:
                    bk.color = Color.gray;
                    break;
            }
            print(skillName.text);
        }
    }
    
}
