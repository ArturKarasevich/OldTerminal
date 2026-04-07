using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class LaptopClicks : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
{
    public TMP_Text textMeshPro;

    public event Action<string> OnLineClicked;
    public event Action<int> OnLineDragStart;
    public event Action<int, int> OnLineDragged; 
    public event Action OnDragEnd;

    private int draggedLine = -1;

    private void Awake()
    {
        textMeshPro.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        textMeshPro.ForceMeshUpdate();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            textMeshPro.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        TMP_TextInfo textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.lineCount; i++)
        {
            TMP_LineInfo line = textInfo.lineInfo[i];

            float lineTop = line.ascender;
            float lineBottom = line.descender;

            if (localPoint.y <= lineTop && localPoint.y >= lineBottom)
            {
                string lineText = GetLineText(textMeshPro, line);
                OnLineClicked?.Invoke(lineText);
                break;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        textMeshPro.ForceMeshUpdate();
        draggedLine = GetLineAt(eventData);

        if (draggedLine != -1)
            OnLineDragStart?.Invoke(draggedLine);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedLine == -1) return;

        textMeshPro.ForceMeshUpdate();
        int overLine = GetLineAt(eventData);

        if (overLine != -1 && overLine != draggedLine)
        {
            OnLineDragged?.Invoke(draggedLine, overLine);
            draggedLine = overLine;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnDragEnd?.Invoke();
        draggedLine = -1;
    }

    private int GetLineAt(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            textMeshPro.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        var info = textMeshPro.textInfo;

        for (int i = 0; i < info.lineCount; i++)
        {
            var line = info.lineInfo[i];
            if (localPoint.y <= line.ascender && localPoint.y >= line.descender)
                return i;
        }
        return -1;
    }

    string GetLineText(TMP_Text textMeshPro, TMP_LineInfo line)
    {
        textMeshPro.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        int first = line.firstCharacterIndex;
        int last = first + line.characterCount;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = first; i < last; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            sb.Append(charInfo.character);
        }

        return sb.ToString();
    }

    public int GetClickedLineIndex(PointerEventData eventData)
    {
        textMeshPro.ForceMeshUpdate();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            textMeshPro.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        var textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.lineCount; i++)
        {
            var line = textInfo.lineInfo[i];

            if (localPoint.y <= line.ascender && localPoint.y >= line.descender)
            {
                return i;
            }
        }

        return -1;
    }



}
