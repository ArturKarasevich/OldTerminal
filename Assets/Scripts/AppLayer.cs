using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEditor;
using TMPro;

public class AppLayer : MonoBehaviour
{
    public int layer = 0;
    public bool active;
    public bool fullscreen;
    GameObject detectPanel;
    public Sprite circleSprite; 
    
    public Texture2D horizontalCursor;
    public Texture2D verticalCursor;
    public Texture2D diagonalCursor;

    private bool cursorsSet = false;

    public Vector2 lastOffsetMin = Vector2.zero;
    public Vector2 lastOffsetMax = Vector2.zero;

    public Vector2 appMinSize = new Vector2(200, 150);

    public Vector2 anchoredPosition = Vector2.zero;
    public Vector2 sizeDelta = Vector2.zero;
    public Color color = Color.black;

    public Sprite background;
    public Font font;
    public TMP_FontAsset tmp_font;

    public enum AppType 
    {
        None,
        Calculator,
        Messenger,
        Notepad
    }
    [Header("Set Script")]
    public bool manualSet = false;
    [ShowIf(nameof(manualSet))]
    public AppType appType;
    [ShowIf(nameof(appType), AppType.Calculator)]
    public GameObject buttonPrefab;
    [ShowIf(nameof(appType), AppType.Messenger)]
    public GameObject buttonPrefab2;


    private void Awake()
    {
        if (!manualSet) appType = AppType.None;
        RectTransform rect = transform.GetComponent<RectTransform>();
        sizeDelta = rect.sizeDelta;
        anchoredPosition = rect.anchoredPosition;

    }

