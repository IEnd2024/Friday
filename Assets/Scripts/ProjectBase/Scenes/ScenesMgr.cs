using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// 同步加载
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="unityAction"></param>
    public void LoadScene(string sceneName,UnityAction unityAction)
    {
        SceneManager.LoadScene(sceneName);
        unityAction();
    }
    /// <summary>
    /// 提供给外部的异步加载接口方法
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="unityAction"></param>
    public void LoadSceneAsyn(string sceneName,UnityAction unityAction)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(sceneName,unityAction));
    }
    /// <summary>
    /// 协程异步加载
    /// </summary>
    /// <param name="name"></param>
    /// <param name="unityAction"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction unityAction)
    {
        AsyncOperation ao=SceneManager.LoadSceneAsync(name);
        //获取场景加载进度
        while(!ao.isDone)
        {
            //事件中心分发进度 外部可以获取进度
            EventCenter.GetInstance().EventTrigger("进度条更新",ao.progress);
            yield return ao.progress;
        }
        unityAction();
    }
}
