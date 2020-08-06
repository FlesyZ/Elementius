using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<Player> player = new List<Player>();
    private List<Animator> anim = new List<Animator>();

    private GameObject UI;

    private Image Black;
    private Image gameover;

    private float playOne = 1;

    void Awake()
    {
        StatWithElement[] stats = FindObjectsOfType<StatWithElement>();
        player.AddRange(FindObjectsOfType<Player>());

        for (int i = 0; i < player.Count; i++)
        {
            player[i].stat = stats[i];
            anim.Add(stats[i].GetComponent<Animator>());
        }

        UI = GameObject.Find("UI");
        Black = GameObject.Find("BlackScreen").GetComponent<Image>();
        gameover = GameObject.Find("GameOver").GetComponent<Image>();

        gameover.canvasRenderer.SetAlpha(1f);
        gameover.GetComponent<CanvasGroup>().alpha = 0;

        UI.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        UI.GetComponent<Canvas>().worldCamera = Camera.main;
        UI.GetComponent<Canvas>().sortingOrder = 100;
    }

    private IEnumerator FadeOut()
    {
        Black.canvasRenderer.SetAlpha(1f);
        Black.CrossFadeAlpha(0, 3f, false);
        yield return new WaitForSecondsRealtime(3f);
    }

    IEnumerator Start()
    {
        yield return FadeOut();

        for (int i = 0; i < anim.Count; i++)
        {
            anim[i].SetTrigger("Move");
            player[i].play = true;
        }
    }

    void Update()
    {
        int ifPlay = 0;
        foreach (var item in player)
            if (item.play) ifPlay++;

        if (ifPlay > 0)
        {
            for (int i = 0; i < anim.Count; i++)
            {
                anim[i].SetBool("isShown", player[i].stat.isShown);
            }
        }

        int alive = 0;
        foreach (var item in player)
            if (item.stat.HP > 0)
                alive++;
        if (alive == 0 && playOne != 0)
        {
            StartCoroutine(GameOver());
            playOne--;
        }
        
    }

    private IEnumerator GameOver()
    {
        gameover.GetComponent<CanvasGroup>().alpha = 1;
        gameover.CrossFadeAlpha(0.7f, 1f, false);
        yield return new WaitForSeconds(1f);

        gameover.CrossFadeAlpha(0f, 5f, false);
        yield return new WaitForSeconds(3f);

        Black.CrossFadeAlpha(1f, 4f, false);
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("TitleScene");
    }
}
