using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        public List<string> ButtonIndex { get; set; } = new List<string>();

        bool isMenu;
        bool processing;
        public bool menu;

        List<GameObject> menuButtons = new List<GameObject>();
        Animator[] AoB;
        Button[] buttons;
        Text[] texts;

        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                menuButtons.Add(transform.GetChild(i).gameObject);
            }

            AoB = new Animator[menuButtons.Count];
            buttons = new Button[menuButtons.Count];
            texts = new Text[menuButtons.Count];

            MenuButton[] options = new MenuButton[menuButtons.Count];

            for (int i = 0; i < AoB.Length; i++)
            {
                AoB[i] = menuButtons[i].GetComponent<Animator>();
                buttons[i] = menuButtons[i].GetComponent<Button>();
                texts[i] = menuButtons[i].GetComponent<Text>();

                var colors = buttons[i].colors;
                colors.disabledColor = new Color(1f, 1f, 1f, 0.48f);
                colors.selectedColor = new Color(1f, 1f, 0f, 1f);
                colors.highlightedColor = new Color(1f, 1f, 0f, 1f);
                colors.pressedColor = new Color(0.7f, 0.7f, 0f, 1f);

                buttons[i].onClick.AddListener(
                    () => {
                        if(ButtonIndex == null)
                            ButtonIndex.Add(buttons[i].name);
                        else if (ButtonIndex.Contains(buttons[i].name))
                            ButtonIndex.Remove(buttons[i].name);

                        StartCoroutine(ToggleOption(buttons[i]));
                    }
                );

                options[i] = menuButtons[i].GetComponent<MenuButton>();
                options[i].order = i;

                buttons[i].interactable = false;
            }

            buttons[1].interactable = true;
        }

        void Start()
        {
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && menu && !processing)
            {
                if (ButtonIndex.Count == 0)
                    menu = false;
                else
                    ButtonIndex.RemoveAt(Mathf.Clamp(ButtonIndex.Count - 1, 0, ButtonIndex.Count - 1));
            }

            if (isMenu != menu && !processing)
            {
                StartCoroutine(ToggleMenu());
                isMenu = menu;
            }
        }

        public IEnumerator ToggleOption(Button selected)
        {
            processing = true;

            Animator anim = new Animator();

            switch (ButtonIndex[0])
            {
                case "Skills":
                    break;
                case "Status":
                    Text[] status = selected.transform.GetChild(0).GetComponentsInChildren<Text>();
                    Player player = FindObjectOfType<Player>();

                    status[1].text = player.stat.STR.ToString();
                    status[2].text = player.stat.AGI.ToString();
                    status[3].text = player.stat.INT.ToString();
                    status[4].text = player.stat.LUK.ToString();

                    anim = selected.transform.GetChild(0).GetComponent<Animator>();
                    break;
                case "Settings":
                    break;
                case null:
                    anim = selected.transform.GetChild(0).GetComponent<Animator>();
                    break;
            }

            anim.SetTrigger("Move");

            yield return new WaitForSeconds(0.5f);

            processing = false;
        }

        public IEnumerator ToggleMenu()
        {
            processing = true;

            Rigidbody2D[] all = FindObjectsOfType<Rigidbody2D>();
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            
            if (menu)
            {
                GetComponent<CanvasGroup>().alpha = 1f;
                for (int i = 0; i < all.Length; i++)
                    all[i].constraints = RigidbodyConstraints2D.FreezeAll;
                for (int i = 0; i < enemies.Length; i++)
                    enemies[i].getFreezed = true;
            }

            foreach (var item in AoB)
            {
                item.SetTrigger("Move");
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            if (!menu)
            {
                GetComponent<CanvasGroup>().alpha = 0f;
                for (int i = 0; i < all.Length; i++)
                    all[i].constraints = RigidbodyConstraints2D.FreezeRotation;
                for (int i = 0; i < enemies.Length; i++)
                    enemies[i].getFreezed = false;
            }

            processing = false;
        }


    }
}
