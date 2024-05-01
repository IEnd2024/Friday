using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�ģ��
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// ͬ������
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="unityAction"></param>
    public void LoadScene(string sceneName,UnityAction unityAction)
    {
        SceneManager.LoadScene(sceneName);
        unityAction();
    }
    /// <summary>
    /// �ṩ���ⲿ���첽���ؽӿڷ���
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="unityAction"></param>
    public void LoadSceneAsyn(string sceneName,UnityAction unityAction)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(sceneName,unityAction));
    }
    /// <summary>
    /// Э���첽����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="unityAction"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction unityAction)
    {
        AsyncOperation ao=SceneManager.LoadSceneAsync(name);
        //��ȡ�������ؽ���
        while(!ao.isDone)
        {
            //�¼����ķַ����� �ⲿ���Ի�ȡ����
            EventCenter.GetInstance().EventTrigger("����������",ao.progress);
            yield return ao.progress;
        }
        unityAction();
    }
}
