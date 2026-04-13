using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("스탯")]
    public float maxHP= 100f;
    public float currentHP;
    public MonsterHPBar hpBar;

    //public float attackDamage = 10f;

    [Header("감지범위")]
    public float detectRange = 10f;
    public float attackRange = 5f;

    // [Header("상태")]
    public enum MonsterState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

   [Header("참조")]
    public Transform target;

    public MonsterState currentState;

    private MonsterAI ai;
    private MonsterAnimation anim;


    private void Awake()
    {
        ai = GetComponent<MonsterAI>();
        anim = GetComponent<MonsterAnimation>();
        hpBar = GetComponentInChildren<MonsterHPBar>();
    }
    void Start()
    {
        currentHP = maxHP;
        hpBar.SetTarget(transform);
        hpBar.UpdateHP(currentHP, maxHP);
    }

    private void Update()
    {
        if (currentState == MonsterState.Dead) return;

        ai.HandleState();
    }
    public void ChangeState(MonsterState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case MonsterState.Idle:
                break;

            case MonsterState.Chase:
                //anim.PlayRun();
                break;

            case MonsterState.Attack:
                //anim.PlayAttack();
                break;

            case MonsterState.Dead:
                //anim.PlayDeath();
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentState == MonsterState.Dead) return;

        hpBar.UpdateHP(currentHP, maxHP);

        //anim.PlayHit(); 

        if (currentHP <= 0)
        {
            ChangeState(MonsterState.Dead);
            Debug.Log("죽음");
        }
    }
}
