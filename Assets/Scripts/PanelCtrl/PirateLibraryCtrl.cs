using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateLibraryCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private List<PirateCardView> pirateCards=new List<PirateCardView>();
    private bool victory = false;
    void Start()
    {
        pirateCards=BaseCard.GetInstance().SetCard("PirateCard", this.transform, pirateCards);
        EventCenter.GetInstance().addEventListener<PirateCardView>("SelectPirate", (obj) =>
        {
            pirateCards.Remove(obj);
            if (pirateCards.Count > 0)
                EventCenter.GetInstance().EventTrigger
                (pirateCards[0].MyId + "PirateActiveUpdata", false);
        });
        EventCenter.GetInstance().addEventListener<PirateCardView>("WinThePirate", (obj) =>
        {
            obj.bk.color = Color.green;
            pirateCards.Add(obj);
            BaseCard.GetInstance().SetCard("PirateCard", this.transform, pirateCards);
            victory = true;
            foreach (PirateCardView card in pirateCards)
                if (card.bk.color != Color.green)
                    victory = false;
            if (victory)
            {
                EventCenter.GetInstance().EventTrigger("ShowText", "ÓÎÏ·Ê¤Àû");
            }
        });
    }

}
