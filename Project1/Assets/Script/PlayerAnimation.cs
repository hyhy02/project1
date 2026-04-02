using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerController controller;
    Player playerData;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        playerData = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        float speed = 0f;

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

}
