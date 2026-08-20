using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 이동 속도")]
    [Range(0f, 30f)] public float walkSpeed = 1f; // 걷는 속도
    [Range(0f, 30f)] public float runSpeed = 3f; // 달리는 속도

    [Header("플레이어 체력, 스테미나")]
    public float maxHP = 100f; // 최대 체력
    public float currentHP; // 현재 체력

    public float maxStamina = 100f; // 최대 스테미너
    public float currentStamina; // 현재 스테미너

    [Header("상태")]
    public PlayerState currentState = PlayerState.Idle; // 플레이어 현재 상태
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Dodge,
        Attack,
        Die
    }

    void Awake()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
    }
}
