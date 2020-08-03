using UnityEngine;
using UnityEngine.UI;

public class StatGeneral : MonoBehaviour
{
    private Image[] HP = new Image[3];
    private Image[] maxHP = new Image[3];

    public UI.Numbers nHp, nMax;

    private UI.Heart heart;

    [Header("血量"), Range(0, 9999)]
    public int hp;

    private int maxH;
    public int maxHp { get { return maxH; } set { maxH = value; } }

    public bool isShown;

    [Header("流動係數"), Range(3f, 20f)]
    public float recovery;

    public float rElapse { get; private set; }

    [Header("能力值")]
    [Tooltip("攻擊"), Range(1, 99)]
    public int ATK;
    [Tooltip("防禦"), Range(1, 99)]
    public int DEF;
    [Tooltip("精神"), Range(1, 99)]
    public int INT;
    [Tooltip("敏捷"), Range(1, 99)]
    public int AGI;

    private int hp_Temp, maxHp_Temp, hp_Stat;
    private int hp_units, hp_tens, hp_hundreds, maxHp_units, maxHp_tens, maxHp_hundreds;

    #region events
    void Awake()
    {
        if (nHp != null) HP = nHp.GetComponentsInChildren<Image>();
        if (nMax != null) maxHP = nMax.GetComponentsInChildren<Image>();

        heart = GameObject.Find("Heart").GetComponent<UI.Heart>();
    }

    protected virtual void Start()
    {
        hp_Temp = hp;
        maxHp_Temp = maxHp;

        rElapse = 50f / recovery;
    }

    protected virtual void FixedUpdate()
    {
        Health();
        if (hp > 0) Recovery();
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
        hp_hundreds = hp_Temp / 100;

        int temp = Mathf.Abs(hp - hp_Temp);
        if (hp > hp_Temp)
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
        else if (hp < hp_Temp)
        {
            if (temp > 256)
                hp_Temp -= 168;
            else if (temp > 16)
                hp_Temp -= 12;
            else
                hp_Temp--;
        }
        


        maxHp_units = maxHp_Temp % 10;
        maxHp_tens = maxHp_Temp / 10 % 10;
        maxHp_hundreds = maxHp_Temp / 100;

        if (maxHp > maxHp_Temp)
            maxHp_Temp++;
        else if (maxHp < maxHp_Temp)
            maxHp_Temp--;

        if (nHp != null && nMax != null)
        {
            HPdisplay(hp_Temp, hp_units, hp_tens, hp_hundreds, HP, nHp);
            HPdisplay(maxHp_Temp, maxHp_units, maxHp_tens, maxHp_hundreds, maxHP, nMax);

            HPdisplay();
        }
    }

    /// <summary>
    /// 血量更動時的動畫
    /// </summary>
    private void HPdisplay()
    {
        if (hp != hp_Temp || hp_Stat <= 1 || hp <= 3 && hp >= 1)
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
        rElapse -= Time.deltaTime;
        rElapse = Mathf.Clamp(rElapse, 0, 50f / recovery);

        if (rElapse == 0)
        {
            hp += (int)Mathf.Ceil(maxHp * 0.01f * (INT / 10));
            rElapse = 50f / recovery;
        }
    }

    public void Damage(int dmg, Transform trans)
    {
        hp -= dmg;
    }

    public void ToggleChat()
    {
        isShown = !isShown;
    }
    #endregion
}
