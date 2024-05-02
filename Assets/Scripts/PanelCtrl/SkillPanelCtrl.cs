using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillPanelCtrl : BasePanel
{
    private TextMeshProUGUI myName;
    private Button left_btn;
    private Button right_btn;
    private Image cardPoint;

    public int listLimitCount=0;
    public UnityAction listCtrl;
    private UnityAction leftClickAction;
    private UnityAction rightClickAction;
    public List<BatCardView> batFreeCards=new List<BatCardView>();
    public List<BatCardView> batCards=new List<BatCardView>();
    private void OnEnable()
    {
        myName = GetControl<TextMeshProUGUI>("name");
        left_btn = GetControl<Button>("Left_btn");
        right_btn = GetControl<Button>("Right_btn");
        cardPoint = GetControl<Image>("SkillCardPile");

        EventCenter.GetInstance().addEventListener("ClearSkillPanel", () =>
        {
            GameObject.Destroy(this.gameObject);
        });

        EventCenter.GetInstance().addEventListener<BatCardView>("DragTarget", (obj) =>
        {
            if (listCtrl != null)
                listCtrl();
            ListLimitCount();
            if (MathBase.GetInstance().IsOverLap(this.transform, obj.transform) && listLimitCount!=0
            && obj.bk.GetComponent<Outline>().effectColor != Color.red)
            {
                if (FreeBatPanelCtrl.Instance.IsFreeBat(obj))
                    batFreeCards.Add(obj);
                else if (BatPanelCtrl.Instance.IsBat(obj))
                    batCards.Add(obj);
                obj.transform.SetParent(cardPoint.transform);
            }
            
            
        });

    }
    private void ListLimitCount()
    {
        if (listLimitCount > 0)
        {
            if (batCards.Count > listLimitCount - 1)
            {
                BaseCard.GetInstance().TurnOverCard(batCards[0], true, BatPanelCtrl.Instance.transform);
                batCards.Clear();
            }
            else if (batFreeCards.Count > listLimitCount - 1)
            {
                BaseCard.GetInstance().TurnOverCard(batFreeCards[0], true, FreeBatPanelCtrl.Instance.transform);
                batFreeCards.Clear();
            }
        }
    }
    public void Init(string skillName, string leftText_btn, string rightText_btn, UnityAction leftAction, UnityAction rightAction)
    {
        myName.text = skillName;
        left_btn.GetComponentInChildren<TextMeshProUGUI>().text = leftText_btn;
        right_btn.GetComponentInChildren<TextMeshProUGUI>().text = rightText_btn;
        leftClickAction = leftAction;
        rightClickAction = rightAction;
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Left_btn":
                leftClickAction();
                break;
            case "Right_btn":
                rightClickAction();
                break;
        }
    }
    private void OnDestroy()
    {
        SkillMgr.GetInstance().isSkill=false;
        listLimitCount = 0;
        myName = null;
        left_btn= null;
        right_btn= null;
        listCtrl=null;
        leftClickAction =null;
        rightClickAction=null;
        cardPoint = null;
        if (batFreeCards.Count > 0 && 
            (GameCtrl.nowState==Game_State.GetBatCard|| GameCtrl.nowState == Game_State.GetFreeBatCard))
            BaseCard.GetInstance().SetCard("BattleCard", FreeBatPanelCtrl.Instance.transform, batFreeCards);
        if (batCards.Count > 0 &&
            (GameCtrl.nowState == Game_State.GetBatCard || GameCtrl.nowState == Game_State.GetFreeBatCard))
            BaseCard.GetInstance().SetCard("BattleCard", BatPanelCtrl.Instance.transform, batCards);
        batFreeCards.Clear();
        batCards.Clear();
        EventCenter.GetInstance().EventTrigger("EndSkill");
        EventCenter.GetInstance().ClearSingleEvent("ClearSkillPanel");
        EventCenter.GetInstance().ClearSingleEvent<BatCardView>("DragTarget");

    }
}
