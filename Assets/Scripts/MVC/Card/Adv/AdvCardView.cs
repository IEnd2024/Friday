using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvCardView : BasePanel
{
    public TextMeshProUGUI advNameText;
    public TextMeshProUGUI batNameText;
    public TextMeshProUGUI freeCardValue;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI advValue1;
    public TextMeshProUGUI advValue2;
    public TextMeshProUGUI advValue3;
    public Image myself;
    public TextMeshProUGUI combatValue;
    public Image stage;
    public GameObject switchObj;
    

    private int myId;
    private bool isEnable = false;

    public int MyId { get => myId;  }
    public bool IsEnable { get => isEnable; }

    private void Start()
    {
        myId = this.GetComponent<AdvCardModel>().MyId;
        EventListener();
    }
    /// <summary>
    /// �ؼ����ݸ���
    /// </summary>
    /// <param name="date"></param>
    private void UpdateInfo(CardData date)
    {
        this.gameObject.name = date.advName;
        advNameText.text = date.advName;
        batNameText.text = date.batName;
        freeCardValue.text = date.freeCardValue.ToString();
        skillName.text = date.skillName;
        hp.text = date.hp.ToString();
        advValue1.text = date.advValue1.ToString();
        advValue2.text = date.advValue2.ToString();
        advValue3.text = date.advValue3.ToString();
        combatValue.text = date.combatValue.ToString();
    }
    /// <summary>
    /// �������״̬����
    /// </summary>
    /// <param name="o"></param>
    public void ActiveUpdata(bool o)
    {
        EventCenter.GetInstance().EventTrigger(myId + "advActiveUpdata", o);
    }
    //�ı�stageͼ���λ��
    private void CheckStage()
    {
        switch (GameCtrl.TotalState)
        {
            case Game_State.State_Green:
                stage.transform.localPosition = advValue1.transform.localPosition;
                break;
            case Game_State.State_Yellow:
                stage.transform.localPosition = advValue2.transform.localPosition;
                break;
            case Game_State.State_Red:
                stage.transform.localPosition = advValue3.transform.localPosition;
                break;
        }
    }
    /// <summary>
    /// ������ݱ仯�¼�
    /// </summary>
    private void TotalStageChange()
    {
        CheckStage();
        switch (GameCtrl.TotalState)
        {
            case Game_State.State_Green:
                //�鿨��仯ð������ս��ֵ
                EventCenter.GetInstance().addEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().addEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                //����ð�ջغϽ���
                EventCenter.GetInstance().addEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) => {
                    if (isEnable)
                        action(int.Parse(advValue1.text));
                });
                break;
            case Game_State.State_Yellow:
                EventCenter.GetInstance().RemoveEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) => {
                    if (isEnable)
                        action(int.Parse(advValue1.text));
                });
                EventCenter.GetInstance().addEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue2", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().addEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue2", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().addEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) => {
                    if (isEnable)
                        action(int.Parse(advValue2.text));
                });
                break;
            case Game_State.State_Red:
                EventCenter.GetInstance().RemoveEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) => {
                    if (isEnable)
                        action(int.Parse(advValue2.text));
                });
                EventCenter.GetInstance().addEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue3", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().addEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue3", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().addEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) => {
                    if (isEnable)
                        action(int.Parse(advValue3.text));
                });
                break;
            case Game_State.State_Pirate:
                print("������Ϯ");
                break;
        }
    }
    private void EventListener()
    {
        //��ʼ������״̬������
        EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "advInfoInit", UpdateInfo);
        EventCenter.GetInstance().addEventListener<bool>(myId + "advActiveUpdataOfPanel", (o) =>
        {
            isEnable = o;
            switchObj.SetActive(o);
        });
        EventCenter.GetInstance().addEventListener<CardData>(myId + "advInfoUpdata", UpdateInfo);
        ActiveUpdata(isEnable);
        //������ݱ仯�¼�
        EventCenter.GetInstance().addEventListener("TotalStageChange", TotalStageChange);
        //�����ս����ʱ�仯��ѳ鿨��
        EventCenter.GetInstance().addEventListener(myId + "FreeBatCardCount", () =>
        {
            if (int.Parse(freeCardValue.text) > 0)
            {
                EventCenter.GetInstance().EventTrigger(myId + "freeCardValue", -1);
            }
            else
            {
                GameCtrl.nowState = Game_State.GetBatCard;
            }
        });
        //�غϽ���ð��ʧ��
        EventCenter.GetInstance().addEventListener(myId + "EndFailRound", () =>
        {
            EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "advInfoInit", UpdateInfo);
            EventCenter.GetInstance().EventTrigger("SaveToDiscardOfAdv", this);
            EventCenter.GetInstance().EventTrigger("AdvShuffle");
        });
        //�غϽ���ð�ճɹ�
        EventCenter.GetInstance().addEventListener(myId + "EndWinRound", () =>
        {
            EventCenter.GetInstance().EventTrigger("AdvShuffle");
            EventCenter.GetInstance().EventTrigger("ChangeAdvToBat", this);
            
        });
        //���UI�¼�
        BaseCard.GetInstance().EnterAndExitCard(myself);
        UIManager.AddCustomEventListener(myself, EventTriggerType.PointerClick, (obj) =>
        {
            if (GameCtrl.nowState == Game_State.GetAdvCard)
            {
                EventCenter.GetInstance().EventTrigger("StartAdv", this);
                EventCenter.GetInstance().EventTrigger("ShowText", "��ʼð��");
                GameCtrl.nowState = Game_State.GetFreeBatCard;
            }
        });
    }
    //����ʱ�Ƴ��¼�
    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<bool>(myId + "advActiveUpdataOfPanel", (o) =>
        {
            isEnable = o;
            switchObj.SetActive(o);
        });
        EventCenter.GetInstance().RemoveEventListener<CardData>(myId + "advInfoUpdata", UpdateInfo);
        EventCenter.GetInstance().RemoveEventListener("TotalStageChange", TotalStageChange);
        EventCenter.GetInstance().RemoveEventListener(myId + "FreeBatCardCount", () =>
        {
            if (int.Parse(freeCardValue.text) > 0)
            {
                EventCenter.GetInstance().EventTrigger(myId + "freeCardValue", -1);
            }
            else
            {
                GameCtrl.nowState = Game_State.GetBatCard;
            }
        });
        EventCenter.GetInstance().RemoveEventListener(myId + "EndFailRound", () =>
        {
            EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(myId + "advInfoInit", UpdateInfo);
            EventCenter.GetInstance().EventTrigger("SaveToDiscardOfAdv", this);
            EventCenter.GetInstance().EventTrigger("AdvShuffle");
        });
        EventCenter.GetInstance().RemoveEventListener(myId + "EndWinRound", () =>
        {
            EventCenter.GetInstance().EventTrigger("ChangeAdvToBat", this);
            EventCenter.GetInstance().EventTrigger("AdvShuffle");
        });
        switch (GameCtrl.TotalState)
        {
            case Game_State.State_Green:
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) =>
                {
                    if (isEnable)
                        action(int.Parse(advValue1.text));
                });
                break;
            case Game_State.State_Yellow:
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue2", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue2", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) =>
                {
                    if (isEnable)
                        action(int.Parse(advValue2.text));
                });
                break;
            case Game_State.State_Red:
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue1", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue2", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue2", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetFreeBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue3", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<BatCardView>(myId + "GetBatCard", (obj) =>
                {
                    if (isEnable)
                        EventCenter.GetInstance().EventTrigger(myId + "advValue3", -int.Parse(obj.combatValue.text));
                });
                EventCenter.GetInstance().RemoveEventListener<UnityAction<int>>("EndRoundOfDeHp", (action) =>
                {
                    if (isEnable)
                        action(int.Parse(advValue3.text));
                });
                break;
        }

    }
}
