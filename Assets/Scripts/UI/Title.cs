using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Image background;
    public Image title;
    private Image[] buttons;
    public Image blackScreen;
    
    private List<string> SaveData = new List<string>();

    void Awake()
    {
        buttons = GameObject.Find("Buttons").GetComponentsInChildren<Image>();

        background.canvasRenderer.SetAlpha(0f);
        title.canvasRenderer.SetAlpha(0f);
        blackScreen.canvasRenderer.SetAlpha(1f);
        foreach (var item in buttons) item.canvasRenderer.SetAlpha(0f);
    }
    
    IEnumerator Start()
    {
        blackScreen.CrossFadeAlpha(0f, 0.3f, false);
        yield return new WaitForSeconds(0.3f);
        blackScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;

        background.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(0.7f);

        title.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(1.3f);

        foreach (var item in buttons)
        {
            item.CrossFadeAlpha(1f, 1f, false);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Update()
    {
        
    }

    public void NewGame()
    {
        blackScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        StartCoroutine(EnterGame());
    }

    IEnumerator EnterGame()
    {
        yield return new WaitForSeconds(1f);

        blackScreen.CrossFadeAlpha(1f, 2.5f, false);
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("UI");
    }

    public void Continue()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
