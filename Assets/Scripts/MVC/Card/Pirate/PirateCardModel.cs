using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PirateCardModel : MonoBehaviour
{

    private int myId;
    private CardData newData;
    private bool isEnable=false;

    public CardData NewData { get => newData; }
    public int MyId { get => myId;  }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void Init(CardData defaultData, int id)
    {
        //��ʼ������
        this.myId = id;
        newData = new CardData(defaultData);
        EventListener();
    }
    /// <summary>
    /// ��������
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, "PirtaeNewData", JsonType.LitJson);
    }
    private void EventListener()
    {
        //ͨ����������ս������ʼ���¼�
        EventCenter.GetInstance().addEventListener<UnityAction<CardData>>(myId + "PirateInfoInit", (action) =>
        {
            action(NewData);
        });
        EventCenter.GetInstance().addEventListener<bool>(myId + "PirateActiveUpdata", (o) =>
        {
            isEnable = o;
        });
        //����ս���¼�
        EventCenter.GetInstance().addEventListener<int>(myId + "PirateValue1", (value) =>
        {
            newData.advValue1 += value;
            EventCenter.GetInstance().EventTrigger(myId + "PirateInfoUpdata", newData);
        });
        EventCenter.GetInstance().addEventListener<int>(myId + "freeCardValue", (value) =>
        {
            newData.freeCardValue += value;
            if (newData.freeCardValue < 0)
                newData.freeCardValue = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", newData);
        });
    }

}
