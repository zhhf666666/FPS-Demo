using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int HP = 100;
    public int MaxPH = 300;
    public Slider PHSlider;
    public Text HealthNum;
    public GameObject RobotExplosion;

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
        HP -= damage;
        if(HP > 0)
        {
            PHSlider.value = HP;
        }
        if(HP <= 0)
        {
            // Death
            PHSlider.value = 0;
            PlayRobotExplosion();   
        }
        SetText();
    }

    private void PlayRobotExplosion()
    {
        if(RobotExplosion)
        {
            GameObject NewExplosion = Instantiate(RobotExplosion, this.transform.position, RobotExplosion.transform.rotation);
            Destroy(NewExplosion, 2);
        }
    }

    private void SetText()
    {
        HealthNum.text = "HP: " + HP.ToString();
    }
}
