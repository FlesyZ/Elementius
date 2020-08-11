using UnityEngine;

public class Attack : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && GetComponentInParent<Player>())
        {
            Player player = GetComponentInParent<Player>();
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.stat.TakeDamage(player, enemy);
        }
        else if (other.CompareTag("Player") && GetComponentInParent<Enemy>())
        {
            Player player = other.GetComponent<Player>();
            Enemy enemy = GetComponentInParent<Enemy>();
            player.stat.TakeDamage(enemy, player);
        }
    }
}