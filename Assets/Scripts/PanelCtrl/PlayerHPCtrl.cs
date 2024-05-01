using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHPCtrl : BasePanel
{
    private TextMeshProUGUI hp;
    void Start()
    {
        Init();
    }
    private void Init()
    {
        hp = GetControl<TextMeshProUGUI>("hp");
        //扣血事件
        EventCenter.GetInstance().addEventListener<int>("HP", (value) =>
        {
            if (int.Parse(hp.text) > 0 && int.Parse(hp.text) <= 22)
            {
                hp.text = (int.Parse(hp.text) + value).ToString();
            }
            else
            {
                //GameFailed
            }
        });
        //回合结算事件
        EventCenter.GetInstance().addEventListener("EndRoundOfHp", () =>
        {
            EventCenter.GetInstance().EventTrigger<UnityAction<int>>("EndRoundOfDeHp", (value) =>
            {
                if (value > 0)
                {
                    GameCtrl.nowState = Game_State.DestroyBatCard;
                    //回合结束扣血
                    EventCenter.GetInstance().EventTrigger("HP", -value);
                    //根据扣血数选择摧毁卡牌
                    EventCenter.GetInstance().EventTrigger("DestoryHP", value);
                    EventCenter.GetInstance().EventTrigger("ShowText", "本次冒险失败");
                }
                else if (value == 0)
                {
                    GameCtrl.nowState = Game_State.Begin;
                    EventCenter.GetInstance().EventTrigger("ShowText", "本次冒险成功");
                    EventCenter.GetInstance().EventTrigger("EndWinRound");
                    EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "EndWinRound");
                }
            });
        });
    }
    
}
