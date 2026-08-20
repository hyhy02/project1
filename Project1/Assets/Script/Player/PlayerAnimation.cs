using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerController controller;
    Player playerData;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        playerData = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        PlayGuard();
    }

    private void UpdateAnimator()
    {
        float speed = 0f;

        if (!controller.IsGrounded)
        {
            animator.SetBool("IsJump", true);
            return;
        }
        else
        {
            animator.SetBool("IsJump", false);
        }
        if (controller.input.magnitude > 0.01f)
        {
            if (playerData.currentState == Player.PlayerState.Walk)
            {
                speed = 0.5f;
            }
            else if (playerData.currentState == Player.PlayerState.Run)
            {
                speed = 1f;
            }
        }

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    // 콤보 공격 애니메이션
    public void PlayAttack(int step)
    {
        animator.SetTrigger("Attack" + step);
    }

    // 가드 애니메이션
    private void PlayGuard()
    {
        animator.SetBool("IsGuard", controller.isGuard);
    }
    // 가드 중 피격 애니메이션
    public void PlayGuardHit()
    {
        animator.SetTrigger("IsGuardHit");
    }

    // 피격 애니메이션
    public void PlayHit()
    {
        animator.SetTrigger("IsHit");
    }

    // 죽음 애니메이션
    public void PlayDeath()
    {
        animator.SetTrigger("IsDie");
    }
}
