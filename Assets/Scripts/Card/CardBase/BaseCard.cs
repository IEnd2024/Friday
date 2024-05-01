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
/// 卡牌基类 实现了卡牌所需的方法 用一个公共的字典存储所有卡牌
/// </summary>
public class BaseCard : BaseManager<BaseCard>
{
    public Dictionary<string,ICardInfo> cardDic=new Dictionary<string,ICardInfo>();



    /// <summary>
    /// 在基类里添加卡牌
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
    /// 在基类里移除卡牌
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
    /// 从基类里获得特定类型的卡牌
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
    /// 清空基类卡牌字典
    /// </summary>
    public void ClearDic()
    {
        cardDic.Clear();
    }
    /// <summary>
    /// 为卡牌添加或取消有参事件监听 true为添加，false为移除
    /// </summary>
    /// <param name="AddOrRemove">true为添加，false为移除</param>
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
    /// 为卡牌添加或取消无参事件监听 true为添加，false为移除
    /// </summary>
    /// <param name="AddOrRemove">true为添加，false为移除</param>
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
    /// 翻转卡片并移动位置
    /// </summary>
    /// <typeparam name="T">卡片类型</typeparam>
    /// <param name="obj">卡牌</param>
    /// <param name="isEnable">卡牌当前状态</param>
    /// <param name="transform">目标位置</param>
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
    /// 鼠标进入退出放大缩小UI
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
    /// 生成卡牌
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
                        //在基类中存储卡牌
                        AddCard(name, obj);
                        //回调函数
                        callBack(obj as T);
                    });
                    break;
                case "BattleCard":
                    UIManager.GetInstance().ShowPanel<BatCardView>("Card/BatCardPanel", E_UI_Layer.Mid, (obj) =>
                    {
                        if (cardDic.ContainsKey(name))
                            k = (cardDic[name] as CardInfo<BatCardView>).cardList.Count;
                        obj.GetComponent<BatCardModel>().Init(cards[j], k);
                        //在基类中存储卡牌
                        AddCard(name, obj);
                        //回调函数
                        callBack(obj as T);
                    });
                    break;
                case "OldCard":
                    UIManager.GetInstance().ShowPanel<BatCardView>("Card/BatCardPanel", E_UI_Layer.Mid, (obj) =>
                    {
                        if (cardDic.ContainsKey(name))
                            k = (cardDic[name] as CardInfo<BatCardView>).cardList.Count;
                        obj.GetComponent<BatCardModel>().Init(cards[j], -k-1);
                        //在基类中存储卡牌
                        AddCard(name, obj);
                        //回调函数
                        callBack(obj as T);
                    });
                    break;

            }
            
        }
    }

    /// <summary>
    /// 洗牌并设置位置
    /// </summary>
    /// <param name="name">卡牌类型名</param>
    /// <param name="Target">目标牌堆位置</param>
    /// <param name="cardList">弃牌堆表</param>
    public List<T> SetCard<T>(string name, Transform Target, List<T> cardList) where T : BasePanel
    {
        if(cardList.Count == 0)
            cardList = GetCard<T>(name);
        //##乱序算法
        foreach (T card in cardList)
        {
            card.transform.SetParent(Target.transform);
            card.transform.localPosition = Vector3.zero;
        }
        return cardList.ToList();
    }
}
