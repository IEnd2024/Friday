using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// UI�㼶ö��
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}
/// <summary>
/// UI������
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    //��¼UI��Cavans ����ʹ��
    public RectTransform canvas;

    public UIManager()
    {
        //����Canvas����ʹ�䲻���л�����ʱ���
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        //�ҵ�����UI��
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //����system����ʹ�䲻���л�����ʱ���
        obj = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }
    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">���ű�����</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="layer">��ʾ����һ��</param>
    /// <param name="callBack">�����Ԥ���崴���ɹ��� ����ʵ�ֵķ���</param>
    public void ShowPanel<T>(string panelName,E_UI_Layer layer=E_UI_Layer.Mid,UnityAction<T> callBack=null)where T : BasePanel
    {
        //����Ѿ���ʾ
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].gameObject.SetActive(true);
            panelDic[panelName].ShowMe();
            if (callBack != null)
                callBack(panelDic[panelName] as T);
            //���������Ϊ�첽ԭ���ظ�����
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
            //���ø�����
            obj.transform.SetParent(father);
            //��ʼ������λ��
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            //(obj.transform as RectTransform).offsetMax = Vector3.zero;
            //(obj.transform as RectTransform).offsetMin = Vector3.zero;
            //�õ�Ԥ�����ϵĽű�
            T panel = obj.GetComponent<T>();
            //������崴������߼�
            if(callBack!=null)
                callBack(panel);
            //�����
            //panelDic.Add(panelName,panel);
        });
    }
    /// <summary>
    /// ɾ�����
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
    /// �������
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
    /// �õ�һ���Ѿ���ʾ����� ����ʹ��
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
    /// ͨ���㼶ö�� �õ���Ӧ�㼶�ĸ�����
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
    /// ���ؼ�����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callBack">�¼���Ӧ����</param>
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