    void Start()
    {
        if (appType == AppType.Calculator) 
        {
            if (GetComponentInChildren <Calculator>() == null)
            {
                GameObject panel = new GameObject(
                    "Panel",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image),
                    typeof(Calculator)
                );

                panel.transform.SetParent(transform, false);

                var Rect = panel.GetComponent<RectTransform>();
                Rect.anchorMin = Vector2.zero;
                Rect.anchorMax = Vector2.one;
                Rect.offsetMin = new Vector2(0, 0);  
                Rect.offsetMax = new Vector2(0, -20);

                var calculator = panel.GetComponent<Calculator>();
                calculator.appLayer = GetComponent<AppLayer>();
                calculator.buttonPrefab = buttonPrefab;
                calculator.font = font;

                var image = panel.GetComponent<Image>();
                image.sprite = background;
                image.color = color;
            }
        }
        else if (appType == AppType.Messenger)
        {
            if (GetComponentInChildren<Messenger>() == null)
            {
                GameObject panel = new GameObject(
                    "Panel",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image),
                    typeof(Messenger)
                );

                panel.transform.SetParent(transform, false);

                var Rect = panel.GetComponent<RectTransform>();
                Rect.anchorMin = Vector2.zero;
                Rect.anchorMax = Vector2.one;
                Rect.offsetMin = new Vector2(0, 0);
                Rect.offsetMax = new Vector2(0, -20);

                var messenger = panel.GetComponent<Messenger>();
                messenger.appLayer = GetComponent<AppLayer>();
                //messenger.buttonPrefab = buttonPrefab2;
                messenger.font = tmp_font;

                var image = panel.GetComponent<Image>();
                image.sprite = background;
                image.color = color;
            }
        }
        else if (appType == AppType.Notepad)
        {
            if (GetComponentInChildren<Notepad>() == null)
            {
                GameObject panel = new GameObject(
                    "Panel",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image),
                    typeof(Notepad)
                );

                panel.transform.SetParent(transform, false);

                var Rect = panel.GetComponent<RectTransform>();
                Rect.anchorMin = Vector2.zero;
                Rect.anchorMax = Vector2.one;
                Rect.offsetMin = new Vector2(0, 0);
                Rect.offsetMax = new Vector2(0, -20);

                var messenger = panel.GetComponent<Notepad>();
                messenger.appLayer = GetComponent<AppLayer>();
                //messenger.buttonPrefab = buttonPrefab2;
                messenger.font = tmp_font;

                var image = panel.GetComponent<Image>();
                image.sprite = background;
                image.color = color;
            }
        }

        detectPanel = new GameObject("OverlayPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));

        detectPanel.transform.SetParent(transform, false);

        RectTransform rect = detectPanel.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0, 0); 
        rect.anchorMax = new Vector2(1, 1); 
        rect.offsetMin = Vector2.zero;   
        rect.offsetMax = Vector2.zero;

        Image img = detectPanel.GetComponent<Image>();
        img.color = new Color(0,0,0,0.001f);

        Button childButton = detectPanel.GetComponent<Button>();

        childButton.onClick.AddListener(OnDetectClicked);

        CreateTopPanel();
    }

    public void OnSetCursors(Texture2D h, Texture2D v, Texture2D d)
    {
        if (cursorsSet) { return; }
        horizontalCursor = h;
        verticalCursor = v;
        diagonalCursor = d;
        CreateResizeHandles();
        cursorsSet = true;
    }
    private void CreateResizeHandles()
    {
        RectTransform myRect = GetComponent<RectTransform>();

        CreateHandle("Resize_Right", new Vector2(1, 0), new Vector2(1, 1), new Vector2(8, -20),
            new Vector2(1, 0.5f), WindowResizer.ResizeEdge.Right);

        CreateHandle("Resize_Bottom", new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 8),
            new Vector2(0.5f, 0), WindowResizer.ResizeEdge.Bottom);

        CreateHandle("Resize_BottomRight", new Vector2(1, 0), new Vector2(1, 0), new Vector2(10, 10),
            new Vector2(1, 0), WindowResizer.ResizeEdge.BottomRight);
    }

    private void CreateHandle(string name, Vector2 min, Vector2 max, Vector2 size, Vector2 pivot, WindowResizer.ResizeEdge edge)
    {
        GameObject handle = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(WindowResizer));
        handle.transform.SetParent(this.transform, false);

        RectTransform rt = handle.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.sizeDelta = size;
        rt.pivot = pivot;

        if (edge == WindowResizer.ResizeEdge.Right) rt.anchoredPosition = new Vector2(0, -10);

        handle.GetComponent<Image>().color = Color.clear;

        WindowResizer resizer = handle.GetComponent<WindowResizer>();
        resizer.Setup(GetComponent<RectTransform>(), new Vector2(200, 150), edge, horizontalCursor, verticalCursor, diagonalCursor);
    }

    public void CreateTopPanel()
    {
        GameObject panelObj = new GameObject("HeaderPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(WindowDragger));
        panelObj.transform.SetParent(this.transform, false);

        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(0.5f, 1); 

        panelRect.sizeDelta = new Vector2(0, 20); 
        panelRect.anchoredPosition = Vector2.zero; 

        panelObj.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1f); 

        HorizontalLayoutGroup layout = panelObj.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(5, 5, 2, 2); 
        layout.spacing = 10;                        
        layout.childAlignment = TextAnchor.MiddleRight;
        layout.childControlHeight = true;
        layout.childControlWidth = false; 
        layout.childForceExpandWidth = false;

        panelObj.GetComponent<WindowDragger>().windowTransform = transform.gameObject.GetComponent<RectTransform>();
        panelObj.GetComponent<WindowDragger>().enabled = true;

        CreateCircleButton(panelObj.transform, 0);
        GameObject hide = CreateCircleButton(panelObj.transform, 1);
        hide.GetComponent<Button>().onClick.AddListener(Hide);
        GameObject full = CreateCircleButton(panelObj.transform, 2);
        full.GetComponent<Button>().onClick.AddListener(Fullscreen);
    }

    GameObject CreateCircleButton(Transform parent, int index)
    {
        GameObject btnObj = new GameObject("Button_" + index, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(parent, false);

        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(16, 16); 

        Image img = btnObj.GetComponent<Image>();
        img.sprite = circleSprite;

        if (index == 0) img.color = Color.red;
        else if (index == 1) img.color = Color.yellow;
        else img.color = Color.green;
        return btnObj;
    }

    void Update()
    {
        if (!fullscreen)
        {
            RectTransform rect = transform.GetComponent<RectTransform>();
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
        }

        if (!active)
        {
            detectPanel.SetActive(true);
        }
        else
        {
            detectPanel.SetActive(false);
        }
    }

    public void OnDetectClicked()
    {
        LayerController layerController = GetComponentInParent<LayerController>();
        if (layerController != null)
        {
            layerController.ChangeLayerActive(layer);
        }
    }

    public void Hide()
    {
        LayerController layerController = GetComponentInParent<LayerController>();
        if (layerController != null)
        {
            layerController.SetLayerInactive(layer);
        }
    }

    public void Show()
    {
        LayerController layerController = GetComponentInParent<LayerController>();
        if (layerController != null)
        {
            layerController.SetLayerActive(layer);
        }
    }

    public void Fullscreen()
    {
        Show();
        RectTransform rect = transform.GetComponent<RectTransform>();
        if (fullscreen)
        {
            fullscreen = false;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.offsetMin = lastOffsetMin;
            rect.offsetMax = lastOffsetMax;
        }
        else
        {
            fullscreen = true;
            lastOffsetMin = rect.offsetMin;
            lastOffsetMax = rect.offsetMax;
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = new Vector2(0, 40);
            rect.offsetMax = new Vector2(0, 0);

        }
    }
}
