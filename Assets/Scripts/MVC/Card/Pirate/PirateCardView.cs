using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PirateCardView : BasePanel
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI freeCardValue;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI advValue1;
    public Image advBK;
    /// <summary>
    /// 初始化获取控件
    /// </summary>
    public void Init(CardData date)
    {
        UpdateInfo(date);
    }

    //更新控件数据
    public void UpdateInfo(CardData date)
    {
        this.gameObject.name = date.advName;
        nameText.text = date.advName;
        freeCardValue.text = date.freeCardValue.ToString();
        skillName.text = date.skillName;
        advValue1.text = date.advValue1.ToString();
    }
}
