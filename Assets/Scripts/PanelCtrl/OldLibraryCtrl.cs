using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldLibraryCtrl : BasePanel
{
    #region ����
    // ��̬ʵ������  
    private static OldLibraryCtrl _instance;
    // ˽�й��캯������ֹ�ⲿֱ��ʵ����  
    private OldLibraryCtrl() { }
    // ��̬���ԣ����ڻ�ȡ����ʵ��  
    public static OldLibraryCtrl Instance
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
