using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [Header("重生物件")]
    public GameObject RespawnObject;
    
    [Header("重生時間")]
    public float RespawnTime;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= RespawnTime)
        {
            Instantiate(RespawnObject, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
