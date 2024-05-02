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
   
        //抽卡事件
        EventCenter.GetInstance().addEventListener<Game_State>("GetBatCard_btn", (layer) =>
        {
            EventCenter.GetInstance().EventTrigger("BatShuffle");
            if (layer == Game_State.GetFreeBatCard && batGetCards.Count > 0)
            {
                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId+"FreeBatCardCount");
                if (layer == Game_State.GetFreeBatCard)
                {
                    EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId+"GetFreeBatCard", batGetCards[batGetCards.Count - 1]);
                    EventCenter.GetInstance().EventTrigger("GetFreeBatCard", batGetCards[batGetCards.Count - 1]);
                    batGetCards.RemoveAt(batGetCards.Count - 1);
                }
            }
            if (layer == Game_State.GetBatCard && batGetCards.Count > 0)
            {
                EventCenter.GetInstance().EventTrigger("HP", -1);
                EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId+"GetBatCard", batGetCards[batGetCards.Count - 1]);
                EventCenter.GetInstance().EventTrigger("GetBatCard", batGetCards[batGetCards.Count - 1]);
                batGetCards.RemoveAt(batGetCards.Count - 1);
            }
            print(batGetCards.Count + "get");
            print(batDiscardCards.Count + "discard");

        });
        //存入弃牌堆
        EventCenter.GetInstance().addEventListener<BatCardView>("SaveToDiscardOfBat", (obj) =>
        {
            batDiscardCards.Add(obj); 
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, batDiscardPile.transform);
        });
        //洗牌事件
        EventCenter.GetInstance().addEventListener("BatShuffle", () =>
        {
            if (batGetCards.Count == 0)
            {
                //添加老化牌至弃牌堆
                EventCenter.GetInstance().EventTrigger("AddOldToBat", batDiscardCards);
                //洗牌并存入抽牌堆
                batGetCards = BaseCard.GetInstance().SetCard("BattleCard", batGetPile.transform, batDiscardCards);
                batDiscardCards.Clear(); 
            }
        });
        //冒险胜利转化冒险卡为战斗卡逻辑
        EventCenter.GetInstance().addEventListener<AdvCardView>("ChangeAdvToBat", (advObj) =>
        {
            EventCenter.GetInstance().EventTrigger<UnityAction<CardData>>(advObj.MyId + "advInfoInit", (value) =>
            {

                BaseCard.GetInstance().CreateCard<BatCardView>("BattleCard", new List<CardData> { value }, (obj) =>
                {
                    //在基类中删除冒险卡
                    BaseCard.GetInstance().RemoveCard("AdventureCard", advObj);
                    advObj.ActiveUpdata(false);
                    Destroy(advObj.gameObject);
                    //存入弃牌堆
                    EventCenter.GetInstance().EventTrigger("SaveToDiscardOfBat",
                        BaseCard.GetInstance().GetCard<BatCardView>("BattleCard")
                        [BaseCard.GetInstance().GetCard<BatCardView>("BattleCard").Count - 1]);
                });
            });
            
        });
    }
}
