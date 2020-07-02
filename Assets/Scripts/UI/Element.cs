using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Elements
{
    None, Brave, Agile, Guard, Origin, Earth, Chaos, Iridescent, Dark = 9
}

public class Element : MonoBehaviour
{
    public int ID;

    [Header("元素性質")]
    public Elements stored = Elements.None;
    public int chained;

    private Image typeStored;
    private bool fade;

    private void type()
    {
        switch (stored)
        {
            case Elements.None:
                break;
            case Elements.Brave:
                break;
            case Elements.Agile:
                break;
            case Elements.Guard:
                break;
            case Elements.Origin:
                break;
            case Elements.Earth:
                break;
            case Elements.Chaos:
                break;
            case Elements.Iridescent:
                break;
            case Elements.Dark:
                break;
        }
    }

    private void chain()
    {
        chained = 0;
        for (int i = ID; i >= 0; i--)
        {
            Element target = GameObject.Find("Slot (" + i + ")").GetComponent<Element>();
            if (target.stored != stored) break;
            if (stored != 0) chained++;
        }
        for (int i = ID; i < 32; i++)
        {
            Element target = GameObject.Find("Slot (" + i + ")").GetComponent<Element>();
            if (target.stored != stored) break;
            if (stored != 0) chained++;
        }
    }

    private IEnumerator Shining()
    {
        while (true)
        {
            if (fade)
            {
                typeStored.CrossFadeColor(new Color(typeStored.color.r, typeStored.color.g, typeStored.color.b, 0.5f), 2f, false, true);
                yield return new WaitForSeconds(2f);
            }
            else
            {
                typeStored.CrossFadeColor(new Color(typeStored.color.r, typeStored.color.g, typeStored.color.b, 0.8f), 1f, false, true);
                yield return new WaitForSeconds(1f);
            }
                
        }
    }

    private void Awake()
    {
        string name = gameObject.name.Replace("Slot (", "").Replace(")","");
        ID = int.Parse(name);

        typeStored = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        
    }
}
