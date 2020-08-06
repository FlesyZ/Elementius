using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public StatGeneral stat;
    public Animator anim { get; set; }
    public Rigidbody2D body { get; private set; }

    [Header("能力值")]
    [Tooltip("基礎生命值"), Range(0, 9999)]
    public int Health = 50;
    [Tooltip("屬性")]
    public Elements State = Elements.None;
    [Tooltip("力量"), Range(0, 99)]
    public int Strength = 3;
    [Tooltip("敏捷"), Range(0, 99)]
    public int Agility = 3;
    [Tooltip("智力"), Range(0, 99)]
    public int Intelligence = 3;
    [Tooltip("幸運"), Range(0, 99)]
    public int Luck = 3;
    [Tooltip("流動係數"), Range(0, 20)]
    public int Recovery = 0;


    [Header("獲得物品/經驗")]
    public float XP;
    public List<GameObject> itemGet;
    [Range(0f, 1f)]
    public float getChance = 0.5f;

    float moveX = 1, moveY = 0;
    Vector2 Movement { get { return new Vector2(moveX, moveY); } }
    Vector3 point = new Vector2(1, -1f);
    RaycastHit2D onGround;
    public bool isMoving { get { return !anim.GetBool("IsTakingDamage"); } }
    bool dead;

    public float Move { get { return anim.GetFloat("Move"); } }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        stat.MaxHP = Health;
        stat.data.hp = Health;
        stat.data.STR = Strength;
        stat.data.AGI = Agility;
        stat.data.INT = Intelligence;
        stat.data.LUK = Luck;
        stat.data.recovery = Recovery;
    }

    void Start()
    {
        Collider2D player = FindObjectOfType<Player>().GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(player, transform.GetComponent<Collider2D>());
    }

    void Update()
    {
        onGround = Physics2D.Raycast(transform.position, point, 1, 1 << 9);

        // auto rotation
        if (onGround.collider)
        {
            if (transform.rotation.y != 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.rotation.y != 180)
                transform.rotation = Quaternion.Euler(0, 180, 0);

            point = new Vector2(point.x * -1f, point.y);
            moveX *= -1f;
        }

        if (isMoving)
            body.velocity = Movement;
        else
            body.velocity = Vector2.zero;

        anim.SetFloat("Move", body.velocity.x);
        
        if (Move > 0)
            anim.SetInteger("Moving", 1);
        else if (Move < 0)
            anim.SetInteger("Moving", -1);
        else
            anim.SetInteger("Moving", 0);

        if (stat.HP <= 0 && !dead)
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

    public void StopMoving()
    {
        anim.SetBool("IsTakingDamage", true);
    }

    public void StartMoving()
    {
        if (stat.HP > 0)
            anim.SetBool("IsTakingDamage", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, point);
    }
}
