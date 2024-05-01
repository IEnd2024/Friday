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
    /// 初始化数据
    /// </summary>
    public void Init(CardData defaultData,int id)
    {
        //初始化数据
        this.myId = id;;
        newData = new CardData(defaultData);
        temporaryData = new CardData(defaultData);
        //创建新数据json文件
        Save();
        //添加事件监听
        EventListener();
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, "AdvNewData", JsonType.LitJson);
    }
    //是否监听事件
    private void EventListener()
    {
        //通过监听触发冒险卡初始化事件
        EventCenter.GetInstance().addEventListener<UnityAction<CardData>>( myId + "advInfoInit", (action) =>
        {
            action(NewData);
            temporaryData = NewData;
        });
        //添加数据监听和触发事件
        EventCenter.GetInstance().addEventListener<bool>( myId + "advActiveUpdata", (o) =>
        {
            isEnable = o;
            EventCenter.GetInstance().EventTrigger(myId+ "advActiveUpdataOfPanel",isEnable);
        });
        EventCenter.GetInstance().addEventListener<int>( myId + "advValue1", (value) =>
        {
            temporaryData.advValue1 = NewData.advValue1 + value;
            if (temporaryData.advValue1 < 0)
                temporaryData.advValue1 = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().addEventListener<int>(myId + "advValue2", (value) =>
        {
            temporaryData.advValue2 = NewData.advValue2 + value;
            if (temporaryData.advValue2 < 0)
                temporaryData.advValue2 = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().addEventListener<int>(myId + "advValue3", (value) =>
        {   
            temporaryData.advValue3 = NewData.advValue3 + value;
            if (temporaryData.advValue3 < 0)
                temporaryData.advValue3 = 0;
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
    //销毁时移除事件
    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<UnityAction<CardData>>(myId + "advInfoInit", (action) =>
        {
            action(NewData);
            temporaryData = NewData;
        });
        EventCenter.GetInstance().RemoveEventListener<bool>(myId + "advActiveUpdata", (o) =>
        {
            isEnable = o;
            EventCenter.GetInstance().EventTrigger(myId + "advActiveUpdataOfPanel", isEnable);
        });
        EventCenter.GetInstance().RemoveEventListener<int>(myId + "advValue1", (value) =>
        {
            temporaryData.advValue1 = NewData.advValue1 + value;
            if (temporaryData.advValue1 < 0)
                temporaryData.advValue1 = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().RemoveEventListener<int>(myId + "advValue2", (value) =>
        {
            temporaryData.advValue2 = NewData.advValue2 + value;
            if (temporaryData.advValue2 < 0)
                temporaryData.advValue2 = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().RemoveEventListener<int>(myId + "advValue3", (value) =>
        {
            temporaryData.advValue3 = NewData.advValue3 + value;
            if (temporaryData.advValue3 < 0)
                temporaryData.advValue3 = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
        EventCenter.GetInstance().RemoveEventListener<int>(myId + "freeCardValue", (value) =>
        {
            temporaryData.freeCardValue = NewData.freeCardValue + value;
            if (temporaryData.freeCardValue < 0)
                temporaryData.freeCardValue = 0;
            EventCenter.GetInstance().EventTrigger(myId + "advInfoUpdata", temporaryData);
        });
    }

}
