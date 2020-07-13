using UnityEngine;

public class AntiLoadDestruction : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
