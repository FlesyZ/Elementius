using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Elements
{
    None, Brave, Agile, Guard, Origin, Earth, Chaos, Iridescent, Dark = 9
}
namespace UI
{
    public class Element : MonoBehaviour
    {
        public int ID;

        [Header("元素性質")]
        public Elements stored = Elements.None;
        public int chained;

        private Image typeStored;
        private bool fade;

        private void Type()
        {
            switch (stored)
            {
                case Elements.None:
                    typeStored.color = new Color(0f, 0f, 0f, 0f);
                    break;
                case Elements.Brave:
                    typeStored.color = new Color(1f, 0f, 0f, 0.5f);
                    break;
                case Elements.Agile:
                    typeStored.color = new Color(0f, 1f, 0f, 0.5f);
                    break;
                case Elements.Guard:
                    typeStored.color = new Color(0f, 0f, 1f, 0.5f);
                    break;
                case Elements.Origin:
                    typeStored.color = new Color(1f, 1f, 0f, 0.5f);
                    break;
                case Elements.Earth:
                    typeStored.color = new Color(1f, 0f, 1f, 0.5f);
                    break;
                case Elements.Chaos:
                    typeStored.color = new Color(1f, 1f, 1f, 0.5f);
                    break;
                case Elements.Iridescent:
                    typeStored.color = new Color(1f, 1f, 1f, 0.9f);
                    break;
                case Elements.Dark:
                    typeStored.color = new Color(0f, 0f, 0f, 0.5f);
                    break;
            }
        }

        private void Chain()
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
                if (stored != 0 && i != ID) chained++;
            }
        }

        public void Base(int n)
        {
            if (ID < n)
            {
                gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            }
        }

        private IEnumerator Shining()
        {
            while (true)
            {
                if (stored == Elements.None)
                {
                    yield return new WaitForSeconds(0.05f);
                }
                else if (stored == Elements.Iridescent)
                {
                    for (int i = 0; i <= 5; i++)
                    {
                        Color irid = new Color(0f, 0f, 0f, 0.9f);
                        if (i == 2 || i == 3 || i == 4)
                            irid.r = 1f;
                        else
                            irid.r = 0f;
                        if (i == 4 || i == 5 || i == 0)
                            irid.g = 1f;
                        else
                            irid.g = 0f;
                        if (i == 0 || i == 1 || i == 2)
                            irid.b = 1f;
                        else
                            irid.b = 0f;
                        typeStored.CrossFadeColor(irid, 0.5f, false, true);
                        yield return new WaitForSecondsRealtime(0.5f);
                    }
                }
                else
                {
                    if (fade)
                    {
                        typeStored.CrossFadeAlpha(0.5f, 2f, false);
                        fade = false;
                        yield return new WaitForSecondsRealtime(2f);
                    }
                    else
                    {
                        typeStored.CrossFadeAlpha(0.8f, 1f, false);
                        fade = true;
                        yield return new WaitForSecondsRealtime(2f);
                    }
                }
            }
        }

        private void Awake()
        {
            string name = gameObject.name.Replace("Slot (", "").Replace(")", "");
            ID = int.Parse(name);

            typeStored = gameObject.GetComponentInChildren<UI.StoredElement>().GetComponent<Image>();

            StartCoroutine(Shining());
        }

        private void Update()
        {
            Type();
            Chain();
        }
    }
}