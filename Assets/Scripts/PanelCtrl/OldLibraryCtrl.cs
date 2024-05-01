using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldLibraryCtrl : BasePanel
{
    private Image oldPoint;
    private List<BatCardView> oldCards=new List<BatCardView>();
    private void Start()
    {
        oldPoint = GetControl<Image>("OldPile");
        oldCards = BaseCard.GetInstance().SetCard("OldCard", oldPoint.transform, oldCards);

        EventCenter.GetInstance().addEventListener<List<BatCardView>>("AddOldToBat", (cardList) =>
        {
            if (cardList.Count > 0)
            {
                cardList.Add(oldCards[oldCards.Count - 1]);
                oldCards.RemoveAt(oldCards.Count - 1);
            }
        });
    }
}
