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
        //连接父子对象
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        //存入list
        poolList = new List<GameObject>() { };
        SaveObj(obj);
    }
    public void SaveObj(GameObject obj)
    {
        //失活对象
        obj.SetActive(false);
        //存入list
        poolList.Add(obj);
        //连接子孙对象
        obj.transform.parent = fatherObj.transform; 
    }
    public GameObject GetObj()
    {
        GameObject obj = null;
        //取出第一个
        obj = poolList[0];
        poolList.RemoveAt(0);
        //激活对象
        obj.SetActive(true);
        //断开子孙关系
        obj.transform.parent = null;
        return obj;
    }
}
/// <summary>
/// 缓存池模块
/// </summary>
public class PoolMgr : BaseManager<PoolMgr>
{
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject poolObj;
    /// <summary>
    /// 从池子中获取对象
    /// </summary>
    public void GetObj(string name,UnityAction<GameObject> action)
    {
        //池子里有
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            //把对象从池子中取出，作为参数引入委托，外部可以将函数方法传入委托调用对象（回调函数技术）
            action(poolDic[name].GetObj());
        }
        //池子中无
        else
        {
            //异步加载资源 创建对象 并通过委托和lambda表达式传入改名的匿名函数
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) => 
            { 
                o.name = name; 
                action(o);
            });
        }
    }
    /// <summary>
    /// 对象存入池子
    /// </summary>
    public void SaveObj(string name, GameObject obj)
    {
        //设置创建对象的父对象
        if (poolObj == null)
            poolObj = new GameObject("Pool");
        //若已存入池子
        if (poolDic.ContainsKey(name))
            poolDic[name].SaveObj(obj);
        //若未存入池子
        else
            poolDic.Add(name, new PoolData(obj,poolObj));
    }
    public void Clear()
    {
        poolDic.Clear();
        poolObj=null;
    }

}
