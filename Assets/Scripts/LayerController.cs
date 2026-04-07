using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class LayerController : MonoBehaviour
{
    public List<GameObject> apps = new List<GameObject>();

    public Texture2D horizontalCursor;
    public Texture2D verticalCursor;
    public Texture2D diagonalCursor;
    void Start()
    {

    }

    int updateCounter = 0;
    void Update()
    {
        if (updateCounter < 10) { updateCounter++; return; }
        updateCounter = 0;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            AppLayer app = child.GetComponent<AppLayer>();
            if (app != null)
            {
                if (!apps.Contains(child.gameObject))
                {
                    apps.Add(child.gameObject);
                    apps[i].GetComponent<AppLayer>().OnSetCursors(horizontalCursor, verticalCursor, diagonalCursor);
                }
            }
        }
        for (int i = 0; i < apps.Count; i++)
        {
            if (apps[i] == null)
                apps.RemoveAt(i);
        }
        for (int i = 0; i < apps.Count; i++)
        {
            apps[i].transform.SetSiblingIndex(i);
            apps[i].GetComponent<AppLayer>().layer = i;
            apps[i].GetComponent<AppLayer>().active = (i == apps.Count - 1);
        }
    }

    public void ChangeLayerActive(int layer)
    {
        if (layer == apps.Count - 1)
        {
            apps.Insert(0, apps[apps.Count - 1]);
            apps.RemoveAt(apps.Count - 1);
            apps[0].SetActive(false);
        }
        else
        {
            GameObject app = apps[layer];
            apps.RemoveAt(layer);
            apps.Add(app);
            app.SetActive(true);
        }
    }

    public void SetLayerInactive(int layer)
    {
        apps.Insert(0, apps[layer]);
        apps.RemoveAt(layer);
        apps[0].SetActive(false);
    }

    public void SetLayerActive(int layer)
    {
        GameObject app = apps[layer];
        apps.RemoveAt(layer);
        apps.Add(app);
        app.SetActive(true);
    }
}
