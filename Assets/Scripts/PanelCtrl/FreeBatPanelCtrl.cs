using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreeBatPanelCtrl : BasePanel
{
    // ��̬ʵ������  
    private static FreeBatPanelCtrl _instance;
    // ˽�й��캯������ֹ�ⲿֱ��ʵ����  
    private FreeBatPanelCtrl() { }
    // ��̬���ԣ����ڻ�ȡ����ʵ��  
    public static FreeBatPanelCtrl Instance
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
