using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class BatLibraryCtrl : BasePanel
{
    private Image batGetPile;
    private Image batDiscardPile;
    private List<BatCardView> batGetCards = new List<BatCardView>();
    private List<BatCardView> batDiscardCards = new List<BatCardView>();
    private void Start()
    {
        Init();

    }
    private void Init()
    {
        batGetPile = GetControl<Image>("BatGetPile");
        batDiscardPile = GetControl<Image>("BatDiscardPile");
        batGetCards = BaseCard.GetInstance().SetCard("BattleCard", batGetPile.transform, batGetCards);
        EventListener();
    }
    
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "BatGetName_btn":
                EventCenter.GetInstance().EventTrigger("GetBatCard_btn",GameCtrl.nowState);
                break;
            case "BatDiscardName_btn":

                break;
        }
    }
    private void EventListener()
    {
        //≥Èø® ¬º˛
        EventCenter.GetInstance().addEventListener<Game_State>("GetBatCard_btn", (layer) =>
        {
            EventCenter.GetInstance().EventTrigger("BatShuffle");
            if (batGetCards.Count > 0  )
            {
                if(GameCtrl.TotalState != Game_State.State_Pirate)
                {
                    if (layer == Game_State.GetFreeBatCard)
                    {
                        EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "FreeBatCardCount", -1);
                        if (layer == Game_State.GetFreeBatCard)
                        {
                            EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetFreeBatCard", batGetCards[batGetCards.Count - 1]);
                            EventCenter.GetInstance().EventTrigger("GetFreeBatCard", batGetCards[batGetCards.Count - 1]);
                            batGetCards.RemoveAt(batGetCards.Count - 1);
                        }
                    }
                    else if (layer == Game_State.GetBatCard)
                    {
                        EventCenter.GetInstance().EventTrigger("HP", -1);
                        EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "GetBatCard", batGetCards[batGetCards.Count - 1]);
                        EventCenter.GetInstance().EventTrigger("GetBatCard", batGetCards[batGetCards.Count - 1]);
                        batGetCards.RemoveAt(batGetCards.Count - 1);
                    }
                }
                else
                {
                    if (layer == Game_State.GetFreeBatCard)
                    {
                        EventCenter.GetInstance().EventTrigger(GameCtrl.nowPirateCard.MyId + "FreeBatCardCount", -1);
                        if (layer == Game_State.GetFreeBatCard)
                        {
                            EventCenter.GetInstance().EventTrigger("GetFreeBatCard", batGetCards[batGetCards.Count - 1]);
                            batGetCards.RemoveAt(batGetCards.Count - 1);
                        }
                    }
                    else if (layer == Game_State.GetBatCard)
                    {
                        EventCenter.GetInstance().EventTrigger("HP", -1);
                        EventCenter.GetInstance().EventTrigger("GetBatCard", batGetCards[batGetCards.Count - 1]);
                        batGetCards.RemoveAt(batGetCards.Count - 1);
                    }
                } 
            } 
        });
        //µ√µΩ≈∆∂—∂•µ⁄“ª’≈≈∆
        EventCenter.GetInstance().addEventListener<UnityAction<List<BatCardView>>>("GetFirstBat", (action) =>
        {
            action(batGetCards);
        });
        //¥Ê»Î∆˙≈∆∂—
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToDiscardOfBat", (obj) =>
        {
            batDiscardCards.Add(obj); 
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, batDiscardPile.transform);
        });
        //¥Ê»Î≥È≈∆∂—
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToGetOfBat", (obj) =>
        {
            batGetCards.Add(obj);
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, batGetPile.transform);
        });
        //¥Ê»Î≥È≈∆∂—∂•
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToGetTopOfBat", (obj) =>
        {
            batGetCards.Insert(0, obj);
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, batGetPile.transform);
        });
        //œ¥≈∆ ¬º˛
        EventCenter.GetInstance().addEventListener("BatShuffle", () =>
        {
            if (batGetCards.Count == 0)
            {
                //ÃÌº”¿œªØ≈∆÷¡∆˙≈∆∂—
                EventCenter.GetInstance().EventTrigger("AddOldToBat", batDiscardCards);
                //œ¥≈∆≤¢¥Ê»Î≥È≈∆∂—
                batGetCards = BaseCard.GetInstance().SetCard("BattleCard", batGetPile.transform, batDiscardCards);
                batDiscardCards.Clear(); 
            }
        });
        //√∞œ’ §¿˚◊™ªØ√∞œ’ø®Œ™’Ω∂∑ø®¬ﬂº≠
        EventCenter.GetInstance().addEventListener<AdvCardView>("ChangeAdvToBat", (advObj) =>
        {
            EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(advObj.MyId + "advInfoInit", (value) =>
            {

                BaseCard.GetInstance().CreateCard<BatCardView>("BattleCard", new List<CardData> { value }, (obj) =>
                {
                    //‘⁄ª˘¿‡÷–…æ≥˝√∞œ’ø®
                    BaseCard.GetInstance().RemoveCard("AdventureCard", advObj);
                    advObj.ActiveUpdata(false);
                    Destroy(advObj.gameObject);
                    //¥Ê»Î∆˙≈∆∂—
                    EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat",
                        BaseCard.GetInstance().GetCard<BatCardView>("BattleCard")
                        [BaseCard.GetInstance().GetCard<BatCardView>("BattleCard").Count - 1]);
                });
            });
            
        });
    }
}
