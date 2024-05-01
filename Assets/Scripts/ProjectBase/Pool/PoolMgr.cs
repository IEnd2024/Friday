using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;
    public PoolData(GameObject obj,GameObject poolObj)
    {
        //���Ӹ��Ӷ���
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        //����list
        poolList = new List<GameObject>() { };
        SaveObj(obj);
    }
    public void SaveObj(GameObject obj)
    {
        //ʧ�����
        obj.SetActive(false);
        //����list
        poolList.Add(obj);
        //�����������
        obj.transform.parent = fatherObj.transform; 
    }
    public GameObject GetObj()
    {
        GameObject obj = null;
        //ȡ����һ��
        obj = poolList[0];
        poolList.RemoveAt(0);
        //�������
        obj.SetActive(true);
        //�Ͽ������ϵ
        obj.transform.parent = null;
        return obj;
    }
}
/// <summary>
/// �����ģ��
/// </summary>
public class PoolMgr : BaseManager<PoolMgr>
{
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject poolObj;
    /// <summary>
    /// �ӳ����л�ȡ����
    /// </summary>
    public void GetObj(string name,UnityAction<GameObject> action)
    {
        //��������
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            //�Ѷ���ӳ�����ȡ������Ϊ��������ί�У��ⲿ���Խ�������������ί�е��ö��󣨻ص�����������
            action(poolDic[name].GetObj());
        }
        //��������
        else
        {
            //�첽������Դ �������� ��ͨ��ί�к�lambda���ʽ�����������������
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) => 
            { 
                o.name = name; 
                action(o);
            });
        }
    }
    /// <summary>
    /// ����������
    /// </summary>
    public void SaveObj(string name, GameObject obj)
    {
        //���ô�������ĸ�����
        if (poolObj == null)
            poolObj = new GameObject("Pool");
        //���Ѵ������
        if (poolDic.ContainsKey(name))
            poolDic[name].SaveObj(obj);
        //��δ�������
        else
            poolDic.Add(name, new PoolData(obj,poolObj));
    }
    public void Clear()
    {
        poolDic.Clear();
        poolObj=null;
    }

}
