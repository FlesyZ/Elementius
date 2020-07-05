using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private Image[] HP = new Image[3];
    private Image[] maxHP = new Image[3];

    public UI.Numbers nHp, nMax;

    private UI.Heart heart;

    [Header("血量"), Range(0, 9999)]
    public int hp = 20;

    private int maxH;
    public int maxHp { get { return maxH; } set { maxH = value; } }

    [Header("元素值"), Range(0, 32)]
    public int elements = 7;
    private List<Elements> eStored = new List<Elements>();

    private int eSlot;
    public int eSlots { get { return eSlot; } set { eSlot = value; } }

    [Header("流動係數"), Range(3f, 20f)]
    public float recovery = 5f;

    private float rElapse;

    private int hp_Temp, maxHp_Temp, hp_Stat, e_Temp;
    private int hp_units, hp_tens, hp_hundreds, hp_thousands, maxHp_units, maxHp_tens, maxHp_hundreds, maxHp_thousands;

    #region events
    private void Awake()
    {
        HP = nHp.GetComponentsInChildren<Image>();
        maxHP = nMax.GetComponentsInChildren<Image>();

        heart = GameObject.Find("Heart").GetComponent<UI.Heart>();
    }

    private void Start()
    {
        maxH = hp;

        hp_Temp = hp;
        maxHp_Temp = maxHp;

        eSlot = elements;

        rElapse = 50f / recovery;
    }

    private void FixedUpdate()
    {
        Health();
        Element();
        Recovery();
    }
    #endregion

    #region method
    #region Health
    /// <summary>
    /// 血量判別
    /// </summary>
    private void Health()
    {
        hp = Mathf.Clamp(hp, 0, maxHp);

        hp_units = hp_Temp % 10;
        hp_tens = hp_Temp / 10 % 10;
        hp_hundreds = hp_Temp / 100 % 10;
        hp_thousands = hp_Temp / 1000;

        int temp = Mathf.Abs(hp - hp_Temp);
        if (hp > hp_Temp)
        {
            if (temp > 256)
                hp_Temp += 168;
            else if (temp > 16)
                hp_Temp += 12;
            else
                hp_Temp++;
            HPdisplay();
        }
        else if (hp < hp_Temp)
        {
            if (temp > 256)
                hp_Temp -= 168;
            else if (temp > 16)
                hp_Temp -= 12;
            else
                hp_Temp--;
            HPdisplay();
        }
        
        HPdisplay(hp_Temp, hp_units, hp_tens, hp_hundreds, hp_thousands, HP, nHp);

        maxHp_units = maxHp_Temp % 10;
        maxHp_tens = maxHp_Temp / 10 % 10;
        maxHp_hundreds = maxHp_Temp / 100 % 10;
        maxHp_thousands = maxHp_Temp / 1000;

        if (maxHp > maxHp_Temp)
            maxHp_Temp++;
        else if (maxHp < maxHp_Temp)
            maxHp_Temp--;
        HPdisplay(maxHp_Temp, maxHp_units, maxHp_tens, maxHp_hundreds, maxHp_thousands, maxHP, nMax);
    }

    /// <summary>
    /// 血量更動時的動畫
    /// </summary>
    private void HPdisplay()
    {
        if (hp != hp_Temp)
            for (int i = 0; i < HP.Length; i++)
            {
                HP[i].transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-5.0f, 5.0f));
            }
        else
            for (int i = 0; i < HP.Length; i++)
            {
                HP[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        float hp_percentage = (float)hp / (float)maxHp;

        if (hp_percentage == 0f)
            hp_Stat = 0;
        else if (hp_percentage > 0f && hp_percentage <= 1f / 6f)
            hp_Stat = 1;
        else if (hp_percentage > 1f / 6f && hp_percentage <= 2f / 6f)
            hp_Stat = 2;
        else if (hp_percentage > 2f / 6f && hp_percentage <= 3f / 6f)
            hp_Stat = 3;
        else if (hp_percentage > 3f / 6f && hp_percentage <= 4f / 6f)
            hp_Stat = 4;
        else if (hp_percentage > 4f / 6f)
            hp_Stat = 5;

        heart.GetComponent<Image>().sprite = heart.heart[hp_Stat];
    }

    /// <summary>
    /// 血量顯示
    /// </summary>
    /// <param name="x">個位數</param>
    /// <param name="y">十位數</param>
    /// <param name="z">百位數</param>
    /// <param name="i">套用圖片</param>
    /// <param name="s">使用素材</param>
    private void HPdisplay(int temp, int x, int y, int z, int k, Image[] i, UI.Numbers s)
    {
        if (temp < 10000 && temp >= 0)
        {
            i[3].sprite = s.numbers[x];
            i[2].sprite = s.numbers[y];
            i[1].sprite = s.numbers[z];
            i[0].sprite = s.numbers[k];
        }
        else
            for (int n = 0; n < i.Length; n++)
            {
                i[n].sprite = s.unknown;
            }

        if (k == 0 && z == 0 && y == 0)
            i[2].color = new Color(i[2].color.r, i[2].color.g, i[2].color.b, 0);
        else
            i[2].color = new Color(i[2].color.r, i[2].color.g, i[2].color.b, 1);
        if (k == 0 && z == 0)
            i[1].color = new Color(i[1].color.r, i[1].color.g, i[1].color.b, 0);
        else
            i[1].color = new Color(i[1].color.r, i[1].color.g, i[1].color.b, 1);
        if (k == 0)
        {
            i[0].GetComponentInParent<GridLayoutGroup>().cellSize = new Vector2(52f, 96f);
            i[0].color = new Color(i[0].color.r, i[0].color.g, i[0].color.b, 0);
        }   
        else
        {
            i[0].GetComponentInParent<GridLayoutGroup>().cellSize = new Vector2(42f, 96f);
            i[0].color = new Color(i[0].color.r, i[0].color.g, i[0].color.b, 1);
        }   
    }
    #endregion

    #region Element
    /// <summary>
    /// 元素判別
    /// </summary>
    private void Element()
    {
        elements = Mathf.Clamp(eStored.Count, 0, 32);
        ElementDisplay();
    }

    #region Get Element
    /// <summary>
    /// 獲取元素
    /// </summary>
    /// <param name="count">獲取數量</param>
    public void ElementGet(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (eStored.Count < 32)
                eStored.Add((Elements)UnityEngine.Random.Range(1, 7));
        }
    }

    /// <summary>
    /// 獲取單顆元素
    /// </summary>
    public void ElementGet()
    {
        ElementGet(1);
    }

    /// <summary>
    /// 獲取指定元素
    /// </summary>
    /// <param name="count">獲取數量</param>
    /// <param name="type">指定元素</param>
    public void ElementGet(int count, Elements type)
    {
        for (int i = 0; i < count; i++)
        {
            if (eStored.Count < 32)
                eStored.Add(type);
        }
    }
    
    /// <summary>
    /// 獲取單顆指定元素
    /// </summary>
    /// <param name="type"></param>
    public void ElementGet(Elements type)
    {
        if (eStored.Count < 32)
            eStored.Add(type);
    }
    #endregion

    #region Lose Element
    /// <summary>
    /// 失去元素
    /// </summary>
    /// <param name="count">失去數量</param>
    public void ElementLoss(int count)
    {
        bool[] lost = new bool[eStored.Count];
        int n;

        if (eStored.Count != 0)
        {
            for (int i = 0; i < count; i++)
            {
                n = UnityEngine.Random.Range(0, eStored.Count);
                while (true)
                {
                    if (!lost[n])
                    {
                        lost[n] = true;
                        break;
                    }
                    else
                    {
                        n++;
                        if (n >= eStored.Count) n = 0;
                    }
                }
            }
            ElementRemove(lost);
        }
    }

    /// <summary>
    /// 失去單顆元素
    /// </summary>
    public void ElementLoss()
    {
        ElementLoss(1);
    }

    /// <summary>
    /// 失去指定元素
    /// </summary>
    /// <param name="count">失去數量</param>
    /// <param name="type">指定元素</param>
    public void ElementLoss(int count, Elements type)
    {
        bool[] lost = new bool[eStored.Count];
        List<int> index = new List<int>();
        int n;
        
        if (eStored.Count != 0)
        {
            for (int i = 0; i < eStored.Count; i++)
            {
                if (eStored[i] == type)
                {
                    index.Add(eStored.FindIndex(i, x => x == type));
                }
            }

            if (index.Count != 0)
            {
                if (count > index.Count) count = index.Count;
                bool[] remove = new bool[count];

                for (int i = 0; i < count; i++)
                {
                    n = UnityEngine.Random.Range(0, index.Count);
                    while (true)
                    {
                        if (!remove[n])
                        {
                            remove[n] = true;
                            break;
                        }
                        else
                        {
                            n++;
                            if (n >= index.Count) n = 0;
                        }
                    }
                }

                for (int i = 0; i < index.Count; i++)
                {
                    if (remove[i] == true) lost[index[i]] = true;
                }

                ElementRemove(lost);
            }
        }
    }

    /// <summary>
    /// 失去單顆指定元素
    /// </summary>
    /// <param name="type">指定元素</param>
    public void ElementLoss(Elements type)
    {
        bool[] lost = new bool[eStored.Count];
        List<int> index = new List<int>();
        
        if (eStored.Count != 0)
        {
            for (int i = 0; i < eStored.Count; i++)
            {
                if (eStored[i] == type)
                {
                    index.Add(eStored.FindIndex(i, x => x == type));
                }
            }

            if (index.Count != 0)
            {
                bool[] remove = new bool[index.Count];
                remove[UnityEngine.Random.Range(0, index.Count)] = true;

                for (int i = 0; i < index.Count; i++)
                {
                    if (remove[i] == true) lost[index[i]] = true;
                }

                ElementRemove(lost);
            }
        }
    }
    #endregion

    /// <summary>
    /// 元素消耗
    /// </summary>
    /// <param name="consume">元素消耗判斷</param>
    public void ElementRemove(bool[] consume)
    {
        for (int i = consume.Length - 1; i >= 0; i--)
        {
            if (consume[i]) eStored.RemoveAt(i);
        }
        ElementDisplay();
    }

    /// <summary>
    /// 元素顯示
    /// </summary>
    private void ElementDisplay()
    {
        UI.Element[] slots = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<UI.Element>();

        if (e_Temp != elements)
        {
            for (int i = 0; i < eStored.Count; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (slots[j].ID == i)
                    {
                        slots[j].stored = eStored[i];
                    }
                    else if (slots[j].ID >= elements)
                    {
                        slots[j].stored = 0;
                    }

                }
            }
            e_Temp = elements;
        }

        for (int i = 0; i < 32; i++)
        {
            slots[i].Base(eSlots);
        }
    }

    /// <summary>
    /// 元素條件判斷
    /// </summary>
    public Tuple<bool[], int> ElementCondition(List<Elements> E, List<int> count, List<bool> isChained)
    {
        bool[] condition = new bool[count.Count];

        UI.Element[] slots = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<UI.Element>();
        bool[] judged = new bool[eStored.Count];
        int[] chain = new int[eStored.Count];
        int chained = 0;

        // 抓取元素連鎖數
        for (int i = 0; i < eStored.Count; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if (slots[j].ID == i)
                {
                    chain[i] = slots[j].chained;
                }
            }
        }

        // 判斷需求元素有沒有與現有元素相符合
        // i = 需求索引, e = 持有索引
        for (int i = 0; i < count.Count; i++)
        {
            for (int e = 0; e < eStored.Count; e++)
            {
                if (isChained[i])
                    chained = chain[e];
                if (eStored[e] == E[i] && !judged[e])
                {
                    // 判斷是否連鎖 以及連鎖數是否相同
                    if (isChained[i] && chain[e] == count[i])
                    {
                        chained--;
                        judged[e] = true;
                    }
                    else if (!isChained[i])
                    {
                        count[i]--;
                        judged[e] = true;
                    }

                    if (count[i] == 0 || (isChained[i] && chained == 0))
                    {
                        condition[i] = true;
                        break;
                    }
                }
            }
        }

        // 判斷所需元素條件是否完全符合
        int skill = condition.Length;
        foreach (var item in condition)
        {
            if (item) skill--;
        }


        // 回傳是否成功 (skill == 0)
        return new Tuple<bool[], int>(judged, skill);
    }
    #endregion

    private void Recovery()
    {
        rElapse -= Time.deltaTime;
        rElapse = Mathf.Clamp(rElapse, 0, 50f / recovery);

        if (rElapse == 0)
        {
            hp += (int)((float)maxHp * 0.01f);
            if (eStored.Count < eSlots)
                eStored.Add((Elements)UnityEngine.Random.Range(1, 7));
            rElapse = 50f / recovery;
        }
    }
    #endregion
}
