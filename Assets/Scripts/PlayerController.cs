using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float RotateSpeed = 180;
    [Range(1,2)]
    public float RotateRatio = 1;
    public Transform PlayerTrans;
    public Transform EyeTrans;
    public float VerticalOffsetLimitation = 60;
    public CharacterController CC;
    public float MoveSpeed = 10;
    public float G = 9.8f;
    public float FallVelocity = 0;
    public Transform CheckPoint;
    public float magrin = 0.1f;
    public float JumpSpeed = 5.0f;
    public bool IsLiving = true;

    void Start()
    {
        CC = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if(!IsLiving)
            return;
        PlayerRotateControl();
        PlayerMove();
    }

    private void PlayerRotateControl()
    {
        if(PlayerTrans == null || EyeTrans == null)
            return;
        float offset_x = Input.GetAxis("Mouse X");   // Horizontal
        float offset_y = Input.GetAxis("Mouse Y");   // Vertical
        PlayerTrans.Rotate(Vector3.up * offset_x * RotateSpeed * RotateRatio * Time.deltaTime);
        float VerticalOffset = CheckAngle(EyeTrans.localEulerAngles.x);
        VerticalOffset -= offset_y * RotateSpeed * RotateRatio * Time.deltaTime;
        VerticalOffset = Mathf.Clamp(VerticalOffset, -VerticalOffsetLimitation, VerticalOffsetLimitation);
        EyeTrans.localRotation = Quaternion.Euler(new Vector3(VerticalOffset, EyeTrans.localEulerAngles.y, EyeTrans.localEulerAngles.z));
    }

    private float CheckAngle(float value)
    {
        float angle = value - 180;
	    if(angle > 0)
		    return angle - 180;
	    return angle + 180;
    }

    public bool IsOnGround()
    {
        if(CheckPoint == null)
            return false;
        return Physics.Raycast(CheckPoint.position, Vector3.down, magrin);
    }

    public void PlayerMove()
    {
        if(CC == null)
            return;
        Vector3 MoveValue = Vector3.zero;
        float LeftRightValue = Input.GetAxis("Horizontal");
        float FrontBackValue = Input.GetAxis("Vertical");
        MoveValue += this.transform.forward * MoveSpeed * FrontBackValue * Time.deltaTime;
        MoveValue += this.transform.right * MoveSpeed * LeftRightValue * Time.deltaTime;

        // Fall
        if(IsOnGround() == false)
        {
            FallVelocity += G * Time.deltaTime;
        }
        else
        {
            FallVelocity = 0;
            if(Input.GetButtonDown("Jump"))
            {
                FallVelocity = -JumpSpeed;
            }
        }
        MoveValue += Vector3.down * FallVelocity * Time.deltaTime;

        CC.Move(MoveValue);
    }

    public void Birth()
    {
        this.transform.position = new Vector3(-29, 0, -29);
        this.transform.localEulerAngles = new Vector3(0, 45, 0);
        IsLiving = true;
        this.GetComponent<HealthController>().Reset();
    }
    
    public void Death()
    {
        int i;
    }
}
