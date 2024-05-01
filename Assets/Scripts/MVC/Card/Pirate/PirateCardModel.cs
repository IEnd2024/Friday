using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCardModel : MonoBehaviour
{

    private int id;
    private CardData newData;

    public int Id { get => id; }
    public CardData NewData { get => newData; }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void Init(CardData defaultData, int id)
    {
        //��ʼ������
        this.id = id;
        newData = new CardData(defaultData);
        //����������json�ļ�
        Save();
    }
    /// <summary>
    /// ��������
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, "PirtaeNewData", JsonType.LitJson);
    }
}
