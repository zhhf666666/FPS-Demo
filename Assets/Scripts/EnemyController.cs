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
    private bool IsLiving = true;
    public HealthController HC;
    public float LerpRatio = 0.1f;
    public float DeathTime = 1;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(!IsLiving)
            return;
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

    public void Birth()
    {
        IsLiving = true;
        EnemyAgent.enabled = true;
        HC.Reset();
    }

    public void Death()
    {
        IsLiving = false;
        EnemyAgent.enabled = false;
        StartCoroutine("DeathAnimation");
    }

    IEnumerator DeathAnimation()
    {
        while(this.transform.localEulerAngles.x < 89)
        {
            float angle = Mathf.Lerp(this.transform.localEulerAngles.x, 90, LerpRatio);
            this.transform.localEulerAngles = new Vector3(angle, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
            yield return null;
        }
        yield return new WaitForSeconds(DeathTime);
        this.transform.position = new Vector3(0, -100, 0);
    }
}
