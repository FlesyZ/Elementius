using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public int range;   // 0 if it is in a short range;

    float timer;

    Collider2D collision { get { return GetComponent<Collider2D>(); } }
    Player player { get { return GetComponentInParent<Player>(); } }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            timer = 0f;
            collision.enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.Damage(player.stat.ATK - enemy.stat.DEF + Random.Range(-1, 3), player.stat.eKeep);
        }
    }
}
