using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCameraController : MonoBehaviour
{
    public Camera MainCamera;
    public Camera WeaponCamera;
    public Vector3 WeaponCameraDefaultPos;
    public Vector3 WeaponCameraCenterPos;
    public float DefaultView = 60;
    public float CenterView = 30;
    public float LerpRatio = 0.2f;
    public GameManager GM;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        WeaponCameraCenterPos = new Vector3(0, 0.058f, -0.01f);   // Pistol
    }

    void Update()
    {
        if(GM.IsPause)
            return;
        ViewChange();
    }

    private void ViewChange()
    {
        if(Input.GetMouseButtonDown(1))
        {
            StopCoroutine("CenterToDefault");
            StartCoroutine("DefaultToCenter");
        }
        if(Input.GetMouseButtonUp(1))
        {
            StopCoroutine("DefaultToCenter");
            StartCoroutine("CenterToDefault");
        }
    }

    IEnumerator DefaultToCenter()
    {
        while(WeaponCamera.transform.localPosition != WeaponCameraCenterPos)
        {
            WeaponCamera.transform.localPosition = Vector3.Lerp(WeaponCamera.transform.localPosition, WeaponCameraCenterPos, LerpRatio);
            WeaponCamera.fieldOfView = Mathf.Lerp(WeaponCamera.fieldOfView, CenterView, LerpRatio);
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, CenterView, LerpRatio);
            yield return null;
        }
    }

    IEnumerator CenterToDefault()
    {
        while(WeaponCamera.transform.localPosition != WeaponCameraDefaultPos)
        {
            WeaponCamera.transform.localPosition = Vector3.Lerp(WeaponCamera.transform.localPosition, WeaponCameraDefaultPos, LerpRatio);
            WeaponCamera.fieldOfView = Mathf.Lerp(WeaponCamera.fieldOfView, DefaultView, LerpRatio);
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, DefaultView, LerpRatio);
            yield return null;
        }
    }
}
