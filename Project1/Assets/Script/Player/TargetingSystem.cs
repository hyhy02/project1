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
                currentTargetPoint = null;
            }
        }

        CheckTarget();
    }

    // 카메라 기준 타겟 잡기
    public void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange, monsterLayer);

        Camera cam = Camera.main;
        float minAngle = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (var hit in hits)
        {

            // 뷰포트 좌표로 변환 (0~1이 화면 안)
            Vector3 viewportPos = cam.WorldToViewportPoint(hit.transform.position);

            // 몬스터가 화면 밖이거나 카메라 뒤에 있으면 락온 실행 x
            bool inScreen = viewportPos.x >= 0f && viewportPos.x <= 1f &&
                            viewportPos.y >= 0f && viewportPos.y <= 1f &&
                            viewportPos.z > 0f; // z > 0 = 카메라 앞

            if (!inScreen) continue;

            // 카메라 → 몬스터 방향과 카메라 정면 방향의 각도 차이로 비교
            Vector3 dirToMonster = (hit.transform.position - cam.transform.position).normalized;
            float angle = Vector3.Angle(cam.transform.forward, dirToMonster);
            
            if (angle < minAngle)
            {
                minAngle = angle;
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

        Camera cam = Camera.main;
        List<Transform> targets = new List<Transform>();

        foreach (var hit in hits)
        {
            // 화면 안에 있는 몬스터만 추가
            Vector3 viewportPos = cam.WorldToViewportPoint(hit.transform.position);

            bool inScreen = viewportPos.x >= 0f && viewportPos.x <= 1f &&
                        viewportPos.y >= 0f && viewportPos.y <= 1f &&
                        viewportPos.z > 0f;

            if (!inScreen) continue;
        
            targets.Add(hit.transform);
        }

        targets.Sort((a, b) =>
        {
            Vector3 dirA = a.position - cam.transform.position;
            Vector3 dirB = b.position - cam.transform.position;

            float angleA = Vector3.SignedAngle(cam.transform.forward, dirA, cam.transform.up);
            float angleB = Vector3.SignedAngle(cam.transform.forward, dirB, cam.transform.up);

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
