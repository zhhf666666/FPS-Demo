using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    public EnemyController EC;
    public int BombDamage = 100;
    public GameObject Player;
    public float MinDistance = 6;
    public float MinBombDistance = 0.6f;
    public GameObject Explosion; 

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(!EC.IsLiving)
            return;
        if(CheckDistance() || EC.IsLocking)
        {
            EC.EnemyAgent.destination = Player.transform.position;
            EC.HBAC.Alerted = true;
            EC.HBAC.MoveSpeed = EC.EnemyAgent.speed;
        }
        else
        {
            EC.RandomNavigation();
            EC.HBAC.Alerted = false;
        }
        CheckBomb();
    }

    bool CheckDistance()
    {
        if(Vector3.Distance(EC.EnemyAgent.transform.position, Player.transform.position) < MinDistance)
            return true;
        else
            return false;
    }

    private void CheckBomb()
    {
        if(Vector3.Distance(this.transform.position, Player.transform.position) < MinBombDistance)
        {
            EC.Death();
            GameObject NewExplosion = Instantiate(Explosion, this.transform.position, Explosion.transform.rotation);
            Destroy(NewExplosion, 2);
        }
    }

}
