using System;
using System.Collections.Generic;
using UnityEngine;
public class Initialization : MonoBehaviour
{
    private Dictionary<string, List<CardData>> cardsData=new Dictionary<string, List<CardData>>();

    public Dictionary<string, List<CardData>> CardsData { get => cardsData; }

    private void Awake()
    {
        NewGameInit();
        //###�����㷨
    }

    private void NewGameInit()
    {
        print(Application.persistentDataPath);
        //ð�տ���ʼ��
        if (!cardsData.ContainsKey("AdventureCard"))
            cardsData.Add("AdventureCard", new List<CardData>());
        cardsData["AdventureCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultAdvData");
        BaseCard.GetInstance().CreateCard<AdvCardView>("AdventureCard", cardsData["AdventureCard"], (o) =>{ });
        //AboutAdventure();
        //ս������ʼ��
        if (!cardsData.ContainsKey("BattleCard"))
            cardsData.Add("BattleCard", new List<CardData>());
        cardsData["BattleCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultBatData");
        BaseCard.GetInstance().CreateCard< BatCardView>("BattleCard", cardsData["BattleCard"], (o) => { });
        //AboutBattle();
        //���꿨��ʼ��
        if (!cardsData.ContainsKey("OldCard"))
            cardsData.Add("OldCard", new List<CardData>());
        cardsData["OldCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultOldData");
        BaseCard.GetInstance().CreateCard<BatCardView>("OldCard", cardsData["OldCard"], (o) => { });

        //AboutOld();
        //��������ʼ��
        if (!cardsData.ContainsKey("PirateCard"))
            cardsData.Add("PirateCard", new List<CardData>());
        cardsData["PirateCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultPirateData");
        //AboutPriate();
        //�����ܽ����ʼ��
        AboutBKPanel();
        
    }
    /// <summary>
    /// ð�տ���ʼ��
    /// </summary>
    //private void AboutAdventure()
    //{
    //    for (int i = 0; i < cardsData["AdventureCard"].Count; i++)
    //    {
    //        int j = i;
    //        UIManager.GetInstance().ShowPanel<AdvCardView>("Card/AdvCardPanel", E_UI_Layer.Mid, (obj) =>
    //        {
    //            obj.GetComponent<AdvCardModel>().Init(cardsData["AdventureCard"][j], j);
    //            //�ڻ����д洢����
    //            BaseCard.GetInstance().AddCard("AdventureCard", obj);
    //        });
    //    }
    //}
    /// <summary>
    /// ս������ʼ��
    /// </summary>
    //private void AboutBattle()
    //{
    //    for (int i = 0; i < cardsData["BattleCard"].Count; i++)
    //    {
    //        int j = i;
    //        UIManager.GetInstance().ShowPanel<BatCardView>("Card/BatCardPanel", E_UI_Layer.Mid, (obj) =>
    //        {
    //            obj.GetComponent<BatCardModel>().Init(cardsData["BattleCard"][j], j);
    //            //�ڻ����д洢����
    //            BaseCard.GetInstance().AddCard("BattleCard", obj);
    //        });
    //    }
    //}
    /// <summary>
    /// ���꿨��ʼ��
    /// </summary>
    private void AboutOld()
    {
        for (int i = 0; i < cardsData["OldCard"].Count; i++)
        {
            int j = i;
            UIManager.GetInstance().ShowPanel<BatCardView>("Card/BatCardPanel", E_UI_Layer.Mid, (obj) =>
            {
                obj.GetComponent<BatCardModel>().Init(cardsData["OldCard"][j], j);
                //�ڻ����д洢����
                BaseCard.GetInstance().AddCard("OldCard", obj);
            });
        }
    }
    /// <summary>
    /// ��������ʼ��
    /// </summary>
    private void AboutPriate()
    {
        for (int i = 0; i < CardsData["PirateCard"].Count; i++)
        {
            int j = i;
            UIManager.GetInstance().ShowPanel<PirateCardView>("Card/PirateCardPanel", E_UI_Layer.Mid, (obj) =>
            {
                obj.GetComponent<PirateCardModel>().Init(CardsData["PirateCard"][j], j);
                //�ڻ����д洢����
                BaseCard.GetInstance().AddCard("PirateCard", obj.GetComponent<PirateCardCtrl>());
            });
        }
    }
    /// <summary>
    /// �����ܽ����ʼ��
    /// </summary>
    private void AboutBKPanel()
    {
        UIManager.GetInstance().ShowPanel<BKPanelCtrl>("Panel/BKPanel", E_UI_Layer.Bot, (obj) => { });
    }
}
