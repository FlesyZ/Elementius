using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Element : MonoBehaviour
    {
        public int ID;

        [Header("元素性質")]
        public Elements stored = Elements.None;
        public int chained;

        Color color = new Color(0, 0, 0, 0);
        Color slot = new Color(1, 1, 1, 0);

        private Image typeStored;
        private bool fade;

        private void colorCode(float r, float g, float b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
        }

        private void Type()
        {
            switch (stored)
            {
                case Elements.None:
                    colorCode(0, 0, 0);
                    break;
                case Elements.Brave:
                    colorCode(1, 0, 0);
                    break;
                case Elements.Agile:
                    colorCode(0, 1, 0);
                    break;
                case Elements.Guard:
                    colorCode(0, 0, 1);
                    break;
                case Elements.Origin:
                    colorCode(1, 1, 0);
                    break;
                case Elements.Earth:
                    colorCode(1, 0, 1);
                    break;
                case Elements.Chaos:
                    colorCode(1, 1, 1);
                    break;
                case Elements.Iridescent:
                    colorCode(1, 1, 1);
                    break;
                case Elements.Dark:
                    colorCode(0, 0, 0);
                    break;
            }

            if (stored == Elements.Iridescent)
                color.a = 0.9f;
            else if (stored == Elements.None)
                color.a = 0f;
            else
                color.a = 0.5f;

            typeStored.color = color;
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
            if (ID < n) slot.a = 0.7f; else slot.a = 0f;
            gameObject.GetComponent<Image>().color = slot;
        }

        private IEnumerator Shining()
        {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.5f);
            while (true)
            {   
                if (stored == Elements.None)
                {
                    wait.waitTime = 0.1f;
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
                        wait.waitTime = 0.5f;
                    }
                }
                else
                {
                    if (fade)
                    {
                        typeStored.CrossFadeAlpha(0.5f, 2f, false);
                        fade = false;
                        wait.waitTime = 2f;
                    }
                    else
                    {
                        typeStored.CrossFadeAlpha(0.8f, 1f, false);
                        fade = true;
                        wait.waitTime = 1f;
                    }
                }
                yield return wait;
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