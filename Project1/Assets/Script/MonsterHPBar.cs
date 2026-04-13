using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public Image fillImage;
    private Transform target; // 몬스터

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void UpdateHP(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
    void LateUpdate()
    {
        // 카메라 바라보게
        transform.forward = Camera.main.transform.forward;

        // 몬스터 따라가기
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 2f, 0);
        }
    }
}
