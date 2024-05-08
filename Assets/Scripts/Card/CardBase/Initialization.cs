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
        //ս������ʼ��
        if (!cardsData.ContainsKey("BattleCard"))
            cardsData.Add("BattleCard", new List<CardData>());
        cardsData["BattleCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultBatData");
        BaseCard.GetInstance().CreateCard< BatCardView>("BattleCard", cardsData["BattleCard"], (o) => { });
        //���꿨��ʼ��
        if (!cardsData.ContainsKey("OldCard"))
            cardsData.Add("OldCard", new List<CardData>());
        cardsData["OldCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultOldData");
        BaseCard.GetInstance().CreateCard<BatCardView>("OldCard", cardsData["OldCard"], (o) => { });

        //��������ʼ��
        if (!cardsData.ContainsKey("PirateCard"))
            cardsData.Add("PirateCard", new List<CardData>());
        cardsData["PirateCard"] = JsonMgr.GetInstance().LoadData<List<CardData>>("DefaultPirateData");
        BaseCard.GetInstance().CreateCard<BatCardView>("PirateCard", cardsData["PirateCard"], (o) => { });
        //�����ܽ����ʼ��
        AboutBKPanel();
        
    }
    /// <summary>
    /// �����ܽ����ʼ��
    /// </summary>
    private void AboutBKPanel()
    {
        UIManager.GetInstance().ShowPanel<BKPanelCtrl>("Panel/BKPanel", E_UI_Layer.Bot, (obj) => { });
    }
}
