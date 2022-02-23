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
    public float BulletDamage = 10;
    public GameObject BulletExplosion;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(BT == BulletType.Player_Bullet && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<HealthController>().Damage(BulletDamage);
            collision.gameObject.GetComponent<EnemyController>().Ani.TriggerOnDamage();
            PlayBulletExplosion();
        }
        else if(BT == BulletType.Enemy_Bullet && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthController>().Damage(BulletDamage);
            PlayBulletExplosion();
        }
        Destroy(gameObject);
    }

    private void PlayBulletExplosion()
    {
        if(BulletExplosion)
        {
            GameObject NewExplosion = Instantiate(BulletExplosion, this.transform.position, BulletExplosion.transform.rotation);
            Destroy(NewExplosion, 2);
        }
    }
}
