using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    Vector2 move = Vector2.right;
    Vector3 point = new Vector2(1, -1f);
    RaycastHit2D onGround;
    bool dead;

    public float Move
    {
        get
        {
            return anim.GetFloat("Move");
        }
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        stat.maxHp = stat.hp;
    }

    void Start()
    {
        Collider2D player = FindObjectOfType<Player>().GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(player, transform.GetComponent<Collider2D>());
    }

    void Update()
    {
        onGround = Physics2D.Raycast(transform.position, point, 1, 1 << 9);

        if (onGround.collider)
        {
            if (transform.rotation.y != 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.rotation.y != 180)
                transform.rotation = Quaternion.Euler(0, 180, 0);

            point = new Vector2(point.x * -1f, point.y);
            move *= -1f;
        }
        
        body.velocity = move;
        anim.SetFloat("Move", body.velocity.x);
        
        if (Move > 0)
            anim.SetInteger("Moving", 1);
        else if (Move < 0)
            anim.SetInteger("Moving", -1);
        else
            anim.SetInteger("Moving", 0);

        if (stat.hp <= 0 && !dead)
            Death();
    }

    void Death()
    {
        anim.SetTrigger("TakeHit");
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

    public void Damage(int dmg, Elements e)
    {
        StartCoroutine(stat.Damage(dmg, transform, e));
        anim.SetTrigger("TakeHit");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, point);
    }
}
