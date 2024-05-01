using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum Card_Type
{
    AdventureCard,
    BattleCard,
    OldCard,
    PirateCard,
}
public interface ICardInfo { }
public class CardInfo<T> : ICardInfo
{
    public List<T> cardList=new List<T>();
    public CardInfo(T card)
    {
        cardList.Add(card);
    }
}
/// <summary>
/// ���ƻ��� ʵ���˿�������ķ��� ��һ���������ֵ�洢���п���
/// </summary>
public class BaseCard : BaseManager<BaseCard>
{
    public Dictionary<string,ICardInfo> cardDic=new Dictionary<string,ICardInfo>();



    /// <summary>
    /// �ڻ�������ӿ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="card"></param>
    public void AddCard<T>(string type,T card)
    {
        if(cardDic.ContainsKey(type))
        {
            (cardDic[type] as CardInfo<T>).cardList.Add(card);
        }
        else
        {
            cardDic.Add(type,new CardInfo<T>(card));
        }
    }
    /// <summary>
    /// �ڻ������Ƴ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void RemoveCard<T>(string type, T obj)
    {
        if (cardDic.ContainsKey(type))
        {
            (cardDic[type] as CardInfo<T>).cardList.Remove(obj);
        }
    }
    /// <summary>
    /// �ӻ��������ض����͵Ŀ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<T> GetCard<T>(string type)
    {
        if (cardDic.ContainsKey(type))
        {
            return (cardDic[type] as CardInfo<T>).cardList.ToList();
        }
        return null;
    }
    /// <summary>
    /// ��ջ��࿨���ֵ�
    /// </summary>
    public void ClearDic()
    {
        cardDic.Clear();
    }
    /// <summary>
    /// Ϊ������ӻ�ȡ���в��¼����� trueΪ��ӣ�falseΪ�Ƴ�
    /// </summary>
    /// <param name="AddOrRemove">trueΪ��ӣ�falseΪ�Ƴ�</param>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void EventListener<T>(bool AddOrRemove,string name,UnityAction<T> action)
    {
        if (AddOrRemove)
        {
            EventCenter.GetInstance().addEventListener(name, action);
        }
        else if (!AddOrRemove)
        {
            EventCenter.GetInstance().RemoveEventListener(name, action);
        }
    }
    /// <summary>
    /// Ϊ������ӻ�ȡ���޲��¼����� trueΪ��ӣ�falseΪ�Ƴ�
    /// </summary>
    /// <param name="AddOrRemove">trueΪ��ӣ�falseΪ�Ƴ�</param>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void EventListener(bool AddOrRemove, string name, UnityAction action)
    {
        if (AddOrRemove)
        {
            EventCenter.GetInstance().addEventListener(name, action);
        }
        else if (!AddOrRemove)
        {
            EventCenter.GetInstance().RemoveEventListener(name, action);
        }
    }
    /// <summary>
    /// ��ת��Ƭ���ƶ�λ��
    /// </summary>
    /// <typeparam name="T">��Ƭ����</typeparam>
    /// <param name="obj">����</param>
    /// <param name="isEnable">���Ƶ�ǰ״̬</param>
    /// <param name="transform">Ŀ��λ��</param>
    public void TurnOverCard<T>(T obj,bool isEnable ,Transform Target)where T : BasePanel
    {
        obj.transform.SetParent(Target);
        (obj.transform as RectTransform).anchorMin = Vector2.one * 0.5f;
        (obj.transform as RectTransform).anchorMax = Vector2.one * 0.5f;

        obj.transform.localPosition = Vector3.zero;
        if (isEnable)
        {
            obj.transform.localRotation = Quaternion.identity;
        }
        else
        {
            obj.transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
    }
    /// <summary>
    /// �������˳��Ŵ���СUI
    /// </summary>
    /// <param name="control"></param>
    public void EnterAndExitCard(UIBehaviour control) 
    {
        UIManager.AddCustomEventListener(control, EventTriggerType.PointerEnter, (obj) =>
        {
            control.gameObject.transform.localScale = Vector3.one * 1.2f;
        });
        UIManager.AddCustomEventListener(control, EventTriggerType.PointerExit, (obj) =>
        {
            control.transform.localScale = Vector3.one;
        });
    }

    /// <summary>
    /// ���ɿ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="cards"></param>
    /// <param name="callBack"></param>
    public void CreateCard<T>(string name, List<CardData> cards,Action<T> callBack) where T : BasePanel
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int j = i;
            int k = 0;
            switch (name)
            {
                case "AdventureCard":
                    UIManager.GetInstance().ShowPanel<AdvCardView>("Card/AdvCardPanel", E_UI_Layer.Mid, (obj) =>
                    {
                        if (cardDic.ContainsKey(name))
                            k = (cardDic[name] as CardInfo<AdvCardView>).cardList.Count;
                        obj.GetComponent<AdvCardModel>().Init(cards[j],k );
                        //�ڻ����д洢����
                        AddCard(name, obj);
                        //�ص�����
                        callBack(obj as T);
                    });
                    break;
                case "BattleCard":
                    UIManager.GetInstance().ShowPanel<BatCardView>("Card/BatCardPanel", E_UI_Layer.Mid, (obj) =>
                    {
                        if (cardDic.ContainsKey(name))
                            k = (cardDic[name] as CardInfo<BatCardView>).cardList.Count;
                        obj.GetComponent<BatCardModel>().Init(cards[j], k);
                        //�ڻ����д洢����
                        AddCard(name, obj);
                        //�ص�����
                        callBack(obj as T);
                    });
                    break;
                case "OldCard":
                    UIManager.GetInstance().ShowPanel<BatCardView>("Card/BatCardPanel", E_UI_Layer.Mid, (obj) =>
                    {
                        if (cardDic.ContainsKey(name))
                            k = (cardDic[name] as CardInfo<BatCardView>).cardList.Count;
                        obj.GetComponent<BatCardModel>().Init(cards[j], -k-1);
                        //�ڻ����д洢����
                        AddCard(name, obj);
                        //�ص�����
                        callBack(obj as T);
                    });
                    break;

            }
            
        }
    }

    /// <summary>
    /// ϴ�Ʋ�����λ��
    /// </summary>
    /// <param name="name">����������</param>
    /// <param name="Target">Ŀ���ƶ�λ��</param>
    /// <param name="cardList">���ƶѱ�</param>
    public List<T> SetCard<T>(string name, Transform Target, List<T> cardList) where T : BasePanel
    {
        if(cardList.Count == 0)
            cardList = GetCard<T>(name);
        //##�����㷨
        foreach (T card in cardList)
        {
            card.transform.SetParent(Target.transform);
            card.transform.localPosition = Vector3.zero;
        }
        return cardList.ToList();
    }
}
