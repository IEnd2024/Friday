using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyLibraryCtrl : BasePanel
{
    #region ����
    // ��̬ʵ������  
    private static DestroyLibraryCtrl _instance;
    // ˽�й��캯������ֹ�ⲿֱ��ʵ����  
    private DestroyLibraryCtrl() { }
    // ��̬���ԣ����ڻ�ȡ����ʵ��  
    public static DestroyLibraryCtrl Instance
    {
        get
        {
            return _instance;
        }
    }
    //����ʱ��ȡ����ʵ��
    private void OnEnable()
    {
        _instance = this;
    }
    #endregion
    public Image destroyPoint;
    private List<BatCardView> destroyList=new List<BatCardView>();
    void Start()
    {
        destroyPoint = GetControl<Image>("DestroyPile");
        EventCenter.GetInstance().addEventListener<BatCardView>("DestroyCard", (obj) =>
        {
            obj.bk.color = Color.gray;
            destroyList.Add(obj);
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, destroyPoint.transform);
        });
    }

    
}
