using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public Transform BulletStartPos;
    public GameObject Bullet;
    public float BulletSpeed = 50.0f;
    public float FireIntervalTime = 0.1f;
    private bool CanFire = true;
    public Transform DefaultPos;
    public Transform BackPos;
    public float LerpRatio = 0.2f;
    public AudioSource ShotAudio;
    public float RecoilAngle_X = 2;
    public float RecoilTime = 0.06f;
    public float RecoverTime = 0.09f;
    public Transform Eye;
    public float VerticalOffsetLimitation = 60;
    public Transform WeaponCamera;
    public float Damage = 10;


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
        /* Running Fire
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
        */

        // Single Fire
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine("SingleFire");
        }
    }

    IEnumerator SingleFire()
    {
        if(CanFire && BulletStartPos != null && Bullet != null)
        {
            GameObject NewBullet = Instantiate(Bullet, BulletStartPos.position, BulletStartPos.rotation);
            NewBullet.GetComponent<Rigidbody>().velocity = NewBullet.transform.forward * BulletSpeed;
            NewBullet.GetComponent<BulletController>().BT = BulletType.Player_Bullet;
            NewBullet.GetComponent<BulletController>().BulletDamage = Damage;
            PlayShotAudio();
            WeaponCamera.localEulerAngles = Vector3.zero;
            StopCoroutine("Recoil");
            StopCoroutine("RecoilAnimation");
            StartCoroutine("Recoil");
            StartCoroutine("RecoilAnimation");
            Destroy(NewBullet, 3);
            CanFire = false;
            yield return new WaitForSeconds(FireIntervalTime);
            CanFire = true;
        }       
    }

    /*
    IEnumerator RunningFire()
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
                Destroy(NewBullet, 3);
            }
            yield return new WaitForSeconds(FireIntervalTime);
        }       
    }
    */

    IEnumerator Recoil()
    {
        if(Eye != null) 
        {
            // Recoil
            float TempTime = 0;
            while(TempTime < RecoilTime)
            {
                TempTime += Time.deltaTime;
                float offset = RecoilAngle_X * Time.deltaTime / RecoilTime;
                float new_angle = Eye.localEulerAngles.x - offset;
                float new_angle2 = WeaponCamera.localEulerAngles.x + offset;
                WeaponCamera.localEulerAngles = new Vector3(new_angle2, 0, 0);
                Eye.localEulerAngles = new Vector3(new_angle, Eye.localEulerAngles.y, Eye.localEulerAngles.z);
                yield return null;
            }
            // Recover
            TempTime = 0;
            while(TempTime < RecoverTime)
            {
                TempTime += Time.deltaTime;
                float offset = RecoilAngle_X * Time.deltaTime / RecoverTime;
                float new_angle = Eye.localEulerAngles.x + offset;
                float new_angle2 = WeaponCamera.localEulerAngles.x - offset;
                WeaponCamera.localEulerAngles = new Vector3(new_angle2, 0, 0);
                Eye.localEulerAngles = new Vector3(new_angle, Eye.localEulerAngles.y, Eye.localEulerAngles.z);
                yield return null;
            }
            WeaponCamera.localEulerAngles = Vector3.zero;
        }
    }

    IEnumerator RecoilAnimation()
    {
        if(DefaultPos != null && BackPos != null)
        {
            while(this.transform.localPosition != BackPos.localPosition)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, BackPos.localPosition, LerpRatio*4);
                yield return null;
            }
            while(this.transform.localPosition != DefaultPos.localPosition)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, DefaultPos.localPosition, LerpRatio);
                yield return null;
            }
        }
    }

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
