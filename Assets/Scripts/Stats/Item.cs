using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Items
{
    private Vector2 pos;
    private bool Float = false;
    private Rigidbody2D r;

    [Header("取得後觸發效果")]
    public string effect;


    private IEnumerator Floating()
    {
        while (true)
        {
            Float = !Float;
            if (Float)
                r.AddForce(Vector2.up);
            else
                r.AddForce(Vector2.down);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Awake()
    {
        pos = transform.position;
        r = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(Floating());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemEffect(effect, other);
        Respawn(SpawnTime);
        gameObject.SetActive(false);
    }
}
