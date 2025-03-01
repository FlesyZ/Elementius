﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public StatGeneral stat;
    public Animator anim { get; set; }
    public Rigidbody2D body { get; private set; }

    AudioSource bone, swing;

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
    RaycastHit2D onGround { get { return Physics2D.Raycast(transform.position + transform.right, transform.up * -1f, 1, 1 << 9); } }

    Player[] players;
    List<RaycastHit2D> RayToPlayers = new List<RaycastHit2D>();

    public float Move { get { return anim.GetFloat("Move"); } }
    public bool isMoving { get { return !(anim.GetBool("IsTakingDamage") || anim.GetBool("IsAttacking") || getFreezed); } }
    public bool getFreezed;

    AttackRange attack;
    public int AttackOrder { get; private set; } = 0;
    
    public float defendTime;

    byte toward = 0;
    bool dead;

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

        bone = gameObject.AddComponent<AudioSource>();
        swing = gameObject.AddComponent<AudioSource>();

        bone.clip = Resources.Load<AudioClip>("Sounds/Bones");
        swing.clip = Resources.Load<AudioClip>("Sounds/Sword_Swing");
    }

    void Start()
    {
        players = FindObjectsOfType<Player>();

        Collider2D player = FindObjectOfType<Player>().GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(player, GetComponent<Collider2D>());

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (var item in enemies)
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), item.GetComponent<Collider2D>());

        attack = GetComponentInChildren<AttackRange>();
    }

    void Update()
    {
        DetectPlayers();

        // auto rotation
        transform.rotation = Quaternion.Euler(0, Toward(), 0);

        if (isMoving)
        {
            body.velocity = Movement;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
            body.constraints = RigidbodyConstraints2D.FreezeAll;

        anim.SetFloat("Move", body.velocity.x);

        foreach (var item in RayToPlayers)
        {
            if (item.distance < 1.2f && !anim.GetBool("IsAttacking"))
            {
                anim.SetBool("IsAttacking", true);
                StartCoroutine(DoAttack());
            }
            else if (item.distance >= 1.2f)
            {
                anim.SetBool("IsAttacking", false);
            }
        }

        if (Move > 0)
            anim.SetInteger("Moving", 1);
        else if (Move < 0)
            anim.SetInteger("Moving", -1);
        else
            anim.SetInteger("Moving", 0);

        if (stat.HP <= 0 && !dead)
            Death();
    }

    float Toward()
    {   
        byte t = toward;

        if (!onGround.collider)
        {
            t = (byte)(++t % 2);
        }
        else
        {
            float? DistanceDetect = null;
            for (int i = 0; i < players.Length; i++)
            {
                if (anim.GetBool("IsAttacking") && (DistanceDetect < RayToPlayers[i].distance || DistanceDetect == null))
                {
                    t = (byte)((transform.position.x - players[i].transform.position.x > 0) ? 1 : 0);
                    DistanceDetect = RayToPlayers[i].distance;
                }
            }
        }

        if (toward != t)
        {
            moveX *= -1f;
            toward = t;
        }

        return toward * 180f;
    }

    void DetectPlayers()
    {
        if (RayToPlayers.Count > 0) RayToPlayers.Clear();

        if (players.Length > 0)
        {
            foreach (var item in players)
                RayToPlayers.Add(Physics2D.Linecast(transform.position, item.transform.position, 1 << 10));
        }
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

    IEnumerator DoAttack()
    {
        WaitForSeconds wait = new WaitForSeconds(0.7f);
        while (anim.GetBool("IsAttacking"))
        {
            anim.SetInteger("Attack_const", AttackOrder);
            anim.SetTrigger("Attack");
            AttackOrder++;
            AttackOrder %= 2;
            yield return wait;
        }
    }

    public void Attack()
    {
        swing.PlayDelayed(0.2f);
        attack.enabled = true;
    }

    public void Defend()
    {
        StartCoroutine(Defending(defendTime));
    }

    public void TakeHit()
    {
        anim.SetTrigger("TakeHit");
        bone.Play();
    }

    IEnumerator Defending(float value)
    {
        WaitForSecondsRealtime defend = new WaitForSecondsRealtime(0)
        {
            waitTime = value
        };

        yield return defend;

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
        Gizmos.DrawRay(transform.position + transform.right, transform.up * -1f);
        foreach (var item in players)
            Gizmos.DrawLine(transform.position, item.transform.position);
    }
}
