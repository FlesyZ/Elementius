using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Camara : MonoBehaviour
{
    private List<GameObject> player = new List<GameObject>();
    private Camera cam;

    void Awake()
    {
        player.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Track();
    }

    private void Track()
    {
        List<Vector2> pos = new List<Vector2>();
        List<float> posX = new List<float>();
        
        foreach (var item in player)
        {
            pos.Add(item.transform.position);
        }
        for (int i = 0; i < pos.Count; i++)
        {
            posX.Add(pos[i].x);
        }

        Vector3 average = new Vector3(Mathf.Clamp(posX.Average(), -8f, 8f), 0f, -10f);
        cam.transform.position = Vector3.Lerp(cam.transform.position, average, 0.5f);
    }
}
