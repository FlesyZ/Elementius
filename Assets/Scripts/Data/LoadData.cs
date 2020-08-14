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
            item.MaxHP = 50;
            item.data.hp = item.MaxHP;
            item.data.STR = 5;
            item.data.INT = 4;
            item.data.AGI = 5;
            item.data.LUK = 3;
            item.data.recovery = 7;
        }

        SceneManager.LoadScene("TestLevel");
    }

    void Update()
    {

    }
}
