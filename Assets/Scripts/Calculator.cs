using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Calculator : MonoBehaviour
{
    public AppLayer appLayer;
    [Header("Настройки")]
    public GameObject buttonPrefab;
    public Font font;
    private Text displayText;
    private RectTransform gridRect;
    private GridLayoutGroup gridLayout;

    // Поля для программистапрограммиста
    private GameObject programmerPanel;
    private Text hexVal, decVal, octVal, binVal;

    private string currentInput = "";
    private bool isProgrammerMode = false;

    private Dictionary<string, Text> sysTexts = new Dictionary<string, Text>();
    private int currentBase = 10;

    private readonly string[] normalLayout = {
        "%", "CE", "CLR", "<-",
        "1/x", "x²", "√x", "/",
        "7", "8", "9", "x",
        "4", "5", "6", "-",
        "1", "2", "3", "+",
        "+/-", "0", ",", "="
    };

    private readonly string[] programmerLayout = {
        "A", "<<", ">>", "CLR", "<-",
        "B", "(", ")", "%", "/",
        "C", "7", "8", "9", "x",
        "D", "4", "5", "6", "-",
        "E", "1", "2", "3", "+",
        "F", "+/-", "0", ",", "="
    };

    [Header("Индикация выбора")]
    private Image selectionIndicator; 
    private string activeSystem = "DEC"; 

    private Button normalModeBtn;
    private Button programmerModeBtn;

    private void Awake()
    {
        appLayer = GetComponentInParent<AppLayer>();
    }
    void Start()
    {
        appLayer.appMinSize = new Vector2(300, 400);
        appLayer.sizeDelta = new Vector2(300, 400);
        appLayer.color = new Color(31, 31, 31);
        InitializeUI();
//SetMode(true);
    }

    void InitializeUI()
    {
        // 1. Дисплей
        GameObject displayObj = new GameObject("Display", typeof(RectTransform), typeof(Image));
        displayObj.transform.SetParent(transform, false);
        displayObj.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        RectTransform dispRT = displayObj.GetComponent<RectTransform>();
        dispRT.anchorMin = new Vector2(0, 0.65f);
        dispRT.anchorMax = new Vector2(1, 0.9f);
        dispRT.offsetMin = dispRT.offsetMax = Vector2.zero;

        displayText = CreateTextElement("0", 100, displayObj.transform);
        displayText.alignment = TextAnchor.MiddleRight;
        displayText.rectTransform.sizeDelta = new Vector2(0, 0);

        // 2. Создаем панель систем счисления
        CreateProgrammerInfoPanel();

        // 3. Сетка кнопок
        GameObject gridObj = new GameObject("Grid", typeof(RectTransform), typeof(GridLayoutGroup));
        gridObj.transform.SetParent(transform, false);
        gridRect = gridObj.GetComponent<RectTransform>();

        gridRect.anchorMin = new Vector2(0, 0);
        gridRect.anchorMax = new Vector2(1, 0.6f);
        gridRect.offsetMin = gridRect.offsetMax = Vector2.zero;

        gridLayout = gridObj.GetComponent<GridLayoutGroup>();
        gridLayout.padding = new RectOffset(5, 5, 5, 5);
        gridLayout.spacing = new Vector2(2, 2);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        CreateModeButtons();

        SetMode(false); // Начинаем с обычного режима
    }

    void CreateModeButtons()
    {
        GameObject panel = new GameObject("ModeButtons", typeof(RectTransform));
        panel.transform.SetParent(transform, false);

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0.90f);
        rt.anchorMax = new Vector2(1, 1);
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        HorizontalLayoutGroup hlg = panel.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 10;
        hlg.childForceExpandWidth = true;
        hlg.childAlignment = TextAnchor.MiddleCenter;

        normalModeBtn = CreateModeButton("Обычный", panel.transform);
        programmerModeBtn = CreateModeButton("Программист", panel.transform);

        normalModeBtn.onClick.AddListener(() => SetMode(false));
        programmerModeBtn.onClick.AddListener(() => SetMode(true));

        UpdateModeButtonsVisual();
    }

    Button CreateModeButton(string text, Transform parent)
    {
        GameObject btnObj = Instantiate(buttonPrefab, parent);
        btnObj.name = text + "Button";

        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(160, 32);

        TextMeshProUGUI label = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        label.text = text;

        return btnObj.GetComponent<Button>();
    }

    void UpdateModeButtonsVisual()
    {
        SetModeButtonState(normalModeBtn, !isProgrammerMode);
        SetModeButtonState(programmerModeBtn, isProgrammerMode);
    }

    void SetModeButtonState(Button btn, bool active)
    {
        Image img = btn.GetComponent<Image>();
        TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();

        img.color = active
            ? new Color32(118, 185, 237, 255)   // Синий
            : new Color32(45, 45, 45, 255);     // Серый

        txt.color = active ? Color.black : Color.white;
    }

    // Смена режима из редактора
    [ContextMenu("Toggle Mode")]
    public void ToggleMode() => SetMode(!isProgrammerMode);

    public void SetMode(bool programmer)
    {
        isProgrammerMode = programmer;
        programmerPanel.SetActive(programmer);

        // В программисте 5 колонок, в обычном 4
        gridLayout.constraintCount = isProgrammerMode ? 5 : 4;

        // Меняем высоту сетки
        gridRect.anchorMax = new Vector2(1, isProgrammerMode ? 0.50f : 0.65f);
        RectTransform dispRT = displayText.transform.parent.GetComponent<RectTransform>();
        dispRT.anchorMin = new Vector2(0, isProgrammerMode ? 0.75f : 0.65f);

        CreateButtons(gridRect);
        UpdateLayout();
        UpdateDisplay();
        UpdateModeButtonsVisual();
    }

    void CreateProgrammerInfoPanel()
    {
        programmerPanel = new GameObject("ProgrammerValues", typeof(RectTransform));
        programmerPanel.transform.SetParent(transform, false);

        RectTransform rt = programmerPanel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0.50f);
        rt.anchorMax = new Vector2(1, 0.65f);
        rt.offsetMin = new Vector2(0, 0); rt.offsetMax = new Vector2(0, 0);

        // Создаем синий индикатор
        GameObject indicatorObj = new GameObject("Indicator", typeof(RectTransform), typeof(Image));
        indicatorObj.transform.SetParent(programmerPanel.transform, false);
        selectionIndicator = indicatorObj.GetComponent<Image>();
        selectionIndicator.color = new Color32(118, 185, 237, 255);
        RectTransform indRT = selectionIndicator.GetComponent<RectTransform>();
        indRT.anchorMin = indRT.anchorMax = new Vector2(0, 0.5f);
        indRT.sizeDelta = new Vector2(4, 20); 

        // Контейнер для строк
        GameObject rowsCont = new GameObject("Rows", typeof(RectTransform), typeof(VerticalLayoutGroup));
        rowsCont.transform.SetParent(programmerPanel.transform, false);
        RectTransform rowsRT = rowsCont.GetComponent<RectTransform>();
        rowsRT.anchorMin = Vector2.zero; rowsRT.anchorMax = Vector2.one;
        rowsRT.offsetMin = new Vector2(30, 0);
        rowsRT.offsetMax = new Vector2(0, 20);

        VerticalLayoutGroup vlg = rowsCont.GetComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true; vlg.childForceExpandHeight = true;

        string[] systems = { "HEX", "DEC", "OCT", "BIN" };
        foreach (string s in systems)
        {
            GameObject row = new GameObject(s, typeof(RectTransform), typeof(Button));
            row.transform.SetParent(rowsCont.transform, false);

            Text label = CreateTextElement(s, 18, row.transform);
            label.alignment = TextAnchor.MiddleLeft;
            label.color = new Color(0.7f, 0.7f, 0.7f); // Серый
            label.rectTransform.sizeDelta = new Vector2(20, 0);

            Text valText = CreateTextElement("0", 25, row.transform);
            valText.alignment = TextAnchor.MiddleLeft;
            valText.rectTransform.offsetMin = new Vector2(55, 0);

            sysTexts.Add(s, valText);

            string sysName = s;
            int b = (s == "HEX") ? 16 : (s == "DEC") ? 10 : (s == "OCT") ? 8 : 2;
            row.GetComponent<Button>().onClick.AddListener(() => SelectActiveSystem(sysName, b, row.GetComponent<RectTransform>()));
            if (b == 10)
                StartCoroutine(waitLoad(sysName, b, row.GetComponent<RectTransform>()));
        }
    }

    IEnumerator<object> waitLoad(string sysName, int b, RectTransform rowRT)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SelectActiveSystem(sysName, b, rowRT);
    }

    void SelectActiveSystem(string name, int b, RectTransform rowRT)
    {
        activeSystem = name;
        currentBase = b;

        // Перемещаем индикатор к выбранной строке
        selectionIndicator.transform.SetParent(rowRT, false);
        RectTransform indRT = selectionIndicator.GetComponent<RectTransform>();
        indRT.anchorMin = new Vector2(0, 0.5f);
        indRT.anchorMax = new Vector2(0, 0.5f);
        indRT.anchoredPosition = new Vector2(-20, 0); 

        if (!string.IsNullOrEmpty(currentInput) && currentInput != "0")
        {
            try
            {
                long val = Convert.ToInt64(sysTexts["DEC"].text); 
                currentInput = Convert.ToString(val, currentBase).ToUpper();
            }
            catch { }
        }

        UpdateButtonsInteractivity();
        UpdateDisplay();
    }

    void UpdateButtonsInteractivity()
    {
        if (!isProgrammerMode) return;

        foreach (Transform child in gridRect)
        {
            Button btn = child.GetComponent<Button>();
            string label = child.name.Replace("(Clone)", "").Trim();

            if (label.Length == 1)
            {
                char c = label[0];
                if (char.IsDigit(c) || (c >= 'A' && c <= 'F'))
                {
                    int val = -1;
                    if (char.IsDigit(c)) val = c - '0';
                    else val = 10 + (c - 'A');

                    btn.interactable = (val < currentBase);
                    btn.GetComponentInChildren<TextMeshProUGUI>().alpha = btn.interactable ? 1f : 0.3f;
                }
            }
        }
    }

    void CreateButtons(Transform parent)
    {
        foreach (Transform child in parent) Destroy(child.gameObject);

        string[] currentLayout = isProgrammerMode ? programmerLayout : normalLayout;

        foreach (string label in currentLayout)
        {
            GameObject btnObj = Instantiate(buttonPrefab, parent);
            btnObj.name = label;
            TextMeshProUGUI t = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            t.text = (label == "<-") ? "<-" : label;
            t.color = Color.white;

            Image img = btnObj.GetComponent<Image>();
            if (label == "=") img.color = new Color32(118, 185, 237, 255);
            else if (char.IsLetter(label[0]) || label.Contains("<") || label.Contains("(")) img.color = new Color32(45, 45, 45, 255);
            else if (char.IsDigit(label[0]) || label == ",") img.color = new Color32(59, 59, 59, 255);
            else img.color = new Color32(50, 50, 50, 255);

            string captured = label;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnKeyClick(captured));
        }
    }

    void OnKeyClick(string key)
    {
        if (currentInput == "Error") currentInput = "";

        if (key == "CLR" || key == "CE") { currentInput = ""; UpdateSystemsValues(); }
        else if (key == "<-") { if (currentInput.Length > 0) currentInput = currentInput.Remove(currentInput.Length - 1); }
        else if (key == "=") Calculate();
        else if (key == "%" || key == "x²" || key == "√x" || key == "1/x") ApplyImmediateOperation(key);
        else if (key == "+/-") HandleNegative();
        else
        {
            string val = (key == "x") ? "*" : (key == ",") ? "." : key;
            currentInput += val;
        }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        displayText.text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;

        if (isProgrammerMode) UpdateSystemsValues();
    }

    void UpdateSystemsValues()
    {

        try
        {
            long val = Convert.ToInt64(currentInput, currentBase);

            sysTexts["HEX"].text = Convert.ToString(val, 16).ToUpper();
            sysTexts["DEC"].text = val.ToString();
            sysTexts["OCT"].text = Convert.ToString(val, 8);
            sysTexts["BIN"].text = Convert.ToString(val, 2);
        }
        catch
        {
            foreach (var txt in sysTexts.Values) txt.text = "0";
        }
    }
    void ApplyImmediateOperation(string op)
    {
        Calculate();

        if (!double.TryParse(currentInput.Replace(",", "."), out double num)) return;

        switch (op)
        {
            case "%": num = num / 100; break;
            case "1/x": num = 1 / num; break;
            case "x²": num = num * num; break;
            case "√x": if (num >= 0) num = Mathf.Sqrt((float)num); break;
            case "+/-": num = -num; break;
        }

        currentInput = num.ToString();
    }
    void HandleNegative()
    {
        if (string.IsNullOrEmpty(currentInput))
        {
            currentInput = "-";
            UpdateDisplay();
            return;
        }

        int lastOpIndex = -1;
        char[] operations = { '+', '-', '*', '/' };

        for (int i = currentInput.Length - 1; i > 0; i--)
        {
            bool isOp = false;
            foreach (char op in operations) if (currentInput[i] == op) isOp = true;

            if (isOp)
            {
                char prevChar = currentInput[i - 1];
                bool prevIsOp = false;
                foreach (char op in operations) if (prevChar == op) prevIsOp = true;

                if (!prevIsOp)
                {
                    lastOpIndex = i;
                    break;
                }
            }
        }

        string prefix = "";
        string lastNumber = "";

        if (lastOpIndex == -1)
        {
            lastNumber = currentInput;
        }
        else
        {
            prefix = currentInput.Substring(0, lastOpIndex + 1);
            lastNumber = currentInput.Substring(lastOpIndex + 1); 
        }

        if (lastNumber.StartsWith("-"))
        {
            lastNumber = lastNumber.Substring(1); 
        }
        else if (!string.IsNullOrEmpty(lastNumber))
        {
            lastNumber = "-" + lastNumber; 
        }
        else if (string.IsNullOrEmpty(lastNumber) && lastOpIndex != -1)
        {
            lastNumber = "-";
        }

        currentInput = prefix + lastNumber;
        UpdateDisplay();
    }

    void Calculate()
    {
        try
        {
            string expr = currentInput.Replace(",", ".");

            if (isProgrammerMode && currentBase != 10)
            {
                string converted = ConvertExpressionToDecimal(expr);
                DataTable dt = new DataTable();
                var res = dt.Compute(converted, "");
                long val = Convert.ToInt64(res);
                currentInput = Convert.ToString(val, currentBase).ToUpper();
            }
            else
            {
                DataTable dt = new DataTable();
                var res = dt.Compute(expr, "");
                currentInput = res.ToString();
            }
        }
        catch
        {
            currentInput = "Error";
        }
    }
    string ConvertExpressionToDecimal(string expr)
    {
        string result = "";
        string number = "";

        foreach (char c in expr)
        {
            if (char.IsLetterOrDigit(c))
            {
                number += c;
            }
            else
            {
                if (number != "")
                {
                    long val = Convert.ToInt64(number, currentBase);
                    result += val.ToString();
                    number = "";
                }
                result += c;
            }
        }

        if (number != "")
        {
            long val = Convert.ToInt64(number, currentBase);
            result += val.ToString();
        }

        return result;
    }


    Text CreateTextElement(string initText, int maxSize, Transform parent)
    {
        GameObject go = new GameObject("Label", typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        Text t = go.GetComponent<Text>();
        t.font = font;
        t.text = initText;
        t.color = Color.white;
        t.resizeTextForBestFit = true;
        t.resizeTextMaxSize = maxSize;
        t.alignment = TextAnchor.MiddleLeft;
        RectTransform textRect = t.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero; textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(0, 0); textRect.offsetMax = new Vector2(0, 0);
        return t;
    }

    void OnRectTransformDimensionsChange() => UpdateLayout();

    public void UpdateLayout()
    {
        if (gridLayout == null || gridRect == null) return;
        float cols = gridLayout.constraintCount;
        float rows = isProgrammerMode ? 6 : 6;

        float w = (gridRect.rect.width - gridLayout.padding.left - gridLayout.padding.right - (gridLayout.spacing.x * (cols - 1))) / cols;
        float h = (gridRect.rect.height - gridLayout.padding.top - gridLayout.padding.bottom - (gridLayout.spacing.y * (rows - 1))) / rows;
        gridLayout.cellSize = new Vector2(w, h);
    }
}