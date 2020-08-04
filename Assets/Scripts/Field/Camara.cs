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
        List<float> posY = new List<float>();

        int CanPlay = 0;

        foreach (var item in player)
        {
            pos.Add(item.transform.position);
            CanPlay++;
        }
        for (int i = 0; i < pos.Count; i++)
        {
            posX.Add(pos[i].x);
            posY.Add(pos[i].y);
        }

        Vector3 average = new Vector3(Mathf.Clamp(posX.Average(), -8f, 193f), Mathf.Clamp(posY.Average(), 0f, 2f), -10f);
        if (CanPlay == 0)
            cam.transform.position = Vector3.Lerp(cam.transform.position, average, Time.deltaTime * 0.5f);
        else
            cam.transform.position = Vector3.Lerp(cam.transform.position, average, 0.5f);
    }
}
