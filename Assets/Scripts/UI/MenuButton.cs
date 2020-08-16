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
        public int order;

        void Awake()
        {
            menu = GetComponentInParent<GameMenu>();

            button = GetComponent<Animator>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            if (menu && !GetComponent<Button>().interactable && menu.ButtonIndex == null) Obfuscated();
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
