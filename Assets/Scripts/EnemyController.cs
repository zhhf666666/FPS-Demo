using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent EnemyAgent;
    public bool IsLiving = true;
    public HealthController HC;
    public float LerpRatio = 0.1f;
    public float DeathTime = 1;
    public GameObject RobotExplosion;
    public HoverBotAnimatorController HBAC;
    public bool IsLocking = false;

    void Start()
    {
    
    }

    void Update()
    {
        
    }

    public void RandomNavigation()
    {
        if(!EnemyAgent.pathPending && EnemyAgent.remainingDistance < 0.5f)
        {
            EnemyAgent.destination = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
        }
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
        PlayRobotExplosion();
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

    private void PlayRobotExplosion()
    {
        if(RobotExplosion)
        {
            GameObject NewExplosion = Instantiate(RobotExplosion, this.transform.position, RobotExplosion.transform.rotation);
            Destroy(NewExplosion, 2);
        }
    }
}
