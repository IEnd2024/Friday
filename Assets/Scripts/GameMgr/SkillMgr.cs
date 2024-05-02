using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkillMgr :BaseManager<SkillMgr>
{
    private string skillName;
    private BatCardView cardSkill;
    public bool isSkill=false;
    public void Init( string skill)
    {
        skillName = skill;
        isSkill = true;
        EventCenter.GetInstance().addEventListener<BatCardView>("Cardskilling", (obj) =>
        {
            cardSkill = obj;
        });
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
                panel.listLimitCount = 0;
                break;
            case "1*摧毁":
                panel.Init(skillName, "摧毁此牌", "取消",
                    () =>
                    {
                        if (panel.batCards.Count > 0 )
                        {
                            panel.batCards[0].ActiveUpdata(false);
                            panel.batCards[0].combatValue.text = 
                            (-int.Parse(panel.batCards[0].combatValue.text)).ToString();
                            EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetBatCard", 
                                panel.batCards[0]);
                            panel.batCards[0].combatValue.text = 
                            (-int.Parse(panel.batCards[0].combatValue.text)).ToString();
                            GameObject.Destroy(panel.gameObject);
                        }
                        else if(panel.batFreeCards.Count > 0)
                        {
                            panel.batFreeCards[0].ActiveUpdata(false);
                            panel.batFreeCards[0].combatValue.text = 
                            (-int.Parse(panel.batFreeCards[0].combatValue.text)).ToString();
                            EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetFreeBatCard", 
                                panel.batFreeCards[0]);
                            panel.batFreeCards[0].combatValue.text = 
                            (-int.Parse(panel.batFreeCards[0].combatValue.text)).ToString();
                            GameObject.Destroy(panel.gameObject);
                        } 
                    },
                    () =>
                    {
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "1*加倍":
                panel.Init(skillName, "加倍此牌", "取消",
                    () =>
                    {
                        if (panel.batCards.Count > 0 &&
                        panel.batCards[0].combatValue.text ==
                            panel.batCards[0].GetComponent<BatCardModel>().NewData.combatValue.ToString()
                            && int.Parse(panel.batCards[0].combatValue.text) != 0)
                        {
                            EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetBatCard",
                            panel.batCards[0]);
                            panel.batCards[0].combatValue.text =
                            (2 * int.Parse(panel.batCards[0].combatValue.text)).ToString();
                            GameObject.Destroy(panel.gameObject);
                        }
                        else if (panel.batFreeCards.Count > 0 &&
                        panel.batFreeCards[0].combatValue.text ==
                            panel.batFreeCards[0].GetComponent<BatCardModel>().NewData.combatValue.ToString()
                            && int.Parse(panel.batFreeCards[0].combatValue.text) != 0)
                        {
                            EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetFreeBatCard",
                                panel.batFreeCards[0]);
                            panel.batFreeCards[0].combatValue.text =
                            (2 * int.Parse(panel.batFreeCards[0].combatValue.text)).ToString();
                            GameObject.Destroy(panel.gameObject);
                        }
                    },
                    () =>
                    {
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "1*复制":
                panel.Init(skillName, "复制此牌", "取消",
                    () =>
                    {
                        if (panel.batCards.Count > 0)
                        {
                            cardSkill.skillName.text = panel.batCards[0].skillName.text;
                            cardSkill.bk.color = Color.gray;
                            GameObject.Destroy(panel.gameObject);
                        }
                        else if (panel.batFreeCards.Count > 0)
                        {
                            cardSkill.skillName.text = panel.batFreeCards[0].skillName.text;
                            cardSkill.bk.color = Color.gray;
                            GameObject.Destroy(panel.gameObject);
                        }
                    },
                    () =>
                    {
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "阶段-1":
                break;
            case "看3张牌":
                break;
            case "1*交换":
                break;
            case "2*交换":
                break;
            case "1*牌库底":
                break;
        }
        panel.transform.localPosition = Vector3.up * Screen.height * 1/4;
    }
}
