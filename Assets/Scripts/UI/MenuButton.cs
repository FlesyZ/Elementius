using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuButton : MonoBehaviour
    {
        GameMenu menu;

        Animator button;
        Text text;

        public List<Text> child { get; set; } = new List<Text>();

        public int order;

        public bool unlocked { get; set; }

        void Awake()
        {
            menu = GetComponentInParent<GameMenu>();

            button = GetComponent<Animator>();
            text = GetComponent<Text>();

            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    child.Add(transform.GetChild(i).GetComponent<Text>());
                    child[i].gameObject.SetActive(false);
                }
            }
        }

        void Update()
        {
            if (menu && !unlocked) Obfuscated();

        }

        void Obfuscated()
        {
            text.text = "";

            string temp = name, result = "";
            char[] vs = temp.ToCharArray();
            byte[] value = Encoding.ASCII.GetBytes(vs);

            for (int i = 0; i < value.Length; i++)
            {
                value[i] -= 32;

                value[i] += (byte)Random.Range(-16, 16);
                if (value[i] < 65)
                    value[i] += 26;
                else if (value[i] > 90)
                    value[i] -= 26;

                value[i] += 32;
                vs[i] = (char)value[i];
                result += vs[i].ToString();
            }

            for (int i = 0; i < order; i++)
                text.text += " ";
            text.text += result;
        }

        public void MenuMovement()
        {
            button.SetBool("isShown", !button.GetBool("isShown"));
        }
    }
}
