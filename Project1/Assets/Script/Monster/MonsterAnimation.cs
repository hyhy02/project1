using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAnimation : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private WeaponHitbox weaponHitbox;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        weaponHitbox = GetComponentInChildren<WeaponHitbox>();
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
    // 애니메이션 이벤트에서 이 함수를 호출
    public void EnableHitbox()
    {
        weaponHitbox?.EnableHitbox();
    }

    public void DisableHitbox()
    {
        weaponHitbox?.DisableHitbox();
    }
}
