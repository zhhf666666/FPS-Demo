using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public EnemyController EC;

    void Update()
    {
        if(!EC.IsLiving)
            return;
        if(EC.IsLocking)
        {
            EC.TrackPlayer();
        }
        else
        {
            EC.RandomNavigation();
        }
    }
}
