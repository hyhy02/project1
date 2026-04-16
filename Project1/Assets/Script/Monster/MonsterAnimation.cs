using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAnimation : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void PlayRunStop()
    {
        animator.SetBool("Run", false);
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
