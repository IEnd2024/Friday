using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCardCtrl : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<PirateCardView>().Init(this.GetComponent<PirateCardModel>().NewData);
    }
}
