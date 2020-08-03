using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public StatGeneral stat;
    public Animator anim { get; set; }
    public Rigidbody2D body { get; private set; }

    [Header("獲得物品/經驗")]
    public float XP;
    public List<GameObject> itemGet;
    [Range(0f, 1f)]
    public float getChance = 0.5f;

    Vector2 move;
    RaycastHit2D onGround;
    bool dead;

    void Awake()
    {
        stat = GetComponent<StatGeneral>();

        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        onGround = Physics2D.Raycast(transform.position, (Vector2)transform.position + (Vector2.down + Vector2.right * 0.7f) * 1.2f);
        move = Vector2.right * 1f;
    }

    void Update()
    {
        body.velocity = move;

        if (stat.hp <= 0 && !dead)
            Death();
    }

    void Death()
    {
        anim.SetTrigger("Death");
        if (Random.Range(0f, 1f) < getChance)
        {
            foreach (var item in itemGet)
            {
                GameObject obj = Instantiate(item, transform.position, Quaternion.identity);
                
                Rigidbody2D body = obj.AddComponent<Rigidbody2D>();
                body.gravityScale = 0;

                Vector2 random = (Vector2.right * Random.Range(-1f, 1f) + Vector2.up * Random.Range(-1f, 1f)) * Random.Range(10, 1000);
                body.AddForce(random);
            }
        }
        Destroy(gameObject, 1f);
        dead = true;
    }

    private void OnTriggerExit2D(Collider2D that)
    {
        if (that.CompareTag("Floor") && onGround == gameObject.GetComponent<RaycastHit2D>())
        {
            if (transform.rotation.y == 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                onGround = Physics2D.Raycast(transform.position, (Vector2)transform.position + (Vector2.down + Vector2.left * 0.7f) * 1.2f);
                move = Vector2.left * 1f;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                onGround = Physics2D.Raycast(transform.position, (Vector2)transform.position + (Vector2.down + Vector2.right * 0.7f) * 1.2f);
                move = Vector2.right * 1f;
            }
        }
    }
}
