using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono模块
/// 让其他类的方法无需继承Mono也能使用Mono中的函数
/// 同一管理Update方法
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    /// <summary>
    /// 无法被移除
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
    /// 给外部添加帧更新函数方法
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }
    /// <summary>
    /// 给外部移除帧更新函数方法
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action) 
    { 
        updateEvent -= action;
    }
    /// <summary>
    /// 得到已经添加的方法
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
    /// 给外部清空帧更新函数方法
    /// </summary>
    /// <param name="action"></param>
    public void ClearUpdateListener()
    {
        updateEvent=null;
    }
}
