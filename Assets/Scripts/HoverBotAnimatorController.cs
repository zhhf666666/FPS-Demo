using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBotAnimatorController : MonoBehaviour
{
    public float MoveSpeed;
    public bool Alerted;
    public Animator HoverBotAnimator;
    void Start()
    {
        HoverBotAnimator = this.GetComponentInChildren<Animator>();
    }

    private void SetParameters()
    {
        if(HoverBotAnimator == null)
            return;
        HoverBotAnimator.SetFloat("MoveSpeed", MoveSpeed);
        HoverBotAnimator.SetBool("Alerted", Alerted);
    }

    void Update()
    {
        SetParameters();
    }

    public void TriggerAttack()
    {
        if(HoverBotAnimator == null)
            return;
        HoverBotAnimator.SetTrigger("Attack");
    }

    public void TriggerOnDamage()
    {
        if(HoverBotAnimator == null)
            return;
        HoverBotAnimator.SetTrigger("OnDamaged");
    }
}
