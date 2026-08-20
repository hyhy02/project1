using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    private Monster monster;
    private Transform player;
    private NavMeshAgent agent;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        monster.target = player;

    }

    public void HandleState()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        switch (monster.currentState)
        {
            case Monster.MonsterState.Idle:
                if(distance<=monster.attackRange)
                {
                    monster.ChangeState(Monster.MonsterState.Attack);
                }
                else if (distance < monster.detectRange)
                {
                    monster.ChangeState(Monster.MonsterState.Chase);
                }
                break;

            case Monster.MonsterState.Chase:
                ChasePlayer();

                if (distance <= monster.attackRange)
                {
                    monster.ChangeState(Monster.MonsterState.Attack);
                }
                break;

            case Monster.MonsterState.Attack:
                agent.SetDestination(transform.position); // 멈추기
                
                // 몬스터 공격이 끝나면
                if(!monster.isAttacking)
                {
                    // 공격 범위 밖일 때, 추적상태로
                    if (distance > monster.attackRange)
                    {
                        monster.ChangeState(Monster.MonsterState.Chase);
                    }
                    else // 공격 범위 안일 때, Idle상태로
                    {
                        monster.ChangeState(Monster.MonsterState.Idle);
                    }
                }
                
                break;
        }
    }
    private void ChasePlayer()
    {
        // 플레이어 따라가게 함. 
        agent.SetDestination(player.position);
    }
}
