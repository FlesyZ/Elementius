using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverText : MonoBehaviour
{
    public Text hover;

    void Awake()
    {
        hover.canvasRenderer.SetAlpha(0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) hover.CrossFadeAlpha(1f, 0.5f, false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) hover.CrossFadeAlpha(0f, 0.5f, false);
    }
}
