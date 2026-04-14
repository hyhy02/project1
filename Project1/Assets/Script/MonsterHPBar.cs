using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public Slider slider_HP;
    private Transform target; // 몬스터
        private float targetValue;   // 목표 값 (실제 HP)
    private float smoothSpeed = 8f; // 부드러운 속도

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void UpdateHP(float current, float max)
    {
        targetValue =  Mathf.Clamp01(current / max);
    }
    void LateUpdate()
    {
        slider_HP.value = Mathf.Lerp(slider_HP.value, targetValue, Time.deltaTime * smoothSpeed);

        // 카메라 바라보게
        transform.forward = Camera.main.transform.forward;

        // 몬스터 따라가기
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 2f, 0);
        }
    }
}
