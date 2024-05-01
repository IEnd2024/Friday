using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ������Monoģ��ʹ��̳е���ģʽ����֤����Ψһ��
/// �ṩЭ�̷���
/// </summary>
public class MonoMgr : BaseManager<MonoMgr>
{
    private MonoController controller;
    public MonoMgr()
    {
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }
    /// <summary>
    /// ��װ���֡���º�������
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        controller.AddUpdateListener(action);
    }
    /// <summary>
    /// ��װ�Ƴ�֡���º�������
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        controller.RemoveUpdateListener(action);
        
    }
    /// <summary>
    /// ��װ �õ��Ѿ���ӵķ���
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public UnityAction GetUpdateLister(UnityAction action)
    {
        return controller.GetUpdateLister(action);
    }
    /// <summary>
    /// ��װ���֡���º�������
    /// </summary>
    public void ClearUpdateListener()
    {
        controller.ClearUpdateListener();
    }
    /// <summary>
    /// ����Э��
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        
        return controller.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }
}
