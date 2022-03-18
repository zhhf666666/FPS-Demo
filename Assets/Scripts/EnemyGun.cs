using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public EnemyController EC;
    public float FireIntervalTime = 0.3f;
    public float AlertAngle = 10;
    public GameObject Player;
    public Transform BulletStartPos;
    public bool AllowFire = true;
    public GameObject Bullet;
    public int Damage = 20;
    public int BulletSpeed = 300;
    public AudioSource ShotAudio;
    public float OrignialSpeed = 3.5f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        AllowFire = true;
        EC.EnemyAgent.speed = OrignialSpeed;
    }

    void Update()
    {
        if(!EC.IsLiving)
            return;
        if(AllowFire && CheckView())
        {
            Vector3 BulletDirection = Player.transform.position + 0.5f * Vector3.up - BulletStartPos.position;
            BulletDirection.Normalize();
            if(RayFunc(BulletDirection))
            {
                EC.SetIsLockingTrue();
                StartCoroutine("OpenFire", BulletDirection);
            }
        }
        else
        {
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

    private bool CheckView()
    {
        Vector2 ViewDirection = new Vector2(this.transform.forward.x, this.transform.forward.z);
        Vector2 Player_Enemy = new Vector2(Player.transform.position.x - this.transform.position.x, Player.transform.position.z - this.transform.position.z);
        Player_Enemy.Normalize();
        float angle = Vector2.Angle(ViewDirection, Player_Enemy);
        if(angle <= AlertAngle)
            return true;
        else
            return false;
    }

    private bool RayFunc(Vector3 direction)
    {
        Ray ray = new Ray(BulletStartPos.position, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            return hit.transform.name == "Player";
        }
        return false;
    }

    IEnumerator OpenFire(Vector3 direction)
    {
        EC.EnemyAgent.speed = 0;
        AllowFire = false;
        GameObject NewBullet = Instantiate(Bullet, BulletStartPos.position, BulletStartPos.rotation);
        foreach(Transform temp in NewBullet.transform)
            temp.gameObject.layer = 0;
        // 子弹偏差
        direction += (Random.Range(-0.05f, 0.05f) * Vector3.up + Random.Range(-0.05f, 0.05f) * Vector3.right);
        //direction += Random.Range(-0.05f, 0.05f) * Vector3.right;
        direction.Normalize();
        NewBullet.GetComponent<Rigidbody>().velocity = direction * BulletSpeed;
        NewBullet.GetComponent<BulletController>().BT = BulletType.Enemy_Bullet;
        NewBullet.GetComponent<BulletController>().BulletDamage = Damage;
        ShotAudio.Play();
        Destroy(NewBullet, 1);
        yield return new WaitForSeconds(FireIntervalTime);
        EC.EnemyAgent.speed = OrignialSpeed;
        AllowFire = true;
    }
}
