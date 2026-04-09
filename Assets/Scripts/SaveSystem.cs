using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string settingsPath = Application.persistentDataPath + "/Settings.json";
    private static string saveFolder = Application.persistentDataPath + "/Saves";
    private static string autosavePath = saveFolder + "/Autosave.json";
    private static string autosaveImagePath = saveFolder + "/ImageAutosave.png";
    private static string save1Path = saveFolder + "/Save1.json";
    private static string save2Path = saveFolder + "/Save2.json";
    private static string save3Path = saveFolder + "/Save3.json";
    private static string save4Path = saveFolder + "/Save4.json";
    private static string save1ImagePath = saveFolder + "/ImageSave1.png";
    private static string save2ImagePath = saveFolder + "/ImageSave2.png";
    private static string save3ImagePath = saveFolder + "/ImageSave3.png";
    private static string save4ImagePath = saveFolder + "/ImageSave4.png";

    public static void Save(GameSettings settings)
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(settingsPath, json);
    }

    public static GameSettings Load()
    {
        if (!File.Exists(settingsPath))
        {
            Debug.Log("Файл настроек не найден, создаю новый.");
            Save(new GameSettings());
        }

        string json = File.ReadAllText(settingsPath);
        return JsonUtility.FromJson<GameSettings>(json);
    }
    public static void Initialize()
    {
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
            Debug.Log("Создана директория сейвов: " + saveFolder);
        }
    }

    public static void SaveGame(int saveNumber, GameData data)
    {
        string savePath = "";
        string saveImagePath = "";
        switch (saveNumber)
        {
            case 1:
                savePath = save1Path;
                saveImagePath = save1ImagePath;
                break;
            case 2:
                savePath = save2Path;
                saveImagePath = save2ImagePath;
                break;
            case 3:
                savePath = save3Path;
                saveImagePath = save3ImagePath;
                break;
            case 4:
                savePath = save4Path;
                saveImagePath = save4ImagePath;
                break;
            default:
                Debug.LogError("Некорректный номер сейва: " + saveNumber);
                return;
        }
        data.saveTime = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        File.Copy(autosaveImagePath, saveImagePath, true);

        Debug.Log("Сохранено: " + savePath);
    }

    public static void AutoSave(GameData data)
    {
        data.saveTime = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(autosavePath, json);

        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(autosaveImagePath, png);
        Object.Destroy(tex);

        Debug.Log("Сохранено: " + autosavePath);
    }

    public static GameData LoadGame(int saveNumber, bool needToAutosave = true)
    {
        string savePath = "";
        string saveImagePath = "";
        switch (saveNumber)
        {
            case 1:
                savePath = save1Path;
                saveImagePath = save1ImagePath;
                break;
            case 2:
                savePath = save2Path;
                saveImagePath = save2ImagePath;
                break;
            case 3:
                savePath = save3Path;
                saveImagePath = save3ImagePath;
                break;
            case 4:
                savePath = save4Path;
                saveImagePath = save4ImagePath;
                break;
            default:
                Debug.LogError("Некорректный номер сейва: " + saveNumber);
                return null;
        }

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Файл не найден: " + savePath);
            return null;
        }
        if (needToAutosave)
        {
            File.Copy(savePath, autosavePath, true);
            File.Copy(saveImagePath, autosaveImagePath, true);
        }
        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    public static GameData LoadAutoSave()
    {
        if (!File.Exists(autosavePath))
        {
            Debug.Log("Автосейв отсутствует.");
            return null;
        }

        string json = File.ReadAllText(autosavePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    public static bool DeleteGame(int saveNumber)
    {
        string savePath = "";
        string saveImagePath = "";
        switch (saveNumber)
        {
            case 1:
                savePath = save1Path;
                saveImagePath = save1ImagePath;
                break;
            case 2:
                savePath = save2Path;
                saveImagePath = save2ImagePath;
                break;
            case 3:
                savePath = save3Path;
                saveImagePath = save3ImagePath;
                break;
            case 4:
                savePath = save4Path;
                saveImagePath = save4ImagePath;
                break;
            default:
                Debug.LogError("Некорректный номер сейва: " + saveNumber);
                return false;
        }

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Файл не найден: " + savePath);
            return false;
        }
        File.Delete(savePath);
        File.Delete(saveImagePath);
        return true;
    }
    public static bool DeleteAutoSave()
    {
        File.Delete(autosavePath);
        File.Delete(autosaveImagePath);
        return true;
    }
}

[System.Serializable]
public class GameSettings
{
    public float textSpeed = 0.02f;
    public float brightness = 0f;
    public string lang = "ru";
}

[System.Serializable]
public class GameData
{
    public string saveName;
    public string saveTime;
    public string status;
}