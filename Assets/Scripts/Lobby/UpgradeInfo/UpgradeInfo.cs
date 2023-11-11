using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInfo : MonoBehaviour
{
    private string name;
    private string detail;
    private float price;

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetPrice(float price)
    {
        this.price = price;
    }

    public void SetDetail(string detail)
    {
        this.detail = detail;
    }

    public string GetName()
    {
        return this.name;
    }

    public string GetDetail()
    {
        return this.detail;
    }

    public float GetPrice()
    {
        return this.price;
    }
}
