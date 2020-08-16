using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StatWithElement stat;
    UI.GameMenu menu;

    public bool play = false;

    private float Speed = 3.0f;
    private float Jump = 2.5f;

    private Animator a;
    public Animator anim { get { return a; } set { a = value; } }

    AttackRange attack;

    private Rigidbody2D r;


    private bool isGrounded = false;
    protected RaycastHit2D grounded
    {
        get
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 0.5f, 1 << 9);
        }
    }


    private List<string> Action = new List<string>();
    private List<float> ActionTimer = new List<float>();
    private float Dash;
    
    private bool Absorbed;
    private float AbsorbTimer;

    private ParticleSystem particle;

    #region events
    private void Start()
    {
        stat = FindObjectOfType<StatWithElement>();

        a = GetComponent<Animator>();
        r = GetComponent<Rigidbody2D>();

        attack = GetComponentInChildren<AttackRange>();

        particle = FindObjectOfType<ParticleSystem>();
        particle.gameObject.SetActive(false);

        menu = FindObjectOfType<UI.GameMenu>();
    }

    private void Update()
    {
        if (play && !menu.menu)
            Move();
        else if (stat.HP == 0)
            Death();

        if (stat.eKeep != Elements.None && particle.gameObject.activeSelf == false)
        {
            particle.gameObject.SetActive(true);
        }
        else if (stat.eKeep == Elements.None && particle.gameObject.activeSelf == true)
        {
            particle.gameObject.SetActive(false);
        }

        if (stat.eKeep != Elements.None && particle.gameObject.activeSelf == true)
        {
            Color colour = Color.white;
            switch (stat.eKeep)
            {
                case Elements.Brave:
                    colour = Color.red;
                    break;
                case Elements.Agile:
                    colour = Color.green;
                    break;
                case Elements.Guard:
                    colour = Color.blue;
                    colour.g = 0.2f;
                    break;
                case Elements.Origin:
                    colour.b = 0f;
                    break;
                case Elements.Earth:
                    colour.g = 0f;
                    break;
                case Elements.Chaos:
                    colour = Color.white;
                    break;
                case Elements.Iridescent:
                    colour.r = UnityEngine.Random.Range(0.5f, 1f);
                    colour.g = UnityEngine.Random.Range(0.5f, 1f);
                    colour.b = UnityEngine.Random.Range(0.5f, 1f);
                    break;
                case Elements.Dark:
                    colour.r = UnityEngine.Random.Range(0f, 0.1f);
                    colour.g = UnityEngine.Random.Range(0f, 0.1f);
                    colour.b = UnityEngine.Random.Range(0f, 0.1f);
                    break;
            }
            var main = particle.main;
            main.startColor = colour;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !menu.menu)
        {
            menu.menu = true;
        }
    }
    #endregion

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

    public void Attack()
    {
        attack.enabled = true;
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
        // 偵測動作狀態
        var action = InAction(Action, ActionTimer);
        Action = action.Item1;
        ActionTimer = action.Item2;

        // 踩在地上的偵測
        if (grounded.collider != null)
        {
            if (!isGrounded && grounded.collider.gameObject.CompareTag("Floor"))
            {
                isGrounded = true;
                a.SetBool("Grounded", isGrounded);
            }

            if (isGrounded && !grounded.collider.gameObject.CompareTag("Floor"))
            {
                isGrounded = false;
                a.SetBool("Grounded", isGrounded);
            }
        }

        float X, x;

        X = Input.GetAxis("Horizontal");
        x = Input.GetAxisRaw("Horizontal");

        // 轉向
        if (X > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (X < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);

        // 移動
        if (Action.Contains("Dash"))
            r.velocity = new Vector2(Mathf.Clamp(X * Speed, Dash, Dash), r.velocity.y);
        else if (X != 0)
            r.velocity = Vector2.Lerp(r.velocity, new Vector2(X * Speed, r.velocity.y), Time.deltaTime * 10f);
        else if (Action.Contains("Attack") || Action.Contains("TakeHit"))
            r.velocity = new Vector2(0f, r.velocity.y);
        else if (x == 0)
            r.velocity = new Vector2(0f, r.velocity.y);

        // 角色動作
        float move;
        if (Action.Contains("Dash"))
            move = Mathf.Clamp(Mathf.Abs(r.velocity.x), 3f, 3f);
        else if (Mathf.Abs(x) == 1)
            move = Mathf.Clamp(Mathf.Abs(r.velocity.x), 0.1f, Mathf.Abs(r.velocity.x));
        else
            move = r.velocity.x;

        if (!Action.Contains("Attack") && !Action.Contains("TakeHit"))
            a.SetFloat("Move", move);
        
        a.SetFloat("Air", r.velocity.y);

        stat.Resting = X == 0;

        // 按鍵處理 - 角色動作
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            a.SetTrigger("Jump");
            isGrounded = false;
            a.SetBool("Grounded", isGrounded);
            r.velocity = new Vector2(r.velocity.x, Jump * Speed);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isGrounded && x == 0 && !Action.Contains("Attack"))
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

    public void TakeHit()
    {
        Action.Add("TakeHit");
        ActionTimer.Add(0.5f);
    }
}
