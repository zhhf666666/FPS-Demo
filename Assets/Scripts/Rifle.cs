using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public Transform BulletStartPos;
    public GameObject Bullet;
    public float BulletSpeed = 50.0f;
    public float FireIntervalTime = 0.05f;
    public Transform DefaultPos;
    public Transform BackPos;
    public float LerpRatio = 0.2f;
    public AudioSource ShotAudio;
    public float RecoilAngle_X = 1;
    public float RecoilTime = 0.04f;
    public float RecoverTime = 0.08f;
    public Transform Eye;
    public float VerticalOffsetLimitation = 60;
    public Transform WeaponCamera;
    public int Damage = 10;
    public Transform[] ReloadObject;
    private bool IsReloading = false;
    private int IsReloadingUp = 1;
    public float ReloadSpeed = 0.01f;
    public float ReloadWaitTime = 1;
    public BulletAmountController BAC;
    private bool IsFire = false;
    public WeaponSwitch WS;
    public AudioSource ReloadAuido;

    void Start()
    {
        this.transform.localPosition = new Vector3(0, -100, 0);
    }

    void Update()
    {
        if(WS.GM.IsPause)
            return;
        if(WS.Current == 1 && WS.CanFire == true)
        {
            OpenFire();
            RayFunc();
            Reloading();
        }
    }

    private void OpenFire()
    {
        if(!BAC.CheckCurrent())
        {
            return;
        }
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
            if(!BAC.CheckCurrent())
            {
                IsFire = false;
                break;
            }
            if(BulletStartPos != null && Bullet != null)
            {
                GameObject NewBullet = Instantiate(Bullet, BulletStartPos.position, BulletStartPos.rotation);
                NewBullet.GetComponent<Rigidbody>().velocity = NewBullet.transform.forward * BulletSpeed;
                NewBullet.GetComponent<BulletController>().BT = BulletType.Player_Bullet;
                NewBullet.GetComponent<BulletController>().BulletDamage = Damage;
                BAC.Consume();
                PlayShotAudio();
                WeaponCamera.localEulerAngles = Vector3.zero;
                StopCoroutine("Recoil");
                StopCoroutine("RecoilAnimation");
                StartCoroutine("Recoil");
                StartCoroutine("RecoilAnimation");
                Destroy(NewBullet, 1);
                yield return new WaitForSeconds(FireIntervalTime);
            }
        }       
    }

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
            while(this.transform.localPosition.z != BackPos.localPosition.z)
            {
                float temp = Mathf.Lerp(this.transform.localPosition.z, BackPos.localPosition.z, LerpRatio*4);
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, temp);
                yield return null;
            }
            while(this.transform.localPosition != DefaultPos.localPosition)
            {
                float temp = Mathf.Lerp(this.transform.localPosition.z, DefaultPos.localPosition.z, LerpRatio);
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, temp);
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

    private void Reloading()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(!IsReloading && BAC.CheckReload())
            {
                StartCoroutine("Reload");
            }
        }
    }

    IEnumerator Reload()
    {
        IsReloading = true;
        ReloadAuido.Play();
        IsReloadingUp = 4;
        while(IsReloadingUp != 0)
        {
            for(int i=0;i<4;i++)
            {
                float x,y;
                if(IsReloadingUp > 0)
                {
                    if(ReloadObject[i].localPosition.y == 0.56f)
                        continue;
                    y = ReloadObject[i].localPosition.y + 0.696f * ReloadSpeed * Time.deltaTime;
                    if(y >= 0.56f)
                    {
                        if(i <= 1)
                            x = -0.146f;
                        else
                            x = 0.146f;
                        ReloadObject[i].localPosition = new Vector3(x, 0.56f, ReloadObject[i].localPosition.z);
                        IsReloadingUp--;
                        if(IsReloadingUp == 0)
                        {
                            IsReloadingUp = -4;
                            yield return new WaitForSeconds(ReloadWaitTime);
                        }
                            
                    }
                    else
                    {
                        if(i<=1)
                            x = ReloadObject[i].localPosition.x - 0.304f * ReloadSpeed * Time.deltaTime;
                        else
                            x = ReloadObject[i].localPosition.x + 0.304f * ReloadSpeed * Time.deltaTime;
                        ReloadObject[i].localPosition = new Vector3(x, y, ReloadObject[i].localPosition.z);
                    }
                }
                else if(IsReloadingUp < 0)
                {
                    if(ReloadObject[i].localPosition.y == 0.4f)
                        continue;
                    y = ReloadObject[i].localPosition.y - 0.696f * ReloadSpeed * Time.deltaTime;
                    if(y <= 0.4f)
                    {
                        if(i <= 1)
                            x = -0.076f;
                        else
                            x = 0.076f;
                        ReloadObject[i].localPosition = new Vector3(x, 0.4f, ReloadObject[i].localPosition.z);
                        IsReloadingUp++;
                    }
                    else
                    {
                        if(i<=1)
                            x = ReloadObject[i].localPosition.x + 0.304f * ReloadSpeed * Time.deltaTime;
                        else
                            x = ReloadObject[i].localPosition.x - 0.304f * ReloadSpeed * Time.deltaTime;
                        ReloadObject[i].localPosition = new Vector3(x, y, ReloadObject[i].localPosition.z);
                    }
                }
            }
            yield return null;
        }
        IsReloading = false;
        ReloadAuido.Stop();
        BAC.Reload();
    }

    public void Interrupt()
    {
        if(IsReloading)
        {
            StopCoroutine("Reload");
            ReloadObject[0].localPosition = new Vector3(-0.076f, 0.4f, ReloadObject[0].localPosition.z);
            ReloadObject[1].localPosition = new Vector3(-0.076f, 0.4f, ReloadObject[1].localPosition.z);
            ReloadObject[2].localPosition = new Vector3(0.076f, 0.4f, ReloadObject[2].localPosition.z);
            ReloadObject[3].localPosition = new Vector3(0.076f, 0.4f, ReloadObject[3].localPosition.z);
            IsReloading = false;
            ReloadAuido.Stop();
        }
    }
}
