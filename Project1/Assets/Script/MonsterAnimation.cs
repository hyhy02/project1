using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRun()
    {
        animator.SetBool("Run", true);
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayHit()
    {
        animator.SetTrigger("Hit");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Die");
    }
}
