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
            StartCoroutine(other.GetComponent<Player>().stat.DamageDisplayer(Random.Range(5, 8).ToString(), other.transform, Elements.None, false));
            if (other.GetComponent<Player>().stat.HP > 0)
                other.transform.position = respawn;
            else
                other.GetComponent<Player>().Death();
        }
    }
}
