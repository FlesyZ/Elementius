using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    private StatWithElement[] stats;
    void Start()
    {
        //StartEvent.Ready();
        stats = FindObjectsOfType<StatWithElement>();

        foreach (var item in stats)
        {
            item.MaxHP = 200;
            item.data.hp = item.MaxHP;
            item.data.STR = 50;
            item.data.INT = 30;
            item.data.AGI = 25;
            item.data.LUK = 50;
            item.data.recovery = 7;
        }

        SceneManager.LoadScene("TestLevel");
    }

    void Update()
    {

    }
}
