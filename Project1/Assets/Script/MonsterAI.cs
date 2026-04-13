using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    private Monster monster;
    private Transform player;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        monster.target = player;

    }

    public void HandleState()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        switch (monster.currentState)
        {
            case Monster.MonsterState.Idle:
                if (distance < monster.detectRange)
                {
                    monster.ChangeState(Monster.MonsterState.Chase);
                }
                break;

            case Monster.MonsterState.Chase:
                ChasePlayer();

                if (distance <= monster.attackRange)
                {
                    monster.ChangeState(Monster.MonsterState.Attack);
                    Debug.Log("공격");
                }
                break;

            case Monster.MonsterState.Attack:
                if (distance > monster.attackRange)
                {
                    monster.ChangeState(Monster.MonsterState.Chase);
                }
                break;
        }
    }
    private void ChasePlayer()
    {
        // NavMesh 사용해서 기능 구현하기
        //Debug.Log("추적");
    }
}
