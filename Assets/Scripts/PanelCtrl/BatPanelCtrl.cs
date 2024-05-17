using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatPanelCtrl : BasePanel
{
    #region 单例
    // 静态实例变量  
    private static BatPanelCtrl _instance;
    // 私有构造函数，防止外部直接实例化  
    private BatPanelCtrl() { }
    // 静态属性，用于获取单例实例  
    public static BatPanelCtrl Instance
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
    public List<BatCardView> batCards = new List<BatCardView>();
    private void Start()
    {
        EventCenter.GetInstance().addEventListener<BatCardView>("GetBatCard", (obj) =>
        {
            if (!batCards.Contains(obj))
            {
                EventCenter.GetInstance().EventTrigger("BatPanelOfCount",-1);
                batCards.Add(obj);
            }
                
        });
        EventCenter.GetInstance().addEventListener("DestroyBlankCard", () =>
        {
            if (batCards.Count > 0)
            {
                foreach (BatCardView card in batCards)
                {
                    if (card.isEnable == false)
                        EventCenter.GetInstance().EventTrigger("DestroyCard", card);
                }
            }
            batCards.Clear();
        });
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToDiscardOfBat", (obj) =>
        {
            if (!batCards.Contains(obj))
            {
                EventCenter.GetInstance().EventTrigger("BatPanelOfCount", 1);
                batCards.Remove(obj);
            }
        });
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToGetOfBat", (obj) =>
        {
            if (!batCards.Contains(obj))
            {
                EventCenter.GetInstance().EventTrigger("BatPanelOfCount", 1);
                batCards.Remove(obj);
            }
        });
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToGetBotOfBat", (obj) =>
        {
            if (!batCards.Contains(obj))
            {
                EventCenter.GetInstance().EventTrigger("BatPanelOfCount", 1);
                batCards.Remove(obj);
            }
        });
    }
    public bool IsBat(BatCardView card)
    {
        if (batCards.Contains(card)) return true;
        return false;
    }
}
