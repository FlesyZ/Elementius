using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        int? option;
        List<int> SelectedOption = new List<int>();

        bool isMenu;
        bool processing;
        public bool menu { get; private set; }

        List<GameObject> menuButtons = new List<GameObject>();
        MenuButton[] options;
        Animator[] anim;
        Text[] texts;

        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                menuButtons.Add(transform.GetChild(i).gameObject);
            }

            anim = new Animator[menuButtons.Count];
            texts = new Text[menuButtons.Count];

            options = new MenuButton[menuButtons.Count];

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i] = menuButtons[i].GetComponent<Animator>();
                texts[i] = menuButtons[i].GetComponent<Text>();

                options[i] = menuButtons[i].GetComponent<MenuButton>();
                options[i].order = i;
            }

            menuButtons[2].GetComponent<MenuButton>().unlocked = true;
        }

        void Start()
        {
            GetComponent<CanvasGroup>().alpha = 0f;
        }

        void Update()
        {
            if (SelectedOption.Count > 0) Debug.Log(SelectedOption[0]);

            if (menu)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (SelectedOption.Count == 0)
                    {
                        menu = false;
                    }
                    else
                    {
                        int x = Mathf.Clamp(SelectedOption.Count - 1, 0, SelectedOption.Count - 1);
                        option = SelectedOption[x];
                        StartCoroutine(ToggleOption(menuButtons[(int)option].transform.GetChild(0).GetComponent<Text>(), false));
                        SelectedOption.RemoveAt(x);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (option != null)
                        Selected();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (SelectedOption == null || option != null)
                    {
                        option--;
                        if (option < 0)
                            option += texts.Length;

                        OptionRejudge();
                    }
                    Debug.Log(option);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (SelectedOption == null || option != null)
                    {
                        option++;
                        if (option >= texts.Length)
                            option = 0;

                        OptionRejudge();
                    }
                    Debug.Log(option);
                }

                Display();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !menu)
            {
                menu = true;
            }

            if (isMenu != menu && !processing)
            {
                StartCoroutine(ToggleMenu());
                isMenu = menu;
            }

        }

        void OptionRejudge()
        {
            int isUnlocked = 0;
            for (int i = 0; i < menuButtons.Count; i++)
            {
                if (option == i && !options[i].unlocked)
                {
                    option++;
                    isUnlocked++;
                }
                if (isUnlocked >= menuButtons.Count)
                    option = null;
            }
        }

        public void Selected()
        {
            if (SelectedOption.Count == 0)
            {
                SelectedOption.Add((int)option);
                StartCoroutine(ToggleOption(menuButtons[(int)option].transform.GetChild(0).GetComponent<Text>(), true));
                option = null;
            }
        }

        public void Display()
        {
            if (SelectedOption.Count == 0)
            {
                if (option != null)
                {
                    ColorRenderer(texts, Color.gray);
                    ColorHighlighter(texts[(int)option], Color.yellow);
                }
                else
                    ColorRenderer(texts, Color.gray);
            }
            else
            {
                Color nonSelected = new Color(0.7f, 0.7f, 0.7f, 1f);
                Color selected = new Color(0.7f, 0.7f, 0f, 1f);
                ColorRenderer(texts, nonSelected);
                ColorHighlighter(texts[SelectedOption[0]], selected);
            }
        }

        #region 處理顏色
        void ColorRenderer(Text[] texts, Color color)
        {
            foreach (var item in texts)
                item.color = color;
        }

        void ColorHighlighter(Text text, Color highlight)
        {
            text.color = highlight;
        }
        #endregion

        public IEnumerator ToggleOption(Text selected, bool value)
        {
            selected.canvasRenderer.SetAlpha(value ? 1 : 0);
            
            processing = true;

            Animator anim;

            switch (option)
            {
                case 0:
                    break;
                case 1:
                    Text[] status = selected.transform.GetChild(0).GetComponentsInChildren<Text>();
                    Player player = FindObjectOfType<Player>();

                    status[1].text = player.stat.STR.ToString();
                    status[2].text = player.stat.AGI.ToString();
                    status[3].text = player.stat.INT.ToString();
                    status[4].text = player.stat.LUK.ToString();
                    break;
                case 2:
                    break;
                case null:
                    break;
            }

            anim = selected.transform.GetChild(0).GetComponent<Animator>();
            anim.SetTrigger("Move");

            yield return new WaitForSecondsRealtime(0.5f);

            processing = false;
        }

        public IEnumerator ToggleMenu()
        {
            processing = true;

            if (menu)
            {
                GetComponent<CanvasGroup>().alpha = 1f;
                Time.timeScale = 0;

                option = 0;
                OptionRejudge();
            }

            foreach (var item in anim)
            {
                item.SetTrigger("Move");
                yield return null;
            }
            yield return new WaitForSecondsRealtime(0.5f);

            if (!menu)
            {
                GetComponent<CanvasGroup>().alpha = 0f;
                Time.timeScale = 1;
            }

            processing = false;
        }


    }
}
