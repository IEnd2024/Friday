using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreeBatPanelCtrl : BasePanel
{
    // 静态实例变量  
    private static FreeBatPanelCtrl _instance;
    // 私有构造函数，防止外部直接实例化  
    private FreeBatPanelCtrl() { }
    // 静态属性，用于获取单例实例  
    public static FreeBatPanelCtrl Instance
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

    public List<BatCardView> batFreeCards = new List<BatCardView>();
    private void Start()
    {
        EventCenter.GetInstance().addEventListener<BatCardView>("GetFreeBatCard", (obj) =>
        {
            batFreeCards.Add(obj);
        });
        EventCenter.GetInstance().addEventListener("EndRoundOfHp", () =>
        {
            if (batFreeCards.Count > 0)
            {
                foreach (BatCardView card in batFreeCards)
                {
                    if (card.isEnable == false)
                        EventCenter.GetInstance().EventTrigger("DestroyCard", card);
                }
            }
            batFreeCards.Clear();
        });
    }
    public bool IsFreeBat(BatCardView card)
    {
        if(batFreeCards.Contains(card)) return true;
        return false;
    }
}
