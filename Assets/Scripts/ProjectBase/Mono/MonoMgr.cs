using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 管理公共Mono模块使其继承单例模式，保证对象唯一性
/// 提供协程方法
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
    /// 封装添加帧更新函数方法
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        controller.AddUpdateListener(action);
    }
    /// <summary>
    /// 封装移除帧更新函数方法
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        controller.RemoveUpdateListener(action);
        
    }
    /// <summary>
    /// 封装 得到已经添加的方法
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public UnityAction GetUpdateLister(UnityAction action)
    {
        return controller.GetUpdateLister(action);
    }
    /// <summary>
    /// 封装清空帧更新函数方法
    /// </summary>
    public void ClearUpdateListener()
    {
        controller.ClearUpdateListener();
    }
    /// <summary>
    /// 开启协程
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
