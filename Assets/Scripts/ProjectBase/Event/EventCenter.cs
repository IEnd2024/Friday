using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
/// <summary>
/// 为了使用泛型而解决装箱拆箱问题的接口和类
/// </summary>
public interface IEventInfo { }
public class EventInfo<T>:IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
/// <summary>
/// 事件中心模块
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();


    /// <summary>
    /// 添加事件监听方法
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">处理事件的委托函数</param>
    public void addEventListener<T>(string eventName, UnityAction<T> action)
    {
        //已有事件监听
        if(eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions += action;
        }
        //未有事件监听
        else
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
    }
    //添加无需参数的事件监听
    public void addEventListener(string eventName, UnityAction action)
    {
        //已有事件监听
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions += action;
        }
        //未有事件监听
        else
        {
            eventDic.Add(eventName, new EventInfo(action));
        }
    }
    /// <summary>
    /// 移除事件监听方法
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">处理事件的委托函数</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }
    //移除无参的事件监听
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= action;
        }
    }

    /// <summary>
    /// 事件触发方法
    /// </summary>
    /// <param name="eventName">触发哪个事件</param>
    public void EventTrigger<T>(string eventName,T info) 
    {
        //已有事件监听
        if (eventDic.ContainsKey(eventName))
        {
            if((eventDic[eventName] as EventInfo<T>).actions != null)
            {
                (eventDic[eventName] as EventInfo<T>).actions.Invoke(info);
            }  
        }
    }
    //无参的事件触发
    public void EventTrigger(string eventName)
    {
        //已有事件监听
        if (eventDic.ContainsKey(eventName))
        {
            if ((eventDic[eventName] as EventInfo).actions != null)
            {
                (eventDic[eventName] as EventInfo).actions.Invoke();
            }
        }
    }
    /// <summary>
    /// 清空事件
    /// 多用于切换场景
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
    /// <summary>
    /// 清空特定事件
    /// </summary>
    /// <param name="eventName"></param>
    public void ClearSingleEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions = null;
        }
    }
    //清空有参特定事件
    public void ClearSingleEvent<T>(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions = null;
        }
    }
}
