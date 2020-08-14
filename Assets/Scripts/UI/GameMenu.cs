using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        public bool menu;
        List<GameObject> menuButtons = new List<GameObject>();
        Animator[] buttons;



        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                menuButtons.Add(transform.GetChild(i).gameObject);
            }

            buttons = new Animator[menuButtons.Count];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = menuButtons[i].GetComponent<Animator>();
            }
        }

        void Start()
        {
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        void Update()
        {

        }

        public void MenuMovement()
        {

        }

        public IEnumerator ToggleMenu()
        {
            if (menu)
            yield return null;
            menu = !menu;
        }
    }
}
