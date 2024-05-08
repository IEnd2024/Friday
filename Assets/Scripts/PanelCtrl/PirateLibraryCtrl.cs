using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateLibraryCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    private List<PirateCardView> pirateCards=new List<PirateCardView>();    
    void Start()
    {
        BaseCard.GetInstance().SetCard("PirateCard", this.transform, pirateCards);
        EventCenter.GetInstance().addEventListener<PirateCardView>("SelectPirate", (obj) =>
        {
            pirateCards.Remove(obj);
            if (pirateCards.Count > 0)
                EventCenter.GetInstance().EventTrigger
                (pirateCards[0].MyId + "PirateActiveUpdata", false);
        });
    }

}
