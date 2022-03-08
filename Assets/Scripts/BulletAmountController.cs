using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletAmountController : MonoBehaviour
{
    public Text BulletAmount;
    public int Max;
    private int Current;
    private int Total;

    void Start()
    {
        Current = Max;
        Total = Max * 2;
        SetText();
    }

    void Update()
    {
        
    }

    void SetText()
    {
        BulletAmount.text = Current.ToString() + '/' + Total.ToString();
    }

    public void Reload()
    {
        if(Total == 0)
            return;
        if(Total >= Max)
        {
            Current = Max;
            Total -= Max;
        }
        else
        {
            Current = Total;
            Total = 0;
        }
        SetText();
    }

    public bool CheckCurrent()
    {
        if(Current == 0)
            return false;
        else
            return true;
    }

    public bool CheckReload()
    {
        if(Total == 0)
            return false;
        else
            return true;
    }

    public void Consume()
    {
        if(Current == 0)
            return;
        Current--;
        SetText();
    }

    public void Add(int num)
    {
        Total += num;
        SetText();
    }
}
