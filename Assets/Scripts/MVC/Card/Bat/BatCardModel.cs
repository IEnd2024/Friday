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
        JsonMgr.GetInstance().SaveData(newData, myId+"BatNewData", JsonType.LitJson);
    }
    private void EventListener()
    {
        //ͨ����������ս������ʼ���¼�
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
