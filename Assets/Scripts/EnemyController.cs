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
    public float MinDistance = 6;
    public GameObject Player;
    public int BombDamage = 100;
    public float MinBombDistance = 0.6f;
    public GameObject Explosion;
    public GameObject Exclamation;
    public float InitDistanceFromPlayer = 10;
    public GameManager GM;
    public AudioSource AlertAudio;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        IsLiving = false;
        EnemyAgent.enabled = false;
        IsLocking = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!IsLiving || GM.IsPause)
            return;
        if(CheckDistance(MinDistance))
        {
            SetIsLockingTrue();
        }
        CheckBomb();
    }

    public void SetIsLockingTrue()
    {
        if(IsLocking == true)
            return;
        IsLocking = true;
        AlertAudio.Play();
        GameObject exclamation = Instantiate(Exclamation, this.transform.position + Vector3.up, Exclamation.transform.rotation);
        exclamation.GetComponent<ParticleSystem>().Play();
        StartCoroutine("ParticleAnimation", exclamation);
        Destroy(exclamation,1);
    }

    IEnumerator ParticleAnimation(GameObject particle)
    {
        while(particle)
        {
            particle.transform.position = this.transform.position + Vector3.up;
            yield return null;
        }
    }

    public bool CheckDistance(float dis)
    {
        if(Vector3.Distance(EnemyAgent.transform.position, Player.transform.position) < dis)
            return true;
        else
            return false;
    }

    public bool CheckDistance(float dis, Vector3 vec)
    {
        if(Vector3.Distance(vec, Player.transform.position) < dis)
            return true;
        else
            return false;
    }

    public void RandomNavigation()
    {
        if(!EnemyAgent.pathPending && EnemyAgent.remainingDistance < 0.5f)
        {
            EnemyAgent.destination = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
        }
        HBAC.Alerted = false;
    }

    public void TrackPlayer()
    {
        EnemyAgent.destination = Player.transform.position;
        HBAC.Alerted = true;
        HBAC.MoveSpeed = EnemyAgent.speed;
    }

    public void Birth()
    {
        SetRandomPos();
        EnemyAgent.enabled = true;
        IsLiving = true;
        IsLocking = false;
        HC.Reset();
    }

    public void Death(bool IsBomb)
    {
        IsLiving = false;
        EnemyAgent.enabled = false;
        GM.DecreaseEnemy();
        if(!IsBomb)
        {
            PlayRobotExplosion();
            StartCoroutine("DeathAnimation");
        }
        else
            this.transform.position = new Vector3(0, -100, 0);
    }

    public void SetRandomPos()
    {
        Vector3 temp;
        while(true)
        {
            temp = new Vector3(Random.Range(-29f, 29f), 0, Random.Range(-29f, 29f));
            if(!CheckDistance(InitDistanceFromPlayer, temp))
                break;
        }
        this.transform.position = temp;
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

    private void CheckBomb()
    {
        if(CheckDistance(MinBombDistance))
        {
            GameObject NewExplosion = Instantiate(Explosion, this.transform.position + Vector3.up * 0.5f, Explosion.transform.rotation);    
            Death(true);
            Player.GetComponent<HealthController>().Damage(BombDamage);
            Destroy(NewExplosion, 2);
        }
    }
}
