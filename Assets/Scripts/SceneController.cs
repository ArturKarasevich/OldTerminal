using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string state = "";
    public Laptop laptop;
    public bool typing = false;
    public bool space = false;
    public LocalizationManager localizationManager;
    public bool recoveryFound = false;

    public float delay = 0.2f;
    public GameObject characterTextGO;
    public TextMeshProUGUI characterText;

    public GameObject prologue;

    public bool fullScreen;

    public Image brightness;

    public Slider textSpeedSlider;
    public Slider brightnessSlider;
    public Slider textSpeedSliderSM;
    public Slider brightnessSliderSM;

    public TextMeshProUGUI gameText;
    public GameObject gameOver;

    [Header("Saves")]
    public GameObject savesGO;
    public RawImage[] saveImages;
    public GameObject[] withSaveGO;
    public GameObject[] emptySaveGO;
    public TextMeshProUGUI[] savesNames;
    public TextMeshProUGUI[] savesDates;
    public GameObject confirmSaveGo;
    public TMP_InputField saveNameInput;
    private int saveNumberTemp;
    void Start()
    {
        SaveSystem.Initialize();
        LoadSaveImages();
        GameSettings settings = SaveSystem.Load();
        localizationManager.SetLanguage(settings.lang);
        textSpeedSlider.value = settings.textSpeed;
        prologue.SetActive(characterTextGO);
        characterText.text = "";
        prologue.SetActive(false);
        characterTextGO.SetActive(false);
        StartCoroutine(AutoSave());
        if (localizationManager != null)
        {
            localizationManager.OnLanguageChange += UpdateLocale;
        }
        StartCoroutine(mainCoroutine());
        gameText.text = localizationManager.GetText("hackInstruction");
    }

    IEnumerator mainCoroutine()
    {
        yield return null;
        if (state == "")
        {
            laptop.btnFlag = true;
            laptop.state = Laptop.State.Off;
            prologue.SetActive(true);
            while (prologue.activeSelf) yield return new WaitForEndOfFrame();
            laptop.btnFlag = false;
            while (laptop.state == Laptop.State.Off) yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            while (laptop.btnFlag == true) yield return new WaitForEndOfFrame();
            characterTextGO.SetActive(true);
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech0")));
            while (typing) yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech1")));
            while (typing) yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech2")));
            while (typing) yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech3")));
            while (typing) yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech4")));
            while (typing) yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech5")));
            while (typing) yield return new WaitForEndOfFrame();
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            space = true;
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech6")));
            while (laptop.state != Laptop.State.Unlock) yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(4);
            StartCoroutine(TypeText(localizationManager.GetText("firstPasswordSpeech7")));
            while (!Input.GetKey(KeyCode.Space)) yield return new WaitForEndOfFrame();
            characterTextGO.SetActive(false);
            characterText.text = string.Empty;
            state = "freeswimming";
        }
        if (state == "freeswimming")
        {
            while (!recoveryFound) yield return new WaitForEndOfFrame();
            state = "game";

        }
    }

    public void reboot()
    {
        StartCoroutine(Reboot());
    }

    public IEnumerator Reboot()
    {
        laptop.state = Laptop.State.Off;
        laptop.btnFlag = true;
        yield return new WaitForSeconds(2);
        laptop.starting.SetActive(true);
        yield return new WaitForSeconds(4);
        laptop.state = Laptop.State.Unlock;
        laptop.btnFlag = false;
        laptop.starting.SetActive(false);
        recoveryFound = true;
    }


    private IEnumerator TypeText(string text)
    {
        typing = true;
        laptop.btnFlag = true;
        characterText.text = "";

        foreach (char c in text)
        {
            characterText.text += c;
            yield return new WaitForSeconds(delay);
            if (space && !Input.GetKey(KeyCode.Space))
            {
                space = false;
            }
            if (!space && Input.GetKey(KeyCode.Space))
            {
                characterText.text = text;
                break;
            }
        }
        space = true;
        typing = false;
        laptop.btnFlag = false;
    }


    void Update()
    {

    }
    int rez_id = 0;
    public void SetRezolution(int id)
    {
        rez_id = id;
        switch (id)
        {
            case 0:
                Screen.SetResolution(1280, 720, fullScreen);
                return;
            case 1:
                Screen.SetResolution(1336, 768, fullScreen);
                return;
            case 2:
                Screen.SetResolution(1920, 1080, fullScreen);
                return;
            case 3:
                Screen.SetResolution(2560, 1440, fullScreen);
                return;
            case 4:
                Screen.SetResolution(3840, 2160, fullScreen);
                return;
        }
    }

    public void SetFullscreen(bool full)
    {
        fullScreen = full;
        SetRezolution(rez_id);
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else { Time.timeScale = 1; }
    }
    bool brFlag = false;
    bool txFlag = false;
    public void SetBrightness()
    {
        if (!brFlag)
        {
            brightness.color = new Color(0, 0, 0, brightnessSlider.value);
            brightnessSliderSM.value = brightnessSlider.value;
            brFlag = true;
        }
        else brFlag = false;
        UpdateBrightness();
    }

    public void SetTextspeed()
    {
        if (!txFlag)
        {
            delay = textSpeedSlider.value;
            textSpeedSliderSM.value = delay;
            txFlag = true;
        }
        else txFlag = false;
        UpdateTextSpeed();
    }

    public void SetBrightnessSM()
    {
        if (!brFlag)
        {
            brightness.color = new Color(0, 0, 0, brightnessSliderSM.value);
            brightnessSlider.value = brightnessSliderSM.value;
            brFlag = true;
        }
        else brFlag = false;
    }

    public void SetTextspeedSM()
    {
        if (!txFlag)
        {
            delay = textSpeedSliderSM.value;
            textSpeedSlider.value = delay;
            txFlag = true;
        }
        else txFlag = false;
    }

    public void Quit()
    {
        Application.Quit();
    }




    public void LoadSaveImages()
    {
        for (int i = 0; i < 4; i++)
        {
            GameData data = SaveSystem.LoadGame(i + 1, false);

            if (data != null)
            {
                withSaveGO[i].SetActive(true);
                emptySaveGO[i].SetActive(false);
                savesNames[i].text = data.saveName;
                savesDates[i].text = data.saveTime;
                LoadImage(i + 1, saveImages[i]);
            }
            else
            {
                withSaveGO[i].SetActive(false);
                emptySaveGO[i].SetActive(true);
                saveImages[i].texture = null;
            }
        }
    }

    public void LoadImage(int saveNumber, RawImage rawImage)
    {
        string path;
        if (saveNumber == 0)
        {
            path = Application.persistentDataPath + "/Saves/ImageAutosave.png";
        }
        else
        {
            path = Application.persistentDataPath + $"/Saves/ImageSave{saveNumber}.png";
        }

        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData))
            {
                rawImage.texture = texture;
            }
        }
        else
        {
            rawImage.texture = null;
        }
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            GameData data = new GameData
            {
                saveName = localizationManager.GetText("autosaveName"),
                status = state
            };
            SaveSystem.AutoSave(data);
        }
    }

    public void SaveGame(int saveNumber)
    {
        saveNumberTemp = saveNumber;
        confirmSaveGo.SetActive(true);
    }

    public void ConfirmSaveGame()
    {
        GameData data = new GameData
        {
            saveName = saveNameInput.text,
            status = state
        };
        SaveSystem.SaveGame(saveNumberTemp, data);
        LoadSaveImages();
        confirmSaveGo.SetActive(false);
    }

    public void LoadGame(int loadNumber)
    {
        GameData data = SaveSystem.LoadGame(loadNumber);
        if (data != null)
        {
            Restart();
        }
    }

    public void DeleteGame(int loadNumber)
    {
        SaveSystem.DeleteGame(loadNumber);
        LoadSaveImages();
    }

    public void DeleteAutoSave()
    {
        SaveSystem.DeleteAutoSave();
        Restart();
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateTextSpeed()
    {
        GameSettings settings = SaveSystem.Load();
        settings.textSpeed = textSpeedSlider.value;
        SaveSystem.Save(settings);
    }

    public void UpdateBrightness()
    {
        GameSettings settings = SaveSystem.Load();
        settings.brightness = brightnessSlider.value;
        SaveSystem.Save(settings);
    }

    public void UpdateLocale()
    {
        GameSettings settings = SaveSystem.Load();

        string selectedLanguageKey = localizationManager.currentLanguage;

        if (selectedLanguageKey != settings.lang)
        {
            settings.lang = selectedLanguageKey;
            SaveSystem.Save(settings);
        }
    }
    int i = 0;
    public void Game (int h)
    {   
        print(h);
        print(i);
        if (i == h)
        {
            i += 1;
            gameText.text = localizationManager.GetText("nodeSynced") + $" ({i}/{7})";

        }
        else
        {
            i = 0;
            gameText.text = localizationManager.GetText("nodeFailure");
        }
        if (i == 7)
        {
            gameOver.gameObject.SetActive(true);
        }
    }
}