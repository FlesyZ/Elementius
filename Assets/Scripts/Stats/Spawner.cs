using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
    private List<GameObject> spawn = new List<GameObject>();
    private List<Vector3> pos = new List<Vector3>();

    public List<string> spawning = new List<string>();
    public List<float> spawnTime = new List<float>();
    
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            pos.Add(transform.GetChild(i).position);
            spawn.Add(Resources.Load<GameObject>("Perfabs/" + transform.GetChild(i).gameObject.name));
        }
    }

    private void Update()
    {
        if (spawnTime.Count > 0)
        {
            bool[] doSpawn = new bool[spawnTime.Count];
            for (int i = 0; i < spawnTime.Count; i++)
            {
                spawnTime[i] -= Time.deltaTime;
                if (spawnTime[i] <= 0)
                {
                    Respawn(i);
                    doSpawn[i] = true;
                }
            }
            Refresh(doSpawn);
        }
    }

    private void Respawn(int index)
    {
        for (int x = 0; x < spawn.Count; x++)
        {
            if (spawn[x].name == spawning[index])
            {
                Instantiate(spawn[x], pos[x], Quaternion.identity, transform);
            }
        }
    }

    private void Refresh(bool[] cache)
    {
        for (int i = cache.Length - 1; i >= 0; i--)
        {
            if (cache[i])
            {
                spawning.RemoveAt(i);
                spawnTime.RemoveAt(i);
            }
        }
    }
}
