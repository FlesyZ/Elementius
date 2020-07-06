using UnityEngine;

public class Ground : MonoBehaviour
{
    private int inCollider;

    private float TimerCD;

    public bool State()
    {
        if (TimerCD > 0)
            return false;
        return inCollider > 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        inCollider++;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        inCollider--;
    }

    public void Disable(float time)
    {
        TimerCD = time;
    }

    void Update()
    {
        TimerCD -= Time.deltaTime;
    }
}
