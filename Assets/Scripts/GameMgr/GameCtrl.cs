using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardData
{
    public int defaultId;
    public string advName;
    public string batName;
    public string skillName;
    public int freeCardValue;
    public int hp;
    public int advValue1;
    public int advValue2;
    public int advValue3;
    public int combatValue;
    public CardData() { }

    public CardData(CardData card)
    {
        this.defaultId = card.defaultId;
        this.advName = card.advName;
        this.batName = card.batName;
        this.skillName = card.skillName;
        this.freeCardValue = card.freeCardValue;
        this.hp = card.hp;
        this.advValue1 = card.advValue1;
        this.advValue2 = card.advValue2;
        this.advValue3 = card.advValue3;
        this.combatValue = card.combatValue;
    }


}
public enum Game_State
{
    Begin,
    GetAdvCard,
    GetFreeBatCard,
    GetBatCard,
    DestroyBatCard,
    State_Green,
    State_Yellow,
    State_Red,
    State_Pirate,
    End,
}
public class GameCtrl : MonoBehaviour
{
    static public Game_State nowState=Game_State.Begin;
    static public Game_State TotalState = Game_State.State_Green;
    static public AdvCardView nowAdvCard;
    // Start is called before the first frame update
    void Start()
    {
        
        Init();
    }

    private void Init()
    {
        EventListener();


    }
    private void EventListener()
    {
        //获取现在进行的冒险卡
        EventCenter.GetInstance().addEventListener<AdvCardView>("StartAdv", (obj) =>
        {
            nowAdvCard = obj;
            print(nowAdvCard.MyId);
        });
    }
}
