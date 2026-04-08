using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notepad : MonoBehaviour
{
    public AppLayer appLayer;
    private TextMeshProUGUI titleText;
    private TMP_InputField inputField;
    public TMP_FontAsset font;
    
    void Start()
    {
        BuildUI();
        if (appLayer != null)
        {
            appLayer.sizeDelta = new Vector2(600, 400);
            appLayer.appMinSize = new Vector2(300, 200);
            appLayer.color = new Color(31, 31, 31);
        }
    }

    void BuildUI()
    {
        GameObject main = new GameObject("main", typeof(RectTransform), typeof(VerticalLayoutGroup));
        main.transform.SetParent(transform, false);
        Stretch(main.GetComponent<RectTransform>());

        var vGroup = main.GetComponent<VerticalLayoutGroup>();
        vGroup.childControlWidth = true; vGroup.childControlHeight = true;
        vGroup.childForceExpandHeight = false;


        GameObject header = new GameObject("Header", typeof(RectTransform), typeof(Image));
        header.transform.SetParent(main.transform, false);
        header.GetComponent<Image>().color = new Color(0.31f, 0.31f, 0.31f);
        header.AddComponent<LayoutElement>().preferredHeight = 20;

        titleText = CreateTMPText("Новый текстовый документ.txt", header.transform, 20, Color.white);
        titleText.alignment = TextAlignmentOptions.Left;
        titleText.rectTransform.offsetMin = new Vector2(5, 0);
        titleText.font = font;



        GameObject scrollGo = new GameObject("TextScroll", typeof(RectTransform), typeof(ScrollRect), typeof(Image));
        scrollGo.transform.SetParent(main.transform, false);
        scrollGo.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f);
        scrollGo.AddComponent<LayoutElement>().flexibleHeight = 1;

        GameObject inputGo = new GameObject("InputField", typeof(RectTransform), typeof(TMP_InputField));
        inputGo.transform.SetParent(scrollGo.transform, false);
        Stretch(inputGo.GetComponent<RectTransform>());

        inputField = inputGo.GetComponent<TMP_InputField>();
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;



        GameObject textArea = new GameObject("TextArea", typeof(RectTransform), typeof(RectMask2D));
        textArea.transform.SetParent(inputGo.transform, false);
        Stretch(textArea.GetComponent<RectTransform>());
        textArea.GetComponent<RectTransform>().offsetMin = new Vector2(5, 5);

        GameObject textDisplay = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textDisplay.transform.SetParent(textArea.transform, false);
        Stretch(textDisplay.GetComponent<RectTransform>());

        var t = textDisplay.GetComponent<TextMeshProUGUI>();
        t.color = Color.white;
        t.fontSize = 25;
        t.font = font;

        inputField.textViewport = textArea.GetComponent<RectTransform>();
        inputField.textComponent = t;
    }

    public void OpenFile(string fileName, string content)
    {
        if (titleText != null) titleText.text = fileName;
        if (inputField != null) inputField.text = content;
        if (appLayer != null) appLayer.Show();
    }

    TextMeshProUGUI CreateTMPText(string content, Transform parent, int size, Color col)
    {
        GameObject go = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        Stretch(go.GetComponent<RectTransform>());
        var t = go.GetComponent<TextMeshProUGUI>();
        t.text = content; t.fontSize = size; t.color = col;
        t.font = font;
        return t;
    }

    void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
    }
}