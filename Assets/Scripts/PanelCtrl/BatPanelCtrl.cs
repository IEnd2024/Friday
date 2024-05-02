using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatPanelCtrl : BasePanel
{
    // ��̬ʵ������  
    private static BatPanelCtrl _instance;
    // ˽�й��캯������ֹ�ⲿֱ��ʵ����  
    private BatPanelCtrl() { }
    // ��̬���ԣ����ڻ�ȡ����ʵ��  
    public static BatPanelCtrl Instance
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

    public List<BatCardView> batCards = new List<BatCardView>();
    private void Start()
    {
        EventCenter.GetInstance().addEventListener<BatCardView>("GetBatCard", (obj) =>
        {
            batCards.Add(obj);
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
    }
    public bool IsBat(BatCardView card)
    {
        if (batCards.Contains(card)) return true;
        return false;
    }
}
