using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowResizer : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform windowTransform;
    public RectTransform desktopTransform;
    [SerializeField] private Vector2 minSize = new Vector2(200, 150);

    private Texture2D horizontalCursor;
    private Texture2D verticalCursor;
    private Texture2D diagonalCursor;

    public enum ResizeEdge { Right, Bottom, BottomRight }
    public ResizeEdge edge; 
    
    private Vector2 initialMousePos;

    void Start()
    {
        GameObject go = GameObject.FindWithTag("Laptop");
        if (go != null)
        {
            desktopTransform = go.GetComponent<RectTransform>();
        }
        else
            print("Ноут не найден");

    }

    public void Setup(RectTransform window, Vector2 min, ResizeEdge e, Texture2D h, Texture2D v, Texture2D d)
    {
        windowTransform = window;
        minSize = min;
        edge = e; 
        horizontalCursor = h;
        verticalCursor = v;
        diagonalCursor = d;
    }
    
    void Update()
    {
        AppLayer app = transform.gameObject.GetComponentInParent<AppLayer>();
        if (app != null)
        {
            minSize = app.appMinSize;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AppLayer app = transform.gameObject.GetComponentInParent<AppLayer>();
        if (app != null)
        {
            if (app.fullscreen) return;
        }
        SetCustomCursor(edge);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    private void SetCustomCursor(ResizeEdge edgeType)
    {
        Texture2D tex = null;
        switch (edgeType)
        {
            case ResizeEdge.Right: tex = horizontalCursor; break;
            case ResizeEdge.Bottom: tex = verticalCursor; break;
            case ResizeEdge.BottomRight: tex = diagonalCursor; break;
        }

        if (tex != null)
        {
            Vector2 hotspot = new Vector2(tex.width / 2f, tex.height / 2f);
            Cursor.SetCursor(tex, hotspot, CursorMode.ForceSoftware);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (windowTransform == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            windowTransform.parent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out initialMousePos
        );
    }

    public void OnDrag(PointerEventData eventData)
    {

        AppLayer app = transform.gameObject.GetComponentInParent<AppLayer>();
        if (app != null)
        {
            if(app.fullscreen) return;
            app.Show();
        }
        if (windowTransform == null) return;

        ResizeWindow(eventData); 
    }

    private void ResizeWindow(PointerEventData eventData)
    {
        AppLayer app = GetComponentInParent<AppLayer>();
        RectTransform desktopRT = desktopTransform; 

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(desktopRT, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos))
        {
            Vector2 maxPos = new Vector2(
                windowTransform.localPosition.x - (windowTransform.pivot.x * windowTransform.rect.width),
                windowTransform.localPosition.y + ((1 - windowTransform.pivot.y) * windowTransform.rect.height)
            );

            Vector2 newSize = windowTransform.sizeDelta;

            if (edge == ResizeEdge.Right || edge == ResizeEdge.BottomRight)
            {
                float proposedWidth = localMousePos.x - maxPos.x;
                float maxWidth = desktopRT.rect.xMax - maxPos.x;

                newSize.x = Mathf.Clamp(proposedWidth, minSize.x, maxWidth);
            }

            if (edge == ResizeEdge.Bottom || edge == ResizeEdge.BottomRight)
            {
                float proposedHeight = maxPos.y - localMousePos.y;
                float maxHeight = maxPos.y - desktopRT.rect.yMin;

                newSize.y = Mathf.Clamp(proposedHeight, minSize.y, maxHeight - 40);
            }

            app.sizeDelta = newSize;

            Vector2 newAnchoredPos = new Vector2(
                maxPos.x + (windowTransform.pivot.x * newSize.x),
                maxPos.y - ((1 - windowTransform.pivot.y) * newSize.y)
            );

            app.anchoredPosition = newAnchoredPos;
        }
    }

}