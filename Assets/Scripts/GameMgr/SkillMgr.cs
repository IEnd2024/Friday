using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkillMgr :BaseManager<SkillMgr>
{
    private string skillName;
    private BatCardView cardSkill;
    private bool isSkill=false;
    private bool islookThreeCard=false;
    
    public string SkillName { get => skillName;  }
    public bool IsSkill { get => isSkill; set => isSkill = value; }
    public bool IslookThreeCard { get => islookThreeCard; set => islookThreeCard = value; }

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
            case "+2����":
                panel.Init(skillName, "�ٳ�һ��", "ȡ��",
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
            case "1*�ݻ�":
                panel.Init("ѡ��һ���ƴݻ�", "�ݻٴ���", "ȡ��",
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
                        cardSkill.bk.color = Color.gray;
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "1*�ӱ�":
                panel.Init("ѡ��һ���ƣ�����ս��������", "�ӱ�����", "ȡ��",
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
                        cardSkill.bk.color = Color.gray;
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "1*����":
                panel.Init("ѡ��һ���ƣ������似��", "���ƴ���", "ȡ��",
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
                        cardSkill.bk.color = Color.gray;
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "�׶�-1":
                break;
            case "��3����":
                islookThreeCard=true;
                EventCenter.GetInstance().EventTrigger<UnityAction<List<BatCardView>>>("GetFirstBat", (objList) =>
                {
                    for(int i = 1; i <= 3; i++)
                    {
                        objList[objList.Count - i].ActiveUpdata(true);
                        BaseCard.GetInstance().TurnOverCard(objList[objList.Count - i], 
                            objList[objList.Count - i].isEnable, panel.cardPoint.transform);
                    }
                });
                panel.Init("�����ƶѶ���������,������˳��Żس����ƿ�," +
                    "ͬʱ����ѡ��ݻ�һ��", "�Ż��ƿ�", "ȡ��",
                   () =>
                   {
                       panel.getCards=new List<BatCardView>
                       ( panel.cardPoint.GetComponentsInChildren<BatCardView>());
                       panel.getCards.Reverse();
                       GameObject.Destroy(panel.gameObject);
                   },
                   () =>
                   {
                       cardSkill.bk.color = Color.gray;
                       panel.getCards = new List<BatCardView>
                       (panel.cardPoint.GetComponentsInChildren<BatCardView>());
                       panel.getCards.Reverse();
                       GameObject.Destroy(panel.gameObject);
                   });
                panel.listLimitCount = 0;
                break;
            case "1*����":
                break;
            case "2*����":
                break;
            case "1*�ƿ��":
                break;
        }
        panel.transform.localPosition = Vector3.up * Screen.height * 1/4;
    }
}
