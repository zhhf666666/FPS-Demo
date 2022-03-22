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

    void Awake()
    {
        Current = Max;
        Total = Max * 3;
    }
    
    void Start()
    {
    
    }

    void Update()
    {
        
    }

    public void SetText()
    {
        BulletAmount.text = Current.ToString() + '/' + Total.ToString();
    }

    public void Reload()
    {
        if(Total == 0)
            return;
        if(Current + Total >= Max)
        {
            Total -= (Max - Current);
            Current = Max;
        }
        else
        {
            Current += Total;
            Total = 0;
        }
        if(this.transform.localPosition.y > -10)
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
        if(Total == 0 || Current == Max)
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
