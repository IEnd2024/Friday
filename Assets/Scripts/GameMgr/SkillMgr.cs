using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
                panel.Init("ѡ��һ���ƴݻ�", "�ݻ�", "ȡ��",
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
                            if (panel.batCards[0].skillName.text == "ֹͣ")
                                if (GameCtrl.nowAdvCard.freeCardValue.text != "0")
                                    GameCtrl.nowState = Game_State.GetFreeBatCard;
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
                            if (panel.batFreeCards[0].skillName.text == "ֹͣ")
                                if (GameCtrl.nowAdvCard.freeCardValue.text != "0")
                                    GameCtrl.nowState = Game_State.GetFreeBatCard;
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
                panel.Init("ѡ��һ���ƣ�����ս��������", "�ӱ�", "ȡ��",
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
                panel.Init("ѡ��һ���ƣ������似��", "����", "ȡ��",
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
                panel.Init("�����ƶѶ���������,������˳��Żس����ƶ�," +
                    "ͬʱ����ѡ��ݻ�һ��(�����Ϊ�ƶѶ�)", "�Ż��ƿ�", "ȡ��",
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
                panel.Init("ѡ��һ���ƣ���һ�������佻��", "����", "ȡ��",
                    () =>
                    {
                        if (panel.batCards.Count > 0)
                        {
                            EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", panel.batCards[0]);
                            EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                            EventCenter.GetInstance().EventTrigger("HP", 1);
                            panel.batCards[0].bk.color = Color.gray;
                            panel.batCards.Clear();
                            GameObject.Destroy(panel.gameObject);
                        }
                        else if (panel.batFreeCards.Count > 0)
                        {
                            EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", panel.batFreeCards[0]);
                            EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetFreeBatCard);
                            if (GameCtrl.nowState == Game_State.GetFreeBatCard)
                                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "FreeBatCardCount", 1);
                            panel.batFreeCards[0].bk.color = Color.gray;
                            panel.batFreeCards.Clear();
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
            case "2*����":
                int count=0;
                panel.Init("ѡ��һ���ƣ���һ�������佻��*2", "����һ��", "ȡ��",
                    () =>
                    {
                        if (panel.batCards.Count > 0)
                        {
                            EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", panel.batCards[0]);
                            EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                            EventCenter.GetInstance().EventTrigger("HP", 1);
                            panel.batCards[0].bk.color = Color.gray;
                            panel.batCards.Clear();
                            count++;
                        }
                        else if (panel.batFreeCards.Count > 0)
                        {
                            EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", panel.batFreeCards[0]);
                            EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetFreeBatCard);
                            if (GameCtrl.nowState == Game_State.GetFreeBatCard)
                                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "FreeBatCardCount", 1);
                            panel.batFreeCards[0].bk.color = Color.gray;
                            panel.batFreeCards.Clear();
                            count++;
                        }
                        if (count == 2)
                            GameObject.Destroy(panel.gameObject);
                    },
                    () =>
                    {
                        if (count == 0)
                            cardSkill.bk.color = Color.gray;
                        GameObject.Destroy(panel.gameObject);
                    });
                panel.listLimitCount = 1;
                break;
            case "1*�ƿ��":
                panel.Init("ѡ��һ���ƣ��Żس鿨�ƶѵ�", "�Ż��ƶ�", "ȡ��",
                    () =>
                    {
                        if (panel.batCards.Count > 0)
                        {
                            EventCenter.GetInstance().EventTrigger("SaveToGetTopOfBat", panel.batCards[0]);
                            panel.batCards[0].bk.color = Color.gray;
                            panel.batCards.Clear();
                            GameObject.Destroy(panel.gameObject);
                        }
                        else if (panel.batFreeCards.Count > 0)
                        {
                            EventCenter.GetInstance().EventTrigger("SaveToGetTopOfBat", panel.batFreeCards[0]);
                            EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetFreeBatCard);
                            if (GameCtrl.nowState == Game_State.GetFreeBatCard)
                                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "FreeBatCardCount", 1);
                            panel.batFreeCards[0].bk.color = Color.gray;
                            panel.batFreeCards.Clear();
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
        }
        panel.transform.localPosition = Vector3.up * Screen.height * 1/4;
    }
    public void OldSkillLogic(BatCardView card)
    {
        switch (card.skillName.text)
        {
            case "-1����ֵ":
                EventCenter.GetInstance().EventTrigger("HP", -1);
                card.bk.GetComponent<Outline>().effectColor = Color.yellow;
                break;
            case "-2����ֵ":
                EventCenter.GetInstance().EventTrigger("HP", -2);
                card.bk.GetComponent<Outline>().effectColor = Color.yellow;
                break;
            case "ս��ֵ��ߵ���=0":
                card.bk.GetComponent<Outline>().effectColor = Color.yellow;
                BatCardView MaxCard = card;
                foreach (BatCardView bat in FreeBatPanelCtrl.Instance.batFreeCards)
                {
                    if (bat.isEnable &&
                        int.Parse(bat.combatValue.text) > int.Parse(MaxCard.combatValue.text))
                        MaxCard = bat;
                }
                foreach (BatCardView bat in BatPanelCtrl.Instance.batCards)
                {
                    if (bat.isEnable &&
                        int.Parse(bat.combatValue.text) > int.Parse(MaxCard.combatValue.text))
                        MaxCard = bat;
                }
                MaxCard.combatValue.text = (-int.Parse(MaxCard.combatValue.text)).ToString();
                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetBatCard", MaxCard);
                MaxCard.combatValue.text = "0";
                MaxCard.bk.GetComponent<Outline>().effectColor = Color.yellow;
                break;
        }
    }
}
