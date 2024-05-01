using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 资源加载模块
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载资源
    public T Load<T>(string name) where T : Object
    {
        T res=Resources.Load<T>(name);
        //是GameObject对象，实例化并返回
        if(res is GameObject)
            return GameObject.Instantiate(res);
        else//不是GameObject对象，直接返回，如TextAsset AudioClip
            return res;
    }
    //异步加载资源
    public void LoadAsync<T>(string name,UnityAction<T> action) where T : Object
    {
        //开启异步的协程
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name,action));
    }
    //真正的协程函数，用于开启对应资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> action) where T: Object
    {
        ResourceRequest r=Resources.LoadAsync<T>(name);
            yield return r;

        if(r.asset is GameObject)
            action(GameObject.Instantiate(r.asset) as T);
        else
            action(r.asset as T);
    }
}
