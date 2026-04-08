using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class LocalizationManager : MonoBehaviour
{
    public string defaultLanguage = "ru";
    private string currentLanguage;

    private Dictionary<string, Dictionary<string, string>> localizedText;

    public event Action OnLanguageChange;

    private void Awake()
    {
        currentLanguage = defaultLanguage;
        LoadLocalization();
    }

    private void LoadLocalization()
    {
        string localizationPath = Application.dataPath + "/localization.json";
        print("Загрузка локализации из: " + localizationPath);

        if (!File.Exists(localizationPath))
        {
            Debug.LogError("Файл локализации не найден: " + localizationPath);
            return;
        }

        string json = File.ReadAllText(localizationPath);

        localizedText = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
    }

    public string GetText(string key)
    {
        if (localizedText.ContainsKey(currentLanguage) &&
            localizedText[currentLanguage].ContainsKey(key))
        {
            return localizedText[currentLanguage][key];
        }
        if (localizedText.ContainsKey("en") &&
            localizedText["en"].ContainsKey(key) &&
            key != "name")
        {
            return localizedText["en"][key];
        }

        Debug.LogWarning("Текст не найден: " + key);
        return key;
    }

    public float GetFloat(string key)
    {
        try
        {
            if (localizedText.ContainsKey(currentLanguage) &&
                localizedText[currentLanguage].ContainsKey(key))
            {
                return float.Parse(localizedText[currentLanguage][key]);
            }
            if (localizedText.ContainsKey("en") &&
                localizedText["en"].ContainsKey(key) &&
                key != "name")
            {
                return float.Parse(localizedText["en"][key]);
            }
        }
        finally {}
        Debug.LogWarning("Текст не найден: " + key);
        return 27;
    }

    public void SetLanguage(string lang)
    {
        if (localizedText.ContainsKey(lang))
            currentLanguage = lang;
        else
            Debug.LogWarning("Язык не найден: " + lang);
        OnLanguageChange?.Invoke();
    }
    public Dictionary<string, string> GetAllLanguages()
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        if (localizedText == null)
        {
            Debug.LogWarning("Локализация не загружена!");
            return result;
        }

        foreach (var langPair in localizedText)
        {
            string langCode = langPair.Key;
            if (!langPair.Value.ContainsKey("name")) { continue; }
            string name = langPair.Value.ContainsKey("name") ? langPair.Value["name"] : langCode;
            result.Add(name, langCode);
        }

        return result;
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<LanguageWrapper> languages;

        public Dictionary<string, Dictionary<string, string>> ToDictionary()
        {
            var dict = new Dictionary<string, Dictionary<string, string>>();
            foreach (var lang in languages)
                dict.Add(lang.languageCode, lang.entries);
            return dict;
        }
    }

    [System.Serializable]
    private class LanguageWrapper
    {
        public string languageCode;
        public Dictionary<string, string> entries;
    }
}

[System.Serializable]
public class LocalizationData
{
    public Dictionary<string, Dictionary<string, string>> languages;
}