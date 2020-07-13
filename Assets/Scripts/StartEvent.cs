using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : MonoBehaviour
{
    static Dialog dialog;

    public static void Ready()
    {
        dialog = GameObject.FindObjectOfType<Dialog>();
        dialog.Call(0.ToString(), true);
    }
}
