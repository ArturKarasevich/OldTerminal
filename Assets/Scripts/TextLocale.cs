using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLocale : MonoBehaviour
{
    public string LocaleName;
    LocalizationManager localizationManager;
    TextMeshProUGUI textMeshProUGUI;

    void Start()
    {
        localizationManager = GameObject.Find("SceneController").GetComponent<LocalizationManager>();
        textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        if (localizationManager != null && textMeshProUGUI != null)
        {
            localizationManager.OnLanguageChange += ChangeLocale;
            textMeshProUGUI.text = localizationManager.GetText(LocaleName);
        }
        else
        {
            Debug.LogError("LocalizationManager не найден");
        }
    }

    void ChangeLocale()
    {
        textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = localizationManager.GetText(LocaleName);
    }
}
