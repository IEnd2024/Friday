using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        //��Ѫ�¼�
        EventCenter.GetInstance().addEventListener<int>("HP", (value) =>
        {
            if (int.Parse(hp.text) >= 0 && int.Parse(hp.text) <= 22)
            {
                hp.text = (int.Parse(hp.text) + value).ToString();
                if(int.Parse(hp.text)<0)
                {
                    print("��Ϸʧ��");
                }
            }
            else
            {
                print("��Ϸʧ��");
            }
        });
        //�غϽ����¼�
        EventCenter.GetInstance().addEventListener("EndRoundOfHp", () =>
        {
            EventCenter.GetInstance().EventTrigger("DestroyBlankCard");
            EventCenter.GetInstance().EventTrigger("TotalStageChange", GameCtrl.TotalState);
            EventCenter.GetInstance().EventTrigger<UnityAction<int>>(GameCtrl.nowAdvCard.MyId + "EndRoundOfDeHp", (value) =>
            {
                if (value > 0)
                {
                    GameCtrl.nowState = Game_State.DestroyBatCard;
                    //�غϽ�����Ѫ
                    EventCenter.GetInstance().EventTrigger("HP", -value);
                    //���ݿ�Ѫ��ѡ��ݻٿ���
                    EventCenter.GetInstance().EventTrigger("DestoryHP", value);
                    EventCenter.GetInstance().EventTrigger("ShowText", "����ð��ʧ��");
                }
                else if (value < 0)
                {
                    GameCtrl.nowState = Game_State.Begin;
                    EventCenter.GetInstance().EventTrigger("ShowText", "����ð�ճɹ�");
                    EventCenter.GetInstance().EventTrigger("EndWinRound");
                    EventCenter.GetInstance().EventTrigger(GameCtrl.nowAdvCard.MyId + "EndWinRound");
                }
            });
        });
    }
    
}
