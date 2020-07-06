using UnityEngine;

public class Items : MonoBehaviour
{
    [Header("重生物件")]
    public Spawnable spawn;
    public GameObject spawnItem;

    [Header("重生時間"), Range(0, 20)]
    public float SpawnTime = 0f;

    public void ItemEffect(string item, Collider2D x)
    {
        if (x.tag == "Player")
        {
            switch (item)
            {
                case "Iridescent":
                    x.GetComponent<Player>().stat.ElementGet(Elements.Iridescent);
                    break;
                default:
                    break;
            }
        }
        
    }

    public void Respawn(float time)
    {
        if (SpawnTime == 0f)
        {
            spawn.RespawnObject = spawnItem;
            spawn.RespawnTime = time;
            Instantiate(spawn, gameObject.transform.position, Quaternion.identity);
        }
    }
}
