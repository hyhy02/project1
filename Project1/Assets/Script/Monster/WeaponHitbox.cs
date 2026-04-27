using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private Monster monster;
    private Collider hitCollider;
    private bool hasHitThisSwing = false; // 한 번의 공격에 한 번만 데미지

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
        hitCollider = GetComponent<Collider>();

        hitCollider.isTrigger = true;
        hitCollider.enabled = false;
    }
    
    public void EnableHitbox()
    {
        hasHitThisSwing = false;
        hitCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        hitCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasHitThisSwing) return;
        if (!other.CompareTag("Player")) return;

        hasHitThisSwing = true;
        monster.DealDamage();
    }
}
