using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] Weapons;
    public EyeCameraController ECC;
    public int Current = 0;
    private int Next = 0;
    public float LerpRatio = 0.1f;
    public Vector3[] CameraCenterParameter;
    public bool CanFire = true;
    public Image[] Icons;
    public GameManager GM;

    void Start()
    {
        Icons[0].color = new Color32(255, 255, 255, 255);
        Icons[1].color = new Color32(152, 152, 152, 255);
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if(GM.IsPause)
            return;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Current != 0)
            {
                Next = 0;
                SwitchWeapon();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(Current != 1)
            {
                Next = 1;
                SwitchWeapon();
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Next = (Current + Weapons.Length - 1) % Weapons.Length;
            SwitchWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Next = (Current + 1) % Weapons.Length;
            SwitchWeapon();
        }

    }

    void SwitchWeapon()
    {
        StopCoroutine("RotateWeapon");
        CanFire = false;
        Icons[Current].color = new Color32(152, 152, 152, 255);
        Icons[Next].color = new Color32(255, 255, 255, 255);
        ECC.WeaponCameraCenterPos = CameraCenterParameter[Next];
        if(Input.GetMouseButton(1))
        {
            ECC.DefaultToCenterFunc();
        }
        Weapons[Current].transform.localPosition = new Vector3(0, -100, 0);
        if(Current == 0)
            Weapons[Current].GetComponent<Pistol>().Interrupt();
        else
            Weapons[Current].GetComponent<Rifle>().Interrupt();
        StartCoroutine("RotateWeapon", Next);
        Weapons[Next].GetComponent<BulletAmountController>().SetText();
        Current = Next;
    }

    IEnumerator RotateWeapon(int index)
    {
        Weapons[index].transform.localEulerAngles = new Vector3(90, 0 ,0);
        Weapons[index].transform.localPosition = new Vector3(0, -1, 0);
        while(Weapons[index].transform.localEulerAngles.x != 0 || Weapons[index].transform.localPosition.y != 0)
        {
            // 忽略插值误差
            if(Weapons[index].transform.localEulerAngles.x < 1)
                Weapons[index].transform.localEulerAngles = new Vector3(0, 0 ,0);
            if(Weapons[index].transform.localPosition.y > -0.01f)
                Weapons[index].transform.localPosition = new Vector3(0, 0, 0);
            if(Weapons[index].transform.localEulerAngles.x != 0)
            {
                float angle = Mathf.Lerp(Weapons[index].transform.localEulerAngles.x, 0, LerpRatio);
                Weapons[index].transform.localEulerAngles = new Vector3(angle, 0 ,0);
            }
            if(Weapons[index].transform.localPosition.y != 0)
            {
                float pos = Mathf.Lerp(Weapons[index].transform.localPosition.y, 0, LerpRatio);
                Weapons[index].transform.localPosition = new Vector3(0, pos, 0);
            }
            yield return null;
        }
        CanFire = true;
    }
}
