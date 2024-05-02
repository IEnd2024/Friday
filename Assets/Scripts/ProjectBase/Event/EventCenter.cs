using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
/// <summary>
/// Ϊ��ʹ�÷��Ͷ����װ���������Ľӿں���
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
/// �¼�����ģ��
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();


    /// <summary>
    /// ����¼���������
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="action">�����¼���ί�к���</param>
    public void addEventListener<T>(string eventName, UnityAction<T> action)
    {
        //�����¼�����
        if(eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions += action;
        }
        //δ���¼�����
        else
        {
            eventDic.Add(eventName, new EventInfo<T>(action));
        }
    }
    //�������������¼�����
    public void addEventListener(string eventName, UnityAction action)
    {
        //�����¼�����
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions += action;
        }
        //δ���¼�����
        else
        {
            eventDic.Add(eventName, new EventInfo(action));
        }
    }
    /// <summary>
    /// �Ƴ��¼���������
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="action">�����¼���ί�к���</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions -= action;
        }
    }
    //�Ƴ��޲ε��¼�����
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions -= action;
        }
    }

    /// <summary>
    /// �¼���������
    /// </summary>
    /// <param name="eventName">�����ĸ��¼�</param>
    public void EventTrigger<T>(string eventName,T info) 
    {
        //�����¼�����
        if (eventDic.ContainsKey(eventName))
        {
            if((eventDic[eventName] as EventInfo<T>).actions != null)
            {
                (eventDic[eventName] as EventInfo<T>).actions.Invoke(info);
            }  
        }
    }
    //�޲ε��¼�����
    public void EventTrigger(string eventName)
    {
        //�����¼�����
        if (eventDic.ContainsKey(eventName))
        {
            if ((eventDic[eventName] as EventInfo).actions != null)
            {
                (eventDic[eventName] as EventInfo).actions.Invoke();
            }
        }
    }
    /// <summary>
    /// ����¼�
    /// �������л�����
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
    /// <summary>
    /// ����ض��¼�
    /// </summary>
    /// <param name="eventName"></param>
    public void ClearSingleEvent(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo).actions = null;
        }
    }
    //����в��ض��¼�
    public void ClearSingleEvent<T>(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            (eventDic[eventName] as EventInfo<T>).actions = null;
        }
    }
}
