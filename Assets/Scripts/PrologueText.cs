using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrologueText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float charDelay;
    public GameObject next;
    public TextMeshProUGUI next_text;
    public float colorDelay;

    void Start()
    {
        StartCoroutine(typeText());
    }

    IEnumerator typeText()
    {
        charDelay *= 2;
        next.SetActive(false);
        next_text.color = new Color(1, 1, 1, 0);
        string txt = text.text;
        text.text = "";
        for (int i = 0; i < txt.Length; i++)
        {
            text.text += txt[i];
            if (i == 28)
                charDelay /= 2;
            if (txt[i] == '.')
                yield return new WaitForSeconds(charDelay);
            yield return new WaitForSeconds(charDelay);
        }
        next.SetActive(true);
        for (float i = 0; i <= 1; i+=0.01f)
        {
            next_text.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(colorDelay);
        }
    }
}
