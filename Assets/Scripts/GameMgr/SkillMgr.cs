using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkillMgr :BaseManager<SkillMgr>
{
    private string skillName;
    public bool isSkill=false;
    public void Init( string skill)
    {
        skillName = skill;
        isSkill = true;
        UIManager.GetInstance().ShowPanel<SkillPanelCtrl>("Panel/SkillPanel", E_UI_Layer.Bot, SkillLogic);
    }

    private void SkillLogic(SkillPanelCtrl panel)
    {
        switch (skillName)
        {
            case "+2张牌":
                panel.Init(skillName, "再抽一张", "取消",
                    () =>
                    {
                        EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                        EventCenter.GetInstance().EventTrigger("HP", 1);
                        GameObject.Destroy(panel.gameObject);
                    },
                    () =>
                    {
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listCtrl = () => { panel.isLimitList = true; };
                break;
            case "1*摧毁":
                panel.Init(skillName, "摧毁一张", "取消",
                    () =>
                    {
                        
                        GameObject.Destroy(panel.gameObject);
                    },
                    () =>
                    {
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listCtrl = () =>
                {
                    if (panel.batCards.Count > 0 )
                    {
                        BaseCard.GetInstance().TurnOverCard(panel.batCards[0], true, BatPanelCtrl.Instance.transform);
                        panel.batCards.Clear();
                    }else if(panel.batFreeCards.Count > 0)
                    {
                        BaseCard.GetInstance().TurnOverCard(panel.batFreeCards[0], true, FreeBatPanelCtrl.Instance.transform);
                        panel.batFreeCards.Clear();
                    }
                };
                break;
        }
        panel.transform.localPosition = Vector3.up * Screen.height * 1/4;
        
    }
}
