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
    //�������״̬����
    public void ActiveUpdata(bool o)
    {
        EventCenter.GetInstance().EventTrigger(myId + "BatActiveUpdata", o);
    }
    //���¿ؼ�����
    public void UpdateInfo(CardData date)
    {
        this.gameObject.name = date.batName;
        nameText.text = date.batName;
        skillName.text = date.skillName;
        hp.text = date.hp.ToString();
        combatValue.text = date.combatValue.ToString();
    }
    //ά�ֿ��Ƶ��Զ�����
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
        //��ʼ������״̬������
        EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "BatInfoInit", UpdateInfo);
        EventCenter.GetInstance().addEventListener<bool>(myId + "BatActiveUpdata", (o) =>
        {
            isEnable = o;
            switchObj.SetActive(o);
        });
        EventCenter.GetInstance().addEventListener<CardData>(myId + "BatInfoUpdata", UpdateInfo);
        ActiveUpdata(isEnable);
        //���UI�¼�
        //�������˳��Ŵ���С
        BaseCard.GetInstance().EnterAndExitCard(myself);
        //��קЧ��
        UIManager.AddCustomEventListener(myself, EventTriggerType.Drag, (obj) =>
        {
            if(isEnable)
            {
                this.transform.position = Input.mousePosition;
            }
        });
        //�����ͷ��߼� �غϽ����ݻٿ��ƽ׶�
        int hpValue = 0;
        EventCenter.GetInstance().addEventListener<int>("DestoryHP", (value) =>
        {
            hpValue = value;
        });
        UIManager.AddCustomEventListener(myself, EventTriggerType.EndDrag, (obj) =>
        {
            if(this.transform.position.y > Screen.height * 2 / 3)
            {
                //�ͷż���
                UseSkill();
                //�ݻٿ��� 
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
        //��������
        EventCenter.GetInstance().addEventListener("EndSkill", () =>
        {
            bk.GetComponent<Outline>().effectColor = Color.black;
        });
        //�غϽ���ð��ʧ��
        EventCenter.GetInstance().addEventListener("EndFailRound", () =>
        {
            if (isEnable)
            {
                EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "BatInfoInit", UpdateInfo);
                EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", this);
            }
            //���ü���
            bk.color = Color.gray;
        });
        //�غϽ���ð�ճɹ�
        EventCenter.GetInstance().addEventListener("EndWinRound", () =>
        {
            if (isEnable)
            {
                EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "BatInfoInit", UpdateInfo);
                EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat", this);
                //���ü���
                bk.color = Color.gray;
            }
        });
    }
    /// <summary>
    /// �����߼�
    /// </summary>
    private void UseSkill()
    {
        if (bk.color != Color.white && GameCtrl.nowState != Game_State.DestroyBatCard
            &&!SkillMgr.GetInstance().isSkill)
        {
            bk.color = Color.white;
            switch (skillName.text)
            {
                case "+1����ֵ":
                    EventCenter.GetInstance().EventTrigger("HP", 1);
                    break;
                case "+2����ֵ":
                    EventCenter.GetInstance().EventTrigger("HP", 2);
                    break;
                case "+1����":
                    EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                    EventCenter.GetInstance().EventTrigger("HP", 1);
                    break;
                case "+2����":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                    EventCenter.GetInstance().EventTrigger("HP", 1);
                    SkillMgr.GetInstance().Init(skillName.text);
                    break;
                case "1*�ݻ�":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    SkillMgr.GetInstance().Init(skillName.text);
                    break;
                case "1*�ӱ�":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    SkillMgr.GetInstance().Init(skillName.text);
                    break;
                case "1*����":
                    bk.GetComponent<Outline>().effectColor = Color.red;
                    SkillMgr.GetInstance().Init(skillName.text);
                    EventCenter.GetInstance().EventTrigger("Cardskilling", this);
                    break;
                case "�׶�-1":
                    if (GameCtrl.TotalState != Game_State.State_Green && GameCtrl.TotalState != Game_State.State_Pirate)
                    {
                        EventCenter.GetInstance().EventTrigger("TotalStageChange", --GameCtrl.TotalState);
                        ++GameCtrl.TotalState;
                    } 
                    break;
                case "��3����":
                    break;
                case "1*����":
                    break;
                case "2*����":
                    break;
                case "1*�ƿ��":
                    break;
                default:
                    bk.color = Color.gray;
                    break;
            }
            print(skillName.text);
        }
    }
    
}
