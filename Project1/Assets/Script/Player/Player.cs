using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 이동 속도")]
    [Range(0f, 30f)] public float walkSpeed = 1f;
    [Range(0f, 30f)] public float runSpeed = 3f;

    [Header("플레이어 체력, 스테미나")]
    public float maxHP = 100f;
    public float currentHP;

    public float maxStamina = 100f;
    public float currentStamina;

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

    void Awake()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
    }
}
