using Game;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class StatGeneral : MonoBehaviour
{
    private Image[] hp = new Image[3];
    private Image[] hpM = new Image[3];

    public UI.Numbers nHp, nMax;

    private UI.Heart heart;

    public Game.GameState data;

    public int HP { get { return Mathf.Clamp(data.hp, 0, MaxHP); } }

    private int maxH;
    public int MaxHP { get { return maxH; } set { maxH = value; } }

    public bool isShown;

    public float Recover { get { return data.recovery; } }
    public bool Resting { protected get; set; }

    public float rElapse { get; protected set; }

    public bool Guarding { get; set; }

    public int STR { get { return Mathf.Clamp(data.STR, 0, data.STR); } }
    public int AGI { get { return Mathf.Clamp(data.AGI, 0, data.AGI); } }
    public int INT { get { return Mathf.Clamp(data.INT, 0, data.INT); } }
    public int LUK { get { return Mathf.Clamp(data.LUK, 0, data.LUK); } }

    private int hp_Temp, maxHp_Temp, hp_Stat;
    private int hp_units, hp_tens, hp_hundreds, maxHp_units, maxHp_tens, maxHp_hundreds;

    #region events
    protected virtual void Awake()
    {
        
        if (nHp != null) hp = nHp.GetComponentsInChildren<Image>();
        if (nMax != null) hpM = nMax.GetComponentsInChildren<Image>();

        heart = GameObject.Find("Heart").GetComponent<UI.Heart>();
    }

    protected virtual void Start()
    {
        hp_Temp = HP;
        maxHp_Temp = MaxHP;

        rElapse = 50f / Recover;
    }

    protected virtual void FixedUpdate()
    {
        Health();
        if (HP > 0) Recovery();
    }
    #endregion

    #region method
    #region Health
    /// <summary>
    /// 血量判別
    /// </summary>
    protected void Health()
    {
        hp_units = hp_Temp % 10;
        hp_tens = hp_Temp / 10 % 10;
        hp_hundreds = hp_Temp / 100;

        int temp = Mathf.Abs(HP - hp_Temp);
        if (HP > hp_Temp)
        {
            if (temp > 256)
                hp_Temp += 168;
            else if (temp > 16)
                hp_Temp += 12;
            else
                hp_Temp++;
            if (nHp != null)
                HPdisplay();
        }
        else if (HP < hp_Temp)
        {
            if (temp > 256)
                hp_Temp -= 168;
            else if (temp > 16)
                hp_Temp -= 12;
            else
                hp_Temp--;
        }

        data.hp = Mathf.Clamp(data.hp, 0, MaxHP);

        maxHp_units = maxHp_Temp % 10;
        maxHp_tens = maxHp_Temp / 10 % 10;
        maxHp_hundreds = maxHp_Temp / 100;

        if (MaxHP > maxHp_Temp)
            maxHp_Temp++;
        else if (MaxHP < maxHp_Temp)
            maxHp_Temp--;

        if (nHp != null && nMax != null)
        {
            HPdisplay(hp_Temp, hp_units, hp_tens, hp_hundreds, hp, nHp);
            HPdisplay(maxHp_Temp, maxHp_units, maxHp_tens, maxHp_hundreds, hpM, nMax);

            HPdisplay();
        }
    }

    /// <summary>
    /// 血量更動時的動畫
    /// </summary>
    private void HPdisplay()
    {
        if (HP != hp_Temp || hp_Stat <= 1 || HP <= 3 && HP >= 1)
            for (int i = 0; i < hp.Length; i++)
            {
                hp[i].transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-5.0f, 5.0f));
            }
        else
            for (int i = 0; i < hp.Length; i++)
            {
                hp[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        float hp_percentage = (float)HP / (float)MaxHP;

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
    private void HPdisplay(int temp, int x, int y, int z, Image[] i, UI.Numbers s)
    {
        if (temp < 1000 && temp >= 0)
        {
            i[2].sprite = s.numbers[x];
            i[1].sprite = s.numbers[y];
            i[0].sprite = s.numbers[z];
        }
        else
            for (int n = 0; n < i.Length; n++)
            {
                i[n].sprite = s.unknown;
            }

        if (z == 0 && y == 0)
            i[1].color = new Color(i[1].color.r, i[1].color.g, i[1].color.b, 0);
        else
            i[1].color = new Color(i[1].color.r, i[1].color.g, i[1].color.b, 1);
        if (z == 0)
        {
            i[0].GetComponentInParent<GridLayoutGroup>().cellSize = new Vector2(52f, 96f);
            i[0].GetComponentInParent<GridLayoutGroup>().spacing = new Vector2(0f, 0f);
            i[0].color = new Color(i[0].color.r, i[0].color.g, i[0].color.b, 0);
        }
        else
        {
            i[0].GetComponentInParent<GridLayoutGroup>().cellSize = new Vector2(39f, 72f);
            i[0].GetComponentInParent<GridLayoutGroup>().spacing = new Vector2(-4f, 0f);
            i[0].color = new Color(i[0].color.r, i[0].color.g, i[0].color.b, 1);
        }
    }
    #endregion

    protected virtual void Recovery()
    {
        if (Resting)
            rElapse -= Time.deltaTime;
        else
            rElapse = Mathf.Infinity;

        rElapse = Mathf.Clamp(rElapse, 0, 50f / Recover);

        if (rElapse == 0)
        {
            int heal = (int)Mathf.Ceil(MaxHP * 0.01f * Mathf.Ceil(INT / 10));
            if (HP < MaxHP)
            {
                if (HP + heal > MaxHP) heal = MaxHP - HP;
                data.hp += heal;
                StartCoroutine(RecoverDisplayer("" + heal, gameObject.transform));
            }

            rElapse = 50f / Recover;
        }
    }

    public IEnumerator DamageDisplayer(string dmg, Transform trans, Elements e, bool isCrit)
    {
        if (dmg.IsNumeric())
        {
            data.hp -= int.Parse(dmg);
        }
        
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.5f);
        Vector2 force = new Vector2(Random.Range(-1f, 1f), 2.5f) * 100f;

        if (isCrit)
            StartCoroutine(CritDisplayer(trans, force));

        GameObject Damage = new GameObject("Damage");
        Damage.transform.position = (Vector2)trans.position + Vector2.up;

        TextMesh damage = Damage.AddComponent<TextMesh>();
        damage.text = dmg;
        damage.characterSize = 0.25f;
        damage.font = Resources.Load("04B_03__") as Font; 
        damage.GetComponent<MeshRenderer>().material = Resources.Load("04B_03__") as Material;
        damage.fontSize = 16;

        Color color = Color.white;
        if (dmg.IsNumeric())
        {
            switch (e)
            {
                case Elements.Brave:
                    color = Color.red;
                    break;
                case Elements.Agile:
                    color = Color.green;
                    break;
                case Elements.Guard:
                    color = Color.blue;
                    color.g = 0.5f;
                    break;
                case Elements.Origin:
                    color = Color.yellow;
                    break;
                case Elements.Earth:
                    color.g = 0;
                    break;
                case Elements.Chaos:
                    color.r = 0.8f;
                    color.g = 0.8f;
                    color.b = 0.8f;
                    break;
                case Elements.Iridescent:
                    color = Color.cyan;
                    break;
                case Elements.Dark:
                    color = Color.gray;
                    break;
                default:
                    break;
            }
        }
        else
        {
            color.r = 0.64f;
            color.g = 0f;
        }
        damage.color = color;

        Renderer renderer = damage.GetComponent<Renderer>();
        renderer.sortingOrder = 20;

        Rigidbody2D body = Damage.AddComponent<Rigidbody2D>();
        body.AddForce(force);

        Destroy(Damage, 1.5f);
        yield return wait;

        wait.waitTime = 0.05f;
        Color fade = new Color(damage.color.r, damage.color.g, damage.color.b, 1);
        while (damage.color.a != 0)
        {
            fade.a -= 0.1f;
            damage.color = fade;
            yield return wait;
        }
    }

    public IEnumerator RecoverDisplayer(string value, Transform trans)
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.5f);
        Vector2 force = new Vector2(Random.Range(-1f, 1f), 2.5f) * 100f;

        GameObject Heal = new GameObject("Recover");
        Heal.transform.position = (Vector2)trans.position + Vector2.up;

        TextMesh heal = Heal.AddComponent<TextMesh>();
        heal.text = value;
        heal.characterSize = 0.25f;
        heal.font = Resources.Load("04B_03__") as Font;
        heal.GetComponent<MeshRenderer>().material = Resources.Load("04B_03__") as Material;
        heal.fontSize = 16;

        heal.color = new Color(0, 0.6f, 0, 1);

        Renderer renderer = heal.GetComponent<Renderer>();
        renderer.sortingOrder = 20;

        Rigidbody2D body = Heal.AddComponent<Rigidbody2D>();
        body.isKinematic = true;
        body.AddForce(force);

        Destroy(Heal, 1.5f);
        yield return wait;

        wait.waitTime = 0.05f;
        Color fade = new Color(heal.color.r, heal.color.g, heal.color.b, 1);
        while (heal.color.a != 0)
        {
            fade.a -= 0.1f;
            heal.color = fade;
            yield return wait;
        }
    }

    public IEnumerator CritDisplayer(Transform trans, Vector2 force)
    {
        yield return new WaitForSeconds(0.1f);

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.5f);
        GameObject Crit = new GameObject("Critical!");
        Crit.transform.position = (Vector2)trans.position + Vector2.up;

        TextMesh crit = Crit.AddComponent<TextMesh>();
        crit.text = "critical!";
        crit.characterSize = 0.25f;
        crit.font = Resources.Load("04B_03__") as Font;
        crit.GetComponent<MeshRenderer>().material = Resources.Load("04B_03__") as Material;
        crit.fontSize = 12;
        crit.color = new Color(1, 0.5f, 0, 1);

        Renderer renderer = crit.GetComponent<Renderer>();
        renderer.sortingOrder = 20;

        Rigidbody2D body = Crit.AddComponent<Rigidbody2D>();
        body.AddForce(force);

        Destroy(Crit, 1.5f);
        yield return wait;

        wait.waitTime = 0.05f;
        Color fade = new Color(crit.color.r, crit.color.g, crit.color.b, 1);
        while (crit.color.a != 0)
        {
            fade.a -= 0.1f;
            crit.color = fade;
            crit.color = fade;
            yield return wait;
        }

    }

    /// <summary>
    /// 玩家受到傷害
    /// </summary>
    /// <param name="attacker">攻擊方</param>
    /// <param name="defender">防禦方</param>
    /// <param name="player">防禦對象</param>
    public void TakeDamage(Enemy enemy, Player player)
    {
        StatGeneral attacker = enemy.stat;
        StatGeneral defender = player.stat;

        float dmg;
        string damage;

        Elements a = enemy.State;
        Elements d = player.stat.eKeep;

        short isStrongOrWeak = GameState.StrongOrWeakDetector(a, d);

        float atk = GameState.ValueWithElements(a, attacker, true);
        float def = GameState.ValueWithElements(a, defender, !Guarding);

        if (isStrongOrWeak > 0)
            dmg = (atk + attacker.INT * 0.2f - def) + attacker.INT * 0.5f;
        else if (isStrongOrWeak < 0)
            dmg = (atk + attacker.INT * 0.2f - def) - defender.INT * 0.5f;
        else
            dmg = (a == 0) ? (atk - def) : (atk + attacker.INT * 0.2f - def);

        dmg = Mathf.Clamp(dmg, 0, dmg);
        bool isCrit = attacker.LUK + Random.Range(attacker.LUK * -1f, attacker.LUK * 0.2f) > attacker.LUK;
        dmg = (isCrit && dmg > attacker.STR) ? (dmg * 2f) : dmg;
        dmg = (Random.Range(0, defender.AGI) > Random.Range(0, attacker.AGI)) ? -1 : dmg;

        damage = (dmg < 0) ? "miss" : ((int)dmg).ToString();

        if (damage == "miss")
            isCrit = false;
        else
            player.GetComponent<Animator>().SetTrigger("TakeHit");

        if (dmg > attacker.STR) isCrit = false;

        StartCoroutine(defender.DamageDisplayer(damage, transform, a, isCrit));
    }

    /// <summary>
    /// 非玩家受到傷害
    /// </summary>
    /// <param name="attacker">攻擊方</param>
    /// <param name="defender">防禦方</param>
    public void TakeDamage(Player player, Enemy enemy)
    {
        StatGeneral attacker = player.stat;
        StatGeneral defender = enemy.stat;

        float dmg;
        string damage;

        Elements a = player.stat.eKeep;
        Elements d = enemy.State;

        short isStrongOrWeak = GameState.StrongOrWeakDetector(a, d);

        float atk = GameState.ValueWithElements(a, attacker, true);
        float def = GameState.ValueWithElements(a, defender, !Guarding);

        if (isStrongOrWeak > 0)
            dmg = (atk + attacker.INT * 0.2f - def) + attacker.INT * 0.5f;
        else if (isStrongOrWeak < 0)
            dmg = (atk + attacker.INT * 0.2f - def) - defender.INT * 0.5f;
        else
            dmg = (a == 0) ? (atk - def) : (atk + attacker.INT * 0.2f - def);

        dmg = Mathf.Clamp(dmg, 0, dmg);
        bool isCrit = attacker.LUK + Random.Range(attacker.LUK * -1f, attacker.LUK * 0.2f) > attacker.LUK;
        dmg = (isCrit && dmg > attacker.STR) ? (dmg * 2f) : dmg;
        dmg = (Random.Range(0, defender.AGI) > Random.Range(0, attacker.AGI)) ? -1 : dmg;

        damage = (dmg < 0) ? "miss" : ((int)dmg).ToString();

        if (damage == "miss")
            isCrit = false;
        else
            defender.GetComponent<Animator>().SetTrigger("TakeHit");

        if (dmg > attacker.STR) isCrit = false;

        StartCoroutine(defender.DamageDisplayer(damage, transform, a, isCrit));
    }

    public void ToggleChat()
    {
        isShown = !isShown;
    }
    #endregion
}
