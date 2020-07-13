using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text Speaker;
    public Text output;
    public Image bg;
    
    List<string> speaker = new List<string>();
    List<string> dialog = new List<string>();
    List<string> align = new List<string>();
    Dictionary<int, Sprite> chara = new Dictionary<int, Sprite>();

    bool next;
    float speed;

    /// <summary>
    /// 呼叫對話腳本
    /// </summary>
    /// <param name="page">頁碼(代碼)</param>
    public void Call(string page, bool isTransparent)
    {
        dialog.AddRange(ReadExcel.Read("Dialog", page, 0, -1));
        speaker.AddRange(ReadExcel.Read("Dialog", page, 1, -1));
        align.AddRange(ReadExcel.Read("Dialog", page, 2, -1));

        StartCoroutine(ProcessDialog(isTransparent));
    }

    private IEnumerator Show()
    {
        bg.CrossFadeAlpha(1f, 0.5f, false);
        yield return new WaitUntil(() => bg.canvasRenderer.GetAlpha() == 1f);
        
    }

    private IEnumerator Hide()
    {
        bg.CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitUntil(() => bg.canvasRenderer.GetAlpha() == 0f);
    }

    private IEnumerator ProcessDialog(bool alpha)
    {
        if (!alpha) yield return Show();

        for (int i = 0; i < dialog.Count; i++)
        {
            if (speaker[i] != "'")
            {
                if (align[i] == "LEFT")
                    Speaker.alignment = TextAnchor.MiddleLeft;
                else if (align[i] == "RIGHT")
                    Speaker.alignment = TextAnchor.MiddleRight;
                    
                Speaker.text = speaker[i];
            }
            
                List<string> text = new List<string>();

            for (int a = 0; a < dialog[i].Length; a++)
            {
                if (dialog[i].Substring(a, 1) == "\\")
                {
                    if (dialog[i].Substring(a, 2) == "\\s")
                    {
                        text.Add(dialog[i].Substring(a, 4));
                        a += 2;
                    }
                    else
                        text.Add(dialog[i].Substring(a, 2));

                    if (dialog[i].Substring(a, 2) == "\\^")
                        break;
                    else
                        a++;
                }
                else 
                    text.Add(dialog[i].Substring(a, 1));
            }

            yield return ShowingText(text);
            next = false;

            Speaker.text = "";
        }

        if (!alpha) yield return Hide();
    }

    private IEnumerator ShowingText(List<string> text)
    {
        foreach (var item in text)
        {
            if (item.Contains("\\") && !item.Contains("\\s"))
            {
                switch (item)
                {
                    case "\\\\":
                        output.text += "\\";
                        break;
                    case "\\n":
                        output.text += "\n";
                        break;
                    case "\\|":
                        yield return new WaitForSecondsRealtime(1f);
                        break;
                    case "\\.":
                        yield return new WaitForSecondsRealtime(0.25f);
                        break;
                    case "\\!":
                        yield return next;
                        next = false;
                        break;
                    case "\\^":
                        next = true;
                        break;
                    default:
                        break;
                }
            }
            else if (item.Contains("\\s"))
            {
                if (item.Substring(2).IsNumeric())
                {
                    speed = float.Parse(item.Substring(2));
                }
            }
            else
            {
                output.text += item;
                yield return new WaitForSecondsRealtime(speed * 0.02f);
            }
            
        }
        while (!next)
        {
            yield return null;
        }
        output.text = "";
    }

    private void Awake()
    {
        bg.canvasRenderer.SetAlpha(0f);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            next = true;
        }
    }
}
