using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldLibraryCtrl : BasePanel
{
    #region 单例
    // 静态实例变量  
    private static OldLibraryCtrl _instance;
    // 私有构造函数，防止外部直接实例化  
    private OldLibraryCtrl() { }
    // 静态属性，用于获取单例实例  
    public static OldLibraryCtrl Instance
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
    private Image oldPoint;
    public List<BatCardView> oldCards=new List<BatCardView>();
    private void Start()
    {
        oldPoint = GetControl<Image>("OldPile");
        oldCards = BaseCard.GetInstance().SetCard("OldCard", oldPoint.transform, oldCards);

        EventCenter.GetInstance().addEventListener<List<BatCardView>>("AddOldToBat", (cardList) =>
        {
            cardList.Add(oldCards[oldCards.Count - 1]);
            oldCards.RemoveAt(oldCards.Count - 1);
        });
    }
}
