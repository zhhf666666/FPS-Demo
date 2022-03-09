using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] Weapons;
    public EyeCameraController ECC;
    public int Current = 0;
    private int Next = 0;
    private int WeaponSize;

    void Start()
    {
        WeaponSize = Weapons.Length;
    }

    void Update()
    {
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
            Next = (Current + WeaponSize - 1) % WeaponSize;
            SwitchWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Next = (Current + 1) % WeaponSize;
            SwitchWeapon();
        }

    }

    void SwitchWeapon()
    {
        Weapons[Current].transform.localPosition = new Vector3(0, -100, 0);
        Weapons[Next].transform.localPosition = new Vector3(0, 0, 0);
        Weapons[Next].GetComponent<BulletAmountController>().SetText();
        //StartCoroutine("RotateWeapon");
        Current = Next;
    }
}
