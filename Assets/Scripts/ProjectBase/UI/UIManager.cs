using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// UI层级枚举
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}
/// <summary>
/// UI管理器
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    //记录UI的Cavans 方便使用
    public RectTransform canvas;

    public UIManager()
    {
        //创建Canvas，并使其不被切换场景时清除
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        //找到各个UI层
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //创建system，并使其不被切换场景时清除
        obj = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后 你想实现的方法</param>
    public void ShowPanel<T>(string panelName,E_UI_Layer layer=E_UI_Layer.Mid,UnityAction<T> callBack=null)where T : BasePanel
    {
        //面板已经显示
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].gameObject.SetActive(true);
            panelDic[panelName].ShowMe();
            if (callBack != null)
                callBack(panelDic[panelName] as T);
            //避免面板因为异步原因重复加载
            return;
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            Transform father=bot.transform;
            switch(layer)
            {
                case E_UI_Layer.Mid:
                    father=mid;
                    break;
                case E_UI_Layer.Top:
                    father=top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            //设置父对象
            obj.transform.SetParent(father);
            //初始化对象位置
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            //(obj.transform as RectTransform).offsetMax = Vector3.zero;
            //(obj.transform as RectTransform).offsetMin = Vector3.zero;
            //得到预设体上的脚本
            T panel = obj.GetComponent<T>();
            //处理面板创建后的逻辑
            if(callBack!=null)
                callBack(panel);
            //存面板
            //panelDic.Add(panelName,panel);
        });
    }
    /// <summary>
    /// 删除面板
    /// </summary>
    /// <param name="panelName"></param>
    public void DestoryPanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].DestoryMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            panelDic[panelName].gameObject.SetActive(false);

        }
    }
    /// <summary>
    /// 得到一个已经显示的面板 方便使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetPanel<T>(string name)where T:BasePanel
    {
        if(panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        return null;
    }
    /// <summary>
    /// 通过层级枚举 得到对应层级的父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null ;
    }
    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger=control.GetComponent<EventTrigger>();
        if (trigger==null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        trigger.triggers.Add(entry);
    }
}
