using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// ������
/// ��UI�ؼ�
/// </summary>
public class BasePanel : MonoBehaviour
{
    //�������滻ԭ��洢���пؼ�
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<Scrollbar>();
        FindChildrenControl<InputField>();
        FindChildrenControl<TextMeshProUGUI>();
    }
    /// <summary>
    /// ��ʾ�Լ�
    /// </summary>
    public virtual void ShowMe() { }
    /// <summary>
    /// ɾ���Լ�
    /// </summary>
    public virtual void DestoryMe() { }
    /// <summary>
    /// �����Լ�
    /// </summary>
    public virtual void HideMe() { }
    /// <summary>
    /// ��ť����¼�
    /// </summary>
    /// <param name="name"></param>
    protected virtual void OnClick(string btnName) { }
    protected virtual void OnValueChanged(string toggleName, bool value) { }
    /// <summary>
    /// �õ���Ӧ���ֵĿؼ��ű�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }
    /// <summary>
    /// �õ��Ӷ���Ķ�Ӧ�ؼ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        if (controls != null)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                string objName = controls[i].gameObject.name;
                if (controlDic.ContainsKey(objName))
                {
                    controlDic[objName].Add(controls[i]);
                }
                else
                {
                    controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
                }
                //����ǰ�ť
                if (controls[i] is Button)
                {
                    (controls[i] as Button).onClick.AddListener(() =>
                    {
                        OnClick(objName);
                    });
                }
                else if (controls[i] is Toggle)
                {
                    (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                    {
                        OnValueChanged(objName, value);
                    });
                }

            }

        }
    }

}