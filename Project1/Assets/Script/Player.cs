using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 이동 속도")]
    [Range(0f, 30f)] public float walkSpeed = 1f;
    [Range(0f, 30f)] public float runSpeed = 3f;

    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Dodge,
        Attack
    }

    public PlayerState currentState = PlayerState.Idle;
}
