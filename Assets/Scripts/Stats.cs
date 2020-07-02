using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private Image[] HP = new Image[3];
    private Image[] maxHP = new Image[3];

    private UI.Numbers nHp, nMax;

    private UI.Heart heart;

    [Header("血量"), Range(0, 9999)]
    public int hp = 20;

    private int maxH;
    public int maxHp { get { return maxH; } set { maxH = value; } }

    private int hp_Temp, maxHp_Temp, hp_Stat;
    private int hp_units, hp_tens, hp_hundreds, hp_thousands, maxHp_units, maxHp_tens, maxHp_hundreds, maxHp_thousands;

    private void Awake()
    {
        HP = GameObject.Find("Health Value").GetComponentsInChildren<Image>();
        maxHP = GameObject.Find("Max Health Value").GetComponentsInChildren<Image>();

        nHp = GameObject.Find("Health Value").GetComponent<UI.Numbers>();
        nMax = GameObject.Find("Max Health Value").GetComponent<UI.Numbers>();

        heart = GameObject.Find("Heart").GetComponent<UI.Heart>();
    }

    private void Start()
    {
        maxH = hp;

        hp_Temp = hp;
        maxHp_Temp = maxHp;
    }

    private void LateUpdate()
    {
        Health();
    }

    #region method
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

        if (hp > hp_Temp)
        {
            hp_Temp++;
            HPdisplay();
        }
        else if (hp < hp_Temp)
        {
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
                HP[i].transform.rotation = Quaternion.Euler(0, 0, Random.Range(-5.0f, 5.0f));
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
        if (temp < 10000)
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
}
