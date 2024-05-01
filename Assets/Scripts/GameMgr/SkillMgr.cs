using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkillMgr :BaseManager<SkillMgr>
{
    private string skillName;
    public bool isSkill=false;
    public void Init( string skill)
    {
        skillName = skill;
        isSkill = true;
        UIManager.GetInstance().ShowPanel<SkillPanelCtrl>("Panel/SkillPanel", E_UI_Layer.Bot, SkillLogic);
    }

    private void SkillLogic(SkillPanelCtrl panel)
    {
        switch (skillName)
        {
            case "+2����":
                panel.Init(skillName, "�ٳ�һ��", "ȡ��",
                    () =>
                    {
                        EventCenter.GetInstance().EventTrigger("GetBatCard_btn", Game_State.GetBatCard);
                        EventCenter.GetInstance().EventTrigger("HP", 1);
                        GameObject.Destroy(panel.gameObject);
                    },
                    () =>
                    {
                        GameObject.Destroy(panel.gameObject);
                    });
                break;
        }
        panel.transform.localPosition = Vector3.up * Screen.height * 1/4;
    }
}
