using UnityEngine;

public class AttackRange : Attack
{
    public int range;   // 0 if it is in a short range;

    public Collider2D[] collision;

    int? order;
    float timer;

    private void OnEnable()
    {
        if (GetComponentInParent<Enemy>())
        {
            order = GetComponentInParent<Enemy>().AttackOrder;
        }

        if (order != null && collision.Length > 1)
        {
            collision[(int)order].enabled = true;
        }
        else if (order != null && collision.Length == 1)
        {
            collision[0].enabled = true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.2f)
        {
            timer = 0f;

            if (collision.Length > 0)
                for (int i = 0; i < collision.Length; i++)
                    collision[i].enabled = false;
            else
                GetComponent<Collider2D>().enabled = false;
            
            this.enabled = false;
        }
    }
}