using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyLibraryCtrl : BasePanel
{
    private Image destroyPoint;
    private List<BatCardView> destroyList=new List<BatCardView>();
    void Start()
    {
        destroyPoint = GetControl<Image>("DestroyPile");
        EventCenter.GetInstance().addEventListener<BatCardView>("DestroyCard", (obj) =>
        {
            destroyList.Add(obj);
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, destroyPoint.transform);

            
        });
    }

    
}
