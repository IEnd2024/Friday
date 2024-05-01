using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BatCardModel : MonoBehaviour
{
    private int myId;
    private bool isEnable = false;
    private CardData newData;

    public int MyId { get => myId; }
    public CardData NewData { get => newData; }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init(CardData defaultData, int id)
    {
        //初始化数据
        this.myId = id;
        newData = new CardData(defaultData); 
        //创建新数据json文件
        Save();
        EventListener();
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, myId+"BatNewData", JsonType.LitJson);
    }
    private void EventListener()
    {
        //通过监听触发战斗卡初始化事件
        EventCenter.GetInstance().addEventListener<UnityAction<CardData>>( myId + "BatInfoInit", (action) =>
        {
            action(NewData);
        });
        EventCenter.GetInstance().addEventListener<bool>( myId + "BatActiveUpdata", (o) =>
        {
            isEnable = o;
        });
    }
}
