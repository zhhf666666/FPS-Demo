using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent EnemyAgent;
    public GameObject Player;
    public float MinDistance = 5;
    public HoverBotAnimatorController Ani;

    void Start()
    {
        EnemyAgent = this.GetComponent<NavMeshAgent>();
        Ani = this.GetComponent<HoverBotAnimatorController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        EnemyAgent.destination = this.transform.position;
    }

    void Update()
    {
        if(CheckDistance())
        {
            EnemyAgent.destination = Player.transform.position;
            Ani.Alerted = true;
            Ani.MoveSpeed = EnemyAgent.speed;
        }
        else
        {
            RandomNavigation();
            Ani.Alerted = false;
        }
    }

    void RandomNavigation()
    {
        if(!EnemyAgent.pathPending && EnemyAgent.remainingDistance < 0.5f)
        {
            EnemyAgent.destination = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
        }
    }

    bool CheckDistance()
    {
        if(Vector3.Distance(EnemyAgent.transform.position, Player.transform.position) < MinDistance)
            return true;
        else
            return false;
    }

}
