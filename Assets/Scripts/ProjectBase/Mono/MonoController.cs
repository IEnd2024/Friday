using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����Monoģ��
/// ��������ķ�������̳�MonoҲ��ʹ��Mono�еĺ���
/// ͬһ����Update����
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    /// <summary>
    /// �޷����Ƴ�
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (updateEvent != null) {  updateEvent.Invoke(); }
    }
    /// <summary>
    /// ���ⲿ���֡���º�������
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }
    /// <summary>
    /// ���ⲿ�Ƴ�֡���º�������
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action) 
    { 
        updateEvent -= action;
    }
    /// <summary>
    /// �õ��Ѿ���ӵķ���
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public UnityAction GetUpdateLister(UnityAction action)
    {
        Delegate[] list=updateEvent.GetInvocationList();
        foreach (Delegate d in list)
        {
            if (d as UnityAction == action)
                return d as UnityAction;
        }
        return null;
    }
    /// <summary>
    /// ���ⲿ���֡���º�������
    /// </summary>
    /// <param name="action"></param>
    public void ClearUpdateListener()
    {
        updateEvent=null;
    }
}
