using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [Header("스탯")]
    public float maxHP= 100f;
    public float currentHP;
    public MonsterHPBar hpBar;

    //public float attackDamage = 10f;

    [Header("감지범위")]
    public float detectRange = 10f;
    public float attackRange = 2f;

    [Header("상태")]
    public MonsterState currentState;
    public enum MonsterState
    {
        Idle,
        Chase,
        Attack,
        Hit,
        Dead
    }
    [Header("공격")]
    public float attackDamage = 10f;
    public float attackDelay = 3f; // 공격 간격
    public bool isAttacking = false;
    private Coroutine attackCoroutine;
    [Space]
    // public float attackCoolAfterHit = 1f; // 피격 후 공격 쿨타임
    // public float attackCoolTimer = 0f;
    public float hitRecoveryTime = 1f; // 경직시간
    [SerializeField] private float hitRecoveryTimer = 0f;

   [Header("참조")]
    public Transform target;
    private MonsterAI ai;
    private MonsterAnimation anim;
    private NavMeshAgent agent;


    private void Awake()
    {
        ai = GetComponent<MonsterAI>();
        anim = GetComponent<MonsterAnimation>();
        hpBar = GetComponentInChildren<MonsterHPBar>();
        agent = GetComponent<NavMeshAgent>();
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

        // 히트시 경직
        if (hitRecoveryTimer > 0)
        {
            hitRecoveryTimer -= Time.deltaTime;
            return; // ai 멈추기
        }

        // 피격 후 공격 쿨타임
        // if (attackCoolTimer > 0)
        // {
        //     attackCoolTimer -= Time.deltaTime;
        // }
        
        ai.HandleState();
    }
    public void ChangeState(MonsterState newState)
    {
        if (currentState == newState) return;

        // 공격 코루틴 정지 (상태 바뀔 때)
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isAttacking = false;
        }

        currentState = newState;

        switch (currentState)
        {
            case MonsterState.Idle:
                agent.isStopped = true;
                break;

            case MonsterState.Chase:
                agent.isStopped = false;
                anim.PlayRun();
                break;

            case MonsterState.Attack:
                agent.isStopped = true;
                if(!isAttacking)
                {
                    attackCoroutine = StartCoroutine(AttackRoutine());
                }
                break;
            case MonsterState.Hit:
                agent.isStopped = true;
                anim.PlayHit();

                hitRecoveryTimer = hitRecoveryTime;
                //attackCoolTimer = attackCoolAfterHit;
                
                StartCoroutine(HitRoutine());
                break;

            case MonsterState.Dead:
                agent.isStopped = true;
                //anim.PlayDeath();
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentState == MonsterState.Dead) return;

        currentHP -= damage;

        hpBar.UpdateHP(currentHP, maxHP);

        if (currentHP <= 0)
        {
            ChangeState(MonsterState.Dead);
            Debug.Log("죽음");
        }

        ChangeState(MonsterState.Hit);
    }

    // 공격 코루틴
    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 플레이어 바라보기
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;

        while (currentState == MonsterState.Attack)
        {
            anim.PlayRunStop();

            yield return new WaitForSeconds(0.5f);

            anim.PlayAttack(); // 공격 애니메이션

            yield return new WaitForSeconds(attackDelay);
        }

        isAttacking = false;
    }

    // 몬스터 공격시 데미지 주기
    public void DealDamage()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange + 0.5f)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
            }
        }
    }
    IEnumerator HitRoutine()
    {
        yield return new WaitForSeconds(0.5f); // 히트 애니메이션 길이

        ChangeState(MonsterState.Idle);
    }
}
