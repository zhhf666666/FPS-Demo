using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public float PH = 100;
    public float MaxPH = 300;
    public Slider PHSlider;
    //public TextMesh HealthNum;
    public GameObject RobotExplosion;

    void Start()
    {
        PHSlider.value = PH;
    }

    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        PH -= damage;
        if(PH > 0)
        {
            PHSlider.value = PH;
            //if(gameObject.CompareTag("Player"))
            //{
            //    HealthNum.text = PH.ToString();
            //}
        }
        if(PH <= 0)
        {
            // Death
            PHSlider.value = 0;
            PlayRobotExplosion();
            
        }
    }

    private void PlayRobotExplosion()
    {
        if(RobotExplosion)
        {
            GameObject NewExplosion = Instantiate(RobotExplosion, this.transform.position, RobotExplosion.transform.rotation);
            Destroy(NewExplosion, 2);
        }
    }
}
