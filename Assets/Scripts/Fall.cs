using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public Vector2 respawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().stat.Damage(Random.Range(5, 8), other.transform);
            if (other.GetComponent<Player>().stat.hp > 0)
                other.transform.position = respawn;
            else
                other.GetComponent<Player>().Death();
        }
    }
}
