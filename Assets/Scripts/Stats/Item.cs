﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Items
{
    private bool Float = false;
    private Rigidbody2D r;

    [Header("取得後觸發效果")]
    public string effect;

    public AudioSource aud;

    private IEnumerator Floating()
    {
        while (true)
        {
            Float = !Float;
            if (Float)
                r.AddForce(Vector2.up);
            else
                r.AddForce(Vector2.down);
            yield return new WaitForSecondsRealtime(5f);
        }
    }

    private void Awake()
    {
        r = GetComponent<Rigidbody2D>();
        if (SpawnTime > 0f) spawn = FindObjectOfType<Spawner>();
    }

    private void Start()
    {
        aud = FindObjectOfType<GameManager>().GetItem;
        StartCoroutine(Floating());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemEffect(effect, other);
        aud.Play();
        Destroy(gameObject);
    }
}
