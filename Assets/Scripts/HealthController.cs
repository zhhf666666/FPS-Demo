using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float PH = 100;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        if(PH > 0)
            PH -= damage;
        if(PH <= 0)
        {
            // Death
        }
    }
}
