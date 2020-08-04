using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StatWithElement stat;

    public bool play = false;

    private float Speed = 3.0f;
    private float Jump = 2.5f;

    private Animator a;
    public Animator anim { get { return a; } set { a = value; } }

    public Collider2D attack { get; set; }

    private Rigidbody2D r;
    private Ground ground;
    private bool isGrounded = false;
    private List<string> Action = new List<string>();
    private List<float> ActionTimer = new List<float>();
    private float Dash;
    private float AbsorbTimer;
    private bool Absorbed;

    private Tuple<List<string>, List<float>> InAction(List<string> a, List<float> timer)
    {
        bool[] remove = new bool[timer.Count];

        for (int i = 0; i < timer.Count; i++)
        {
            timer[i] -= Time.deltaTime;
            
            if (timer[i] <= 0f)
            {
                remove[i] = true;
            }
        }

        for (int i = remove.Length - 1; i >= 0; i--)
        {
            if (remove[i])
            {
                a.RemoveAt(i);
                timer.RemoveAt(i);
            }
        }
        return new Tuple<List<string>, List<float>>(a, timer);
    }

    public void Death()
    {
        stat.ElementLoss(3);
        a.SetTrigger("Death");
        play = false;
        r.velocity = new Vector2(0f, 0f);
    }

    private void Move()
    {
        // detect 
        var action = InAction(Action, ActionTimer);
        Action = action.Item1;
        ActionTimer = action.Item2;

        if (!isGrounded && ground.State())
        {
            isGrounded = true;
            a.SetBool("Grounded", isGrounded);
        }

        if (isGrounded && !ground.State())
        {
            isGrounded = false;
            a.SetBool("Grounded", isGrounded);
        }

        float X = Input.GetAxis("Horizontal");
        float x = Input.GetAxisRaw("Horizontal");
        if (X > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (X < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);

        if (Action.Contains("Dash"))
            r.velocity = new Vector2(Mathf.Clamp(X * Speed, Dash, Dash), r.velocity.y);
        else if (X != 0)
            r.velocity = Vector2.Lerp(r.velocity, new Vector2(X * Speed, r.velocity.y), Time.deltaTime * 10f);
        else if (x == 0)
            r.velocity = new Vector2(0f, r.velocity.y);
        else if (Action.Contains("Attack") || Action.Contains("Damage"))
            r.velocity = new Vector2(0f, r.velocity.y);

        float move;
        if (Action.Contains("Dash"))
            move = Mathf.Clamp(Mathf.Abs(r.velocity.x), 3f, 3f);
        else if (Mathf.Abs(x) == 1)
            move = Mathf.Clamp(Mathf.Abs(r.velocity.x), 0.1f, Mathf.Abs(r.velocity.x));
        else
            move = r.velocity.x;
        if (!Action.Contains("Attack") && !Action.Contains("Damage"))
            a.SetFloat("Move", move);
        a.SetFloat("Air", r.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            a.SetTrigger("Jump");
            isGrounded = false;
            a.SetBool("Grounded", isGrounded);
            r.velocity = new Vector2(r.velocity.x, Jump * Speed);
            ground.Disable(0.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isGrounded && !Action.Contains("Attack"))
        {
            a.SetTrigger("Attack");
            Action.Add("Attack");
            ActionTimer.Add(1.25f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && !Action.Contains("Dash") && X != 0)
        {
            a.SetTrigger("Dash");
            Action.Add("Dash");
            ActionTimer.Add(1f);
            Dash = x * Speed * 2f;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && X == 0 && !Absorbed)
        {
            AbsorbTimer += Time.deltaTime;
            if (AbsorbTimer >= 0.5f)
            {
                stat.ElementAbsorption();
                Absorbed = true;
            }
        }

        if (Absorbed || !Input.GetKey(KeyCode.LeftShift) && AbsorbTimer > 0)
        {
            AbsorbTimer -= Time.deltaTime;
            if (AbsorbTimer <= 0)
            {
                AbsorbTimer = 0;
                Absorbed = false;
            }
        }
    }

    public void Attack()
    {
        attack.enabled = true;
    }

    #region events
    private void Start()
    {
        stat = FindObjectOfType<StatWithElement>();
        ground = FindObjectOfType<Ground>();

        a = GetComponent<Animator>();
        r = GetComponent<Rigidbody2D>();

        attack = GetComponentInChildren<AttackRange>().GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (play)
            Move();
        else if (stat.hp == 0)
            Death();
    }
    #endregion
}
