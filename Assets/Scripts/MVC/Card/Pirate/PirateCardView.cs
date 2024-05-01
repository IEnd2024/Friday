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
    /// ��ʼ����ȡ�ؼ�
    /// </summary>
    public void Init(CardData date)
    {
        UpdateInfo(date);
    }

    //���¿ؼ�����
    public void UpdateInfo(CardData date)
    {
        this.gameObject.name = date.advName;
        nameText.text = date.advName;
        freeCardValue.text = date.freeCardValue.ToString();
        skillName.text = date.skillName;
        advValue1.text = date.advValue1.ToString();
    }
}
