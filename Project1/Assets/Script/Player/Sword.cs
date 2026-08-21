using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage = 10;
    private HashSet<Monster> hitMonsters = new HashSet<Monster>();

    public void EnableCollider()
    {
        hitMonsters.Clear();
        GetComponent<Collider>().enabled = true;
    }

    public void DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster"))
            return;

        Monster monster = other.GetComponent<Monster>();

        if (monster == null)
            return;

        // 공격에 이미 맞은 몬스터라면 무시
        if (hitMonsters.Contains(monster))
            return;

        hitMonsters.Add(monster);

        monster.TakeDamage(damage);
    }
}
