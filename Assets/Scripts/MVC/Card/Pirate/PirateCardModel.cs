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
    /// 初始化数据
    /// </summary>
    public void Init(CardData defaultData, int id)
    {
        //初始化数据
        this.myId = id;
        newData = new CardData(defaultData);
        EventListener();
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, "PirtaeNewData", JsonType.LitJson);
    }
    private void EventListener()
    {
        //通过监听触发战斗卡初始化事件
        EventCenter.GetInstance().addEventListener<UnityAction<CardData>>(myId + "PirateInfoInit", (action) =>
        {
            action(NewData);
        });
        EventCenter.GetInstance().addEventListener<bool>(myId + "PirateActiveUpdata", (o) =>
        {
            isEnable = o;
        });
    }

}
