using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject trigger;
    public string TriggerName;
    public int index;

    public void TriggerMenu()
    {
        UI.MenuButton button = trigger.GetComponent<UI.MenuButton>();

        button.unlocked = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && trigger.GetComponent<UI.MenuButton>())
        {
            Invoke(TriggerName, 0f);
        }
    }
}
