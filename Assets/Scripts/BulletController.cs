using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Player_Bullet = 0,
    Enemy_Bullet = 1,
}

public class BulletController : MonoBehaviour
{
    public BulletType BT = BulletType.Player_Bullet;
    public int BulletDamage = 10;
    public GameObject BulletExplosion;
    public GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(BT == BulletType.Player_Bullet && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<HealthController>().Damage(BulletDamage);
            collision.gameObject.GetComponent<EnemyController>().HBAC.TriggerOnDamage();
            collision.gameObject.GetComponent<EnemyController>().SetIsLockingTrue();
            PlayBulletExplosion(this.transform.position);
        }
        else if(BT == BulletType.Enemy_Bullet && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthController>().Damage(BulletDamage);
            PlayBulletExplosion(Player.transform.position);
        }
        Destroy(gameObject);
    }

    private void PlayBulletExplosion(Vector3 pos)
    {
        if(BulletExplosion)
        {
            GameObject NewExplosion = Instantiate(BulletExplosion, pos, BulletExplosion.transform.rotation);
            Destroy(NewExplosion, 2);
        }
    }
}
