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
            item.maxHp = 20;
            item.hp = item.maxHp;
            item.ATK = 5;
            item.DEF = 5;
            item.INT = 3;
            item.AGI = 5;
            item.recovery = 7;
        }

        SceneManager.LoadScene("TestLevel");
    }

    void Update()
    {

    }
}
