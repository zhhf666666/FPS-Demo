using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public Transform BulletStartPos;
    public GameObject Bullet;
    public float BulletSpeed = 50.0f;
    public float FireIntervalTime = 0.1f;
    private bool IsFire;
    public Transform BackPos;
    public float LerpRatio = 0.2f;
    public AudioSource ShotAudio;


    void Start()
    {
        
    }

    void Update()
    {
        OpenFire();
        RayFunc();
    }

    private void OpenFire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            IsFire = true;
            StartCoroutine("Fire");
        }
        if(Input.GetMouseButtonUp(0))
        {
            IsFire = false;
            StopCoroutine("Fire");
        }
    }

    IEnumerator Fire()
    {
        while(IsFire)
        {
            if(BulletStartPos != null && Bullet != null)
            {
                GameObject NewBullet = Instantiate(Bullet, BulletStartPos.position, BulletStartPos.rotation);
                NewBullet.GetComponent<Rigidbody>().velocity = NewBullet.transform.forward * BulletSpeed;
                PlayShotAudio();
                //StopCoroutine("Recoil");
                //StartCoroutine("Recoil");
                Destroy(NewBullet, 5);
            }
            yield return new WaitForSeconds(FireIntervalTime);
        }       
    }

    /*IEnumerator Recoil()
    {
        if(BackPos != null)
        {
            while(this.transform.localPosition != BackPos.localPosition)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, BackPos.localPosition, LerpRatio*4);
                yield return null;
            }
            while(this.transform.localPosition != BackPos.localPosition)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, BackPos.localPosition, LerpRatio*4);
                yield return null;
            }
        }
    }*/

    private void PlayShotAudio()
    {
        if(ShotAudio)
        {
            ShotAudio.Play();
        }
    }

    private void RayFunc()
    {
        Ray ray = new Ray(BulletStartPos.position, BulletStartPos.forward);
        Debug.DrawRay(ray.origin, ray.direction*1000, Color.red);
    }
}
