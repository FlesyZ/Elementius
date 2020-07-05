using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public Stats stat;

    /// <summary>
    /// 需要的元素數量
    /// </summary>
    private Elements e;
    private int count;

    private List<Elements> E;
    private List<int> needed;

    /// <summary>
    /// 引數中所需的元素有沒有連鎖需求
    /// </summary>
    private List<bool> isChained;

    /// <summary>
    /// 使用技能
    /// 
    /// </summary>
    /// <param name="skill">技能代號</param>
    public void UseSkill(string skill)
    {
        E = new List<Elements>();
        needed = new List<int>();
        isChained = new List<bool>();
        List<char> code = new List<char>();
        code.AddRange(skill.ToLower().ToCharArray());

        count = 0;

        // 元素代碼(文字): r, g, b, y, m, w, i, d
        // 連鎖代碼(整數): 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        for (int i = 0; i < code.Count; i++)
        {
            if (Extension.IsNumeric(code[i].ToString()))
                IsChain(i, Mathf.Clamp(i - 1, 0, i), code);
            else
                IsElement(i, Mathf.Clamp(i - 1, 0, i), code);

            if (i == code.Count - 1)
            {
                if (!Extension.IsNumeric(code[i].ToString()))
                {
                    E.Add(e);
                    isChained.Add(false);
                }
                needed.Add(count);
            }
        }
        
        for (int i = 0; i < E.Count; i++)
        {
            print("Detected: " + E[i] + " x " + needed[i] + (isChained[i] ? "(chained)" : ""));
        }
        
        var result = stat.ElementCondition(E, needed, isChained);
        if (result.Item2 == 0)
        {
            stat.ElementRemove(result.Item1);
            print("發動成功!");
        }
        else
        {
            print("發動失敗!");
            Debug.Log(result.Item2);
        }
    }

    private void IsElement(int i, int last, List<char> target)
    {
        // 元素代碼拆解
        if (i != 0 && target[i] != target[last])
        {
            if (!Extension.IsNumeric(target[last].ToString()))
            {
                E.Add(e);
                isChained.Add(false);
            }
            needed.Add(count);
            count = 0;
        }
        if (i == 0 || target[i] != target[last])
        {
            switch (target[i])
            {
                case 'r':
                    e = Elements.Brave;
                    break;
                case 'g':
                    e = Elements.Agile;
                    break;
                case 'b':
                    e = Elements.Guard;
                    break;
                case 'y':
                    e = Elements.Origin;
                    break;
                case 'm':
                    e = Elements.Earth;
                    break;
                case 'w':
                    e = Elements.Chaos;
                    break;
                case 'i':
                    e = Elements.Iridescent;
                    break;
                case 'd':
                    e = Elements.Dark;
                    break;
            }
        }   
        count++;
    }

    private void IsChain(int i, int last, List<char> target)
    {
        if (i != last)
            // 判斷放上數字之前一個元素 與其之前的元素是否相等
            if (last != 0 && target[last] == target[last - 1] && !Extension.IsNumeric(target[last].ToString()))
            {
                E.Add(e);
                needed.Add(count - 1);
                isChained.Add(false);
            }
        if (i == 0)
            e = Elements.None;
        E.Add(e);
        count = int.Parse(target[i].ToString());
        isChained.Add(true);
    }
}
