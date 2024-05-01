using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ��Դ����ģ��
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //ͬ��������Դ
    public T Load<T>(string name) where T : Object
    {
        T res=Resources.Load<T>(name);
        //��GameObject����ʵ����������
        if(res is GameObject)
            return GameObject.Instantiate(res);
        else//����GameObject����ֱ�ӷ��أ���TextAsset AudioClip
            return res;
    }
    //�첽������Դ
    public void LoadAsync<T>(string name,UnityAction<T> action) where T : Object
    {
        //�����첽��Э��
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name,action));
    }
    //������Э�̺��������ڿ�����Ӧ��Դ
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
