using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatWithElement : StatGeneral
{
    private int e_Temp;

    [Header("元素值"), Range(0, 32)]
    public int elements;
    private List<Elements> eStored = new List<Elements>();
    public Elements eKeep { get; set; }

    private int eSlot;
    public int eSlots { get { return eSlot; } set { eSlot = value; } }

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        Health();
        eSlot = elements;
        if (HP > 0) Recovery();
    }

    protected override void FixedUpdate()
    {
        Element();
        base.FixedUpdate();
    }

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
            if (eStored.Count < count)
                count = eStored.Count;

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

    public void ElementAbsorption()
    {
        if (eStored.Count != 0)
        {
            eKeep = eStored[0];
            eStored.RemoveAt(0);
        }
    }

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
        UI.Element[] slots = gameObject.GetComponentsInChildren<UI.Element>();

        if (e_Temp != elements)
        {
            for (int n = 0; n < 32; n++)
            {
                if (eStored.Count != 0)
                {
                    if (slots[n].ID == n && slots[n].ID < eStored.Count)
                    {
                        slots[n].stored = eStored[n];
                    }
                    else if (slots[n].ID >= elements)
                    {
                        slots[n].stored = 0;
                    }
                }
                else if (slots[n].ID >= elements)
                {
                    slots[n].stored = 0;
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

    protected override void Recovery()
    {
        rElapse -= Time.deltaTime;
        rElapse = Mathf.Clamp(rElapse, 0, 50f / Recover);

        if (rElapse == 0)
        {
            int heal = (int)Mathf.Ceil(MaxHP * 0.01f * (INT / 10));
            if (data.hp < MaxHP)
            {
                StartCoroutine(RecoverDisplayer(heal.ToString(), gameObject.transform));
            }
            data.hp += heal;
            rElapse = 50f / Recover;
            if (eStored.Count < eSlots)
                eStored.Add((Elements)UnityEngine.Random.Range(1, 7));
        }
    }

    public void TakeDamage(StatWithElement attacker, StatGeneral defender)
    {
        float dmg;
        string damage;
        
        // [1] Brave >> Agile >> Guard >> Brave
        // [2] Origin <> Earth
        // [3] Dark >> Chaos >> (1)
        // [4] Iridescent >> (2)

        Elements a = attacker.eKeep;
        Elements d = (defender.GetComponent<Enemy>()) ? defender.GetComponent<Enemy>().State : defender.GetComponent<StatWithElement>().eKeep;

        short isStrongOrWeak = States.StrongOrWeakDetector(a, d);

        float atk = UnityEngine.Random.Range(attacker.STR * 0.99f, attacker.STR * 1.01f);
        float def = UnityEngine.Random.Range(defender.STR * 0.99f, defender.STR * 1.01f);

        if (isStrongOrWeak > 0)
            dmg = (atk + attacker.INT * 0.2f - def) + attacker.INT * 0.5f;
        else if (isStrongOrWeak < 0)
            dmg = (atk + attacker.INT * 0.2f - def) - defender.INT * 0.5f;
        else
            dmg = (a == 0) ? (atk - def) : (atk + attacker.INT * 0.2f - def);

        dmg = Mathf.Clamp(dmg, 0, dmg);
        bool isCrit = attacker.LUK + UnityEngine.Random.Range(attacker.LUK * -1f, attacker.LUK * 0.2f) > attacker.LUK;
        dmg = (isCrit && dmg > attacker.STR) ? (dmg * 2f) : dmg;
        dmg = (UnityEngine.Random.Range(0, defender.AGI) > UnityEngine.Random.Range(0, attacker.AGI)) ? -1 : dmg;

        damage = (dmg < 0) ? "miss" : ((int)dmg).ToString();

        if (damage == "miss")
            isCrit = false;
        else
            defender.GetComponent<Animator>().SetTrigger("TakeHit");

        if (dmg > attacker.STR) isCrit = false;
        
        StartCoroutine(defender.DamageDisplayer(damage, transform, a, isCrit));
    }
}
