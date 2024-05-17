using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvLibraryCtrl : BasePanel
{

    
    private Image advGetPile;
    private Image advDiscardPile;
    private List<AdvCardView> advGetCards = new List<AdvCardView>();
    private List<AdvCardView> advDiscardCards = new List<AdvCardView>();
    private void Start()
    {
        Init();
        
    }
    private void Init()
    {
        advGetPile = GetControl<Image>("AdvGetPile");
        advDiscardPile = GetControl<Image>("AdvDiscardPile");
        advGetCards = BaseCard.GetInstance().SetCard("AdventureCard", advGetPile.transform, advGetCards);
        EventListener();
        
    }
    
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "AdvGetName_btn":
                if (GameCtrl.nowState == Game_State.Begin&&GameCtrl.TotalState!=Game_State.State_Pirate)
                {
                    EventCenter.GetInstance().EventTrigger("GetAdvCard", advGetCards);
                    GameCtrl.nowState = Game_State.GetAdvCard;
                }
                break;
            case "AdvDiscardName_btn":

                break;
        }
    }
    private void EventListener()
    {
        //��ʼ��ð�տ��׶α仯
        EventCenter.GetInstance().EventTrigger("TotalStageChange",GameCtrl.TotalState);
        //�ӳ��ƶ��Ƴ������ƣ�һ��Ϊ����ð�յģ�һ�ŷ������ƶ�
        EventCenter.GetInstance().addEventListener<AdvCardView>("StartAdv", (obj) =>
        {
            advGetCards.Remove(obj);
            if(advGetCards.Count != 0)
                EventCenter.GetInstance().EventTrigger("SaveToDiscardOfAdv", advGetCards.Last());
        });
        //�������ƶ�
        EventCenter.GetInstance().addEventListener<AdvCardView>("SaveToDiscardOfAdv", (obj) =>
        {
            if (advGetCards.Contains(obj))
                advGetCards.Remove(obj);
            advDiscardCards.Add(obj);
            obj.ActiveUpdata(false);
            BaseCard.GetInstance().TurnOverCard(obj, false, advDiscardPile.transform);
        });
        //ϴ���¼� ������Ϸ��һ�׶�
        EventCenter.GetInstance().addEventListener("AdvShuffle", () =>
        {
            
            if (advGetCards.Count == 0 && advDiscardCards.Count > 0)
            {
                if (GameCtrl.TotalState >= Game_State.State_Green)
                {
                    GameCtrl.TotalState++;
                    EventCenter.GetInstance().EventTrigger("TotalStageChange", GameCtrl.TotalState);
                    switch (GameCtrl.TotalState)
                    {
                        case Game_State.State_Yellow:
                            EventCenter.GetInstance().EventTrigger("ShowText", "ð���Ѷ�����������ͨ");
                            break;
                        case Game_State.State_Red:
                            EventCenter.GetInstance().EventTrigger("ShowText", "ð���Ѷ�������������");
                            break;
                        case Game_State.State_Pirate:
                            EventCenter.GetInstance().EventTrigger("ShowText", "������Ϯ");
                            break;
                        }
                    if (GameCtrl.TotalState == Game_State.State_Pirate)
                        EventCenter.GetInstance().EventTrigger("StartPirate");
                }
                //ϴ�Ʋ�������ƶ�
                advGetCards = BaseCard.GetInstance().SetCard("BattleCard", advGetPile.transform, advDiscardCards);
                advDiscardCards.Clear();
                print("game"+GameCtrl.TotalState);
            }
        });

    }

}
