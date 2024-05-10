using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyLibraryCtrl : BasePanel
{
    #region 单例
    // 静态实例变量  
    private static DestroyLibraryCtrl _instance;
    // 私有构造函数，防止外部直接实例化  
    private DestroyLibraryCtrl() { }
    // 静态属性，用于获取单例实例  
    public static DestroyLibraryCtrl Instance
    {
        get
        {
            return _instance;
        }
    }
    //唤醒时获取单例实例
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
