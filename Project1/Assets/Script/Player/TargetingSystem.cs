using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public float lockOnRange = 10f; // 락온 범위
    public LayerMask monsterLayer;

    public Transform currentTarget; // 현재 타겟(몬스터 root)
    public Transform currentTargetPoint; // 타겟팅 포인트 위치
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentTarget == null)
            {
                FindTarget();
            }
            else
            {
                currentTarget = null;
            }
        }

        CheckTarget();
    }

    // 가장 가까운 몬스터 타겟 찾기
    public void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange, monsterLayer);

        float minDist = Mathf.Infinity;
        Transform bestTarget = null;

        // 범위 안에서 가장 가까운 몬스터로.
        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                bestTarget = hit.transform;
            }
        }

        currentTarget = bestTarget;

        if (currentTarget != null)
        {
            Transform tp = currentTarget.Find("TargetPoint");

            if (tp != null)
            {
                currentTargetPoint = tp;
            }
            // else
            // {
            //     currentTargetPoint = currentTarget;
            // }
        }
    }

    private void CheckTarget()
    {
        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.position);

        // 거리 벗어나면 락온 해제
        if (dist > lockOnRange)
        {
            currentTarget = null;
            currentTargetPoint = null;
        }
            

        // 죽으면 락온 해제
        if (!currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = null;
            currentTargetPoint = null;
        }
    }
    
    public void SwitchTarget(bool toRight)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange, monsterLayer);

        List<Transform> targets = new List<Transform>();

        foreach (var hit in hits)
            targets.Add(hit.transform);

        targets.Sort((a, b) =>
        {
            Vector3 dirA = a.position - transform.position;
            Vector3 dirB = b.position - transform.position;

            float angleA = Vector3.SignedAngle(transform.forward, dirA, Vector3.up);
            float angleB = Vector3.SignedAngle(transform.forward, dirB, Vector3.up);

            return angleA.CompareTo(angleB);
        });

        if (targets.Count == 0) return;
        if (currentTarget == null)
        {
            currentTarget = targets[0];
            return;
        }

        int index = targets.IndexOf(currentTarget);

        if (toRight)
            index = (index + 1) % targets.Count;
        else
            index = (index - 1 + targets.Count) % targets.Count;

        currentTarget = targets[index];
    }
}
