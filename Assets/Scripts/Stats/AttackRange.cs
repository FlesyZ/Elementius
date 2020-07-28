using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public int range;
    Collider2D collision { get { return GetComponent<Collider2D>(); } }
    Player player { get { return GetComponentInParent<Player>(); } }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StatGeneral stat = other.GetComponent<Enemy>().stat;
            stat.Damage(player.stat.ATK - stat.DEF, stat.gameObject.transform);
        }
        collision.gameObject.SetActive(false);
    }
}
