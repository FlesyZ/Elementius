using System.Linq;
using UnityEngine;

public class Items : MonoBehaviour
{
    [Header("重生判斷")]
    public Spawner spawn;
    
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
                case "InitialElementGet":
                    x.GetComponent<Player>().stat.eSlots += 5;
                    break;
                default:
                    break;
            }
        }
        
    }

    public void Respawn(float time)
    {
        if (gameObject.name.Contains("(Clone)")) 
            gameObject.name = gameObject.name.Replace("(Clone)", "");
        spawn.spawning.Add(gameObject.name);
        spawn.spawnTime.Add(time); 
    }

    private void OnDestroy()
    {
        if (SpawnTime > 0f) Respawn(SpawnTime);
    }
}
