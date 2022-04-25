using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int HP;
    public int MaxHP = 300;
    public Slider PHSlider;
    public Text HealthNum;
    public EnemyController EC;
    public PlayerController PC;

    void Start()
    {
        PHSlider.value = HP;
        SetText();
    }

    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        if(HP == 0)
            return;
        HP -= damage;
        if(HP > 0)
        {
            PHSlider.value = HP;
        }
        if(HP <= 0)
        {
            // Death
            HP = 0;
            PHSlider.value = 0;
            if(EC)
            {
                EC.Death(false);
            }
            else if(PC)
            {
                PC.Death();
            }
        }
        SetText();
    }

    private void SetText()
    {
        if(HealthNum == null)
            return;
        HealthNum.text = "HP: " + HP.ToString();
    }

    public void Reset()
    {
        if(EC)
            HP = MaxHP;
        else if(PC)
            HP = 200;
        PHSlider.value = HP;
        SetText();
    }

    public void GetHP(int value)
    {
        if(PC)
        {
            HP += value;
            if(HP > MaxHP)
                HP = MaxHP;
            PHSlider.value = HP;
            SetText();
        }
    }
}
