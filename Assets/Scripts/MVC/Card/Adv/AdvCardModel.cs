using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdvCardModel : MonoBehaviour
{
    private int myId;
    private CardData newData;
    private CardData temporaryData;

    private bool isEnable=false;
    public int MyId { get => myId; }
    public CardData NewData { get => newData; }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void Init(CardData defaultData,int id)
    {
        //��ʼ������
        this.myId = id;;
        newData = new CardData(defaultData);
        temporaryData = new CardData(defaultData);
        //����������json�ļ�
        Save();
        //����¼�����
        EventListener();
    }
    /// <summary>
    /// ��������
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, "AdvNewData", JsonType.LitJson);
    }
    //�Ƿ�����¼�
    private void EventListener()
    {
        //ͨ����������ð�տ���ʼ���¼�
        EventCenter.GetInstance().addEventListener<UnityAction<CardData>>( myId + "advInfoInit", (action) =>
        {
            action(NewData);
            temporaryData = NewData;
        });
        //������ݼ����ʹ����¼�
        EventCenter.GetInstance().addEventListener<bool>( myId + "advActiveUpdata", (o) =>
        {
            isEnable = o;
            EventCenter.GetInstance().EventTrigger(myId+ "advActiveUpdataOfPanel",isEnable);
        });
        EventCenter.GetInstance().addEventListener<int>( myId + "advValue1", (value) =>
        {
            temporaryData.advValue1 = NewData.advValue1 + value;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().addEventListener<int>(myId + "advValue2", (value) =>
        {
            temporaryData.advValue2 = NewData.advValue2 + value;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().addEventListener<int>(myId + "advValue3", (value) =>
        {   
            temporaryData.advValue3 = NewData.advValue3 + value;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().addEventListener<int>(myId + "freeCardValue", (value) =>
        {
            temporaryData.freeCardValue = NewData.freeCardValue + value;
            if (temporaryData.freeCardValue < 0)
                temporaryData.freeCardValue = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });

    }
    //����ʱ�Ƴ��¼�
    private void OnDestroy()
    {
        EventCenter.GetInstance().ClearSingleEvent<UnityAction<CardData>>(myId + "advInfoInit");
        EventCenter.GetInstance().ClearSingleEvent<bool>(myId + "advActiveUpdata");
        EventCenter.GetInstance().ClearSingleEvent<int>(myId + "advValue1");
        EventCenter.GetInstance().ClearSingleEvent<int>(myId + "advValue2");
        EventCenter.GetInstance().ClearSingleEvent<int>(myId + "advValue3");
        EventCenter.GetInstance().ClearSingleEvent<int>(myId + "freeCardValue");
    }

}
