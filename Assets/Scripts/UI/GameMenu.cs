using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        int? option;
        List<int> SelectedOption = new List<int>();

        bool isMenu;
        bool processing;
        int isUnlocked;
        public bool menu { get; private set; }

        public List<GameObject> menuButtons { get; set; } = new List<GameObject>();
        List<Text> texts = new List<Text>();
        MenuButton[] options;
        Animator[] anim;

        AudioSource select;

        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                menuButtons.Add(transform.GetChild(i).gameObject);
            }

            anim = new Animator[menuButtons.Count];

            options = new MenuButton[menuButtons.Count];

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i] = menuButtons[i].GetComponent<Animator>();
                texts.Add(menuButtons[i].GetComponent<Text>());

                options[i] = menuButtons[i].GetComponent<MenuButton>();
                options[i].order = i;
            }

            select = gameObject.AddComponent<AudioSource>();

            options[2].unlocked = true;

            select.clip = Resources.Load<AudioClip>("Sounds/Select");
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
                        //main.volume = 0.5f;
                        select.Play();
                        menu = false;
                    }
                    else
                    {
                        option = SelectedOption[SelectedOption.Count - 1];
                        StartCoroutine(ToggleOption(menuButtons[SelectedOption[SelectedOption.Count - 1]].transform.GetChild(0).GetComponent<Text>(), true, false));
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (option != null && SelectedOption.Count == 0)
                    {
                        //main.volume = 0.5f;
                        select.Play();

                        Selected();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (SelectedOption.Count == 0 || option != null)
                    {
                        option--;
                        if (option < 0)
                            option += texts.Count;

                        OptionRejudge(true);
                    }
                    /*
                    else if (SelectedOption[0] == 2 || option != null)
                    {
                        option--;
                        if (option < 0)
                            option += texts.Count;
                    }
                    */

                    select.Play();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (SelectedOption.Count == 0 || option != null)
                    {
                        option++;
                        if (option >= texts.Count)
                            option = 0;

                        OptionRejudge(false);
                    }
                    /*
                    else if (SelectedOption[0] == 2 || option != null)
                    {
                        option++;
                        if (option >= texts.Count)
                            option = 0;
                    }
                    */

                    select.Play();
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

        void OptionRejudge(bool isBackwards)
        {
            isUnlocked = 0;
            if (isBackwards)
            {
                for (int i = menuButtons.Count - 1; i >= 0; i--)
                {
                    if (option == i && !options[i].unlocked)
                    {
                        option--;
                        isUnlocked--;
                    }
                    if (isUnlocked <= menuButtons.Count * -1)
                        option = null;

                    if (option < 0)
                        option += texts.Count;
                }
            }
            else
            {
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
        }

        public void Selected()
        {
            if (SelectedOption.Count == 0)
            {
                SelectedOption.Add((int)option);

                if (menuButtons[SelectedOption[SelectedOption.Count - 1]].transform.childCount > 0)
                {
                    StartCoroutine(ToggleOption(menuButtons[SelectedOption[SelectedOption.Count - 1]].transform.GetChild(0).GetComponent<Text>(), false, true));
                }
                else
                {
                    SelectedOption.RemoveAt(SelectedOption.Count - 1);
                }
            }
        }

        public void Display()
        {
            if (SelectedOption.Count == 0)
            {
                if (option != null)
                {
                    Color nonSelected = new Color(0.7f, 0.7f, 0.7f, 1f);
                    ColorRenderer(texts, nonSelected);

                    for (int i = 0; i < texts.Count; i++)
                    {
                        if (!options[i].unlocked)
                            ColorHighlighter(texts[i], Color.gray);
                    }

                    ColorHighlighter(texts[(int)option], Color.yellow);
                }
                else
                    ColorRenderer(texts, Color.gray);
            }
            else if (SelectedOption[0] == 2)
            {
                if (option != null)
                {
                    Color nonSelected = new Color(0.7f, 0.7f, 0.7f, 1f);
                    ColorRenderer(texts, nonSelected);

                    for (int i = 0; i < texts.Count; i++)
                    {
                        if (!options[i].unlocked)
                            ColorHighlighter(texts[i], Color.gray);
                    }

                    ColorHighlighter(texts[(int)option], Color.yellow);
                }
                else
                    ColorRenderer(texts, Color.gray);
            }
            else 
            {
                Color selected = new Color(0.7f, 0.7f, 0f, 1f);
                ColorRenderer(texts, Color.gray);
                ColorHighlighter(texts[SelectedOption[0]], selected);
            }
        }

        #region 處理顏色
        void ColorRenderer(List<Text> texts, Color color)
        {
            foreach (var item in texts)
                item.color = color;
        }

        void ColorHighlighter(Text text, Color highlight)
        {
            text.color = highlight;
        }
        #endregion

        public IEnumerator ToggleOption(Text selected, bool isRemove, bool value)
        {
            selected.gameObject.SetActive(value);
            
            processing = true;

            Animator animator;


            if (isRemove)
            {
                switch (option)
                {
                    case 0:
                        break;
                    case 1:
                        option = 1;
                        break;
                    case 2:
                        Display();

                        menuButtons.Clear();
                        texts.Clear();

                        for (int i = 0; i < transform.childCount; i++)
                        {
                            menuButtons.Add(transform.GetChild(i).gameObject);
                        }

                        for (int i = 0; i < anim.Length; i++)
                        {
                            texts.Add(menuButtons[i].GetComponent<Text>());
                        }

                        option = 2;
                        break;
                    case null:
                        break;
                }

                SelectedOption.RemoveAt(SelectedOption.Count - 1);
            }
            else
            {
                switch (option)
                {
                    case 0:
                        break;
                    case 1:

                        Text[] status = selected.GetComponentsInChildren<Text>();
                        Player player = FindObjectOfType<Player>();

                        status[1].text = player.stat.STR.ToString();
                        status[2].text = player.stat.AGI.ToString();
                        status[3].text = player.stat.INT.ToString();
                        status[4].text = player.stat.LUK.ToString();

                        option = null;
                        break;
                    case 2:
                        Time.timeScale = 1f;
                        Destroy(GameObject.Find("UI"));
                        SceneManager.LoadScene("TitleScene");
                        break;
                    case null:
                        break;
                }
            }

            if (option != 2)
            {
                animator = selected.GetComponent<Animator>();
                animator.SetTrigger("Move");
            }

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
                OptionRejudge(false);
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
