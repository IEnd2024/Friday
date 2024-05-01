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
    /// 初始化数据
    /// </summary>
    public void Init(CardData defaultData, int id)
    {
        //初始化数据
        this.id = id;
        newData = new CardData(defaultData);
        //创建新数据json文件
        Save();
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    private void Save()
    {
        JsonMgr.GetInstance().SaveData(newData, "PirtaeNewData", JsonType.LitJson);
    }
}
