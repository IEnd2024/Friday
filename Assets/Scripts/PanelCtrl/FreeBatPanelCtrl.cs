using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FreeBatPanelCtrl : BasePanel
{
    #region 单例
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
    #endregion
    public List<BatCardView> batFreeCards = new List<BatCardView>();
    private void Start()
    {
        EventCenter.GetInstance().addEventListener<BatCardView>("GetFreeBatCard", (obj) =>
        {
            batFreeCards.Add(obj);
            if (obj.skillName.text == "停止" && GameCtrl.nowState == Game_State.GetFreeBatCard)
            {
                GameCtrl.nowState = Game_State.GetBatCard;
                obj.bk.GetComponent<Outline>().effectColor = Color.yellow;
            }
        });
        EventCenter.GetInstance().addEventListener("DestroyBlankCard", () =>
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
