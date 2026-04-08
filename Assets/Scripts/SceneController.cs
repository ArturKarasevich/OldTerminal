using System.Collections;
using TMPro;
using UnityEngine;

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

    void Start()
    {
        prologue.SetActive(characterTextGO);
        characterText.text = "";
        prologue.SetActive(false);
        characterTextGO.SetActive(false);
        StartCoroutine(mainCoroutine());
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
}
