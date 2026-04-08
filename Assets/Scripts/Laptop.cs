using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Laptop : MonoBehaviour
{
    public enum State
    {
        Off,
        Lock,
        Unlock
    }
    public State state = State.Off;

    public GameObject black;
    public GameObject login;
    public GameObject apps;
    public GameObject sys;
    public GameObject desktop;

    public MusicPlayer player;

    [Header("Password")]
    public TMP_InputField input;
    public GameObject wp;
    public GameObject rp;
    public int password;
    public bool loggedIn = false;

    [Header("Power Btn")]
    public GameObject powerLamp;
    private float _timer = 0f;
    public float delay = 2f;
    public bool btnFlag = false;
    public GameObject starting;

    void Start()
    {
        loggedIn = false;
        wp.SetActive(false);
        rp.SetActive(false);
        starting.SetActive(false);
    }

    void Update()
    {
        if (state == State.Off)
        { 
            black.SetActive(true);
            login.SetActive(false);
            apps.SetActive(false);
            sys.SetActive(false);
            desktop.SetActive(false);
        }
        if (state == State.Lock)
        {
            black.SetActive(false);
            login.SetActive(true);
            apps.SetActive(false);
            sys.SetActive(false);
            desktop.SetActive(false);
        }
        if (state == State.Unlock)
        {
            black.SetActive(false);
            login.SetActive(false);
            apps.SetActive(true);
            sys.SetActive(true);
            desktop.SetActive(true);
        }

        if (state == State.Off)
        {
            _timer += Time.deltaTime;
            if (_timer >= delay)
            {
                powerLamp.SetActive(!powerLamp.activeSelf);
                _timer = 0;
            }
        }
        else
        {
            powerLamp.SetActive(false);
        }
    }

    public void Login()
    {
        if (input.text.ToString() == password.ToString())
        {
            wp.SetActive(false);
            rp.SetActive(true);
            loggedIn = true;
            StartCoroutine(loginDelay());   
        }
        else
        {
            wp.SetActive(true);
            rp.SetActive(false);
        }
    }

    public void Pwr() 
    {
        if (btnFlag) return;
        if (!loggedIn)
        {
            if (state != State.Off)
            {
                player.Pause();
                state = State.Off;
            }
            else
            {
                StartCoroutine(pwrOnDelay());
            }
        }
        else
        {
            if (state != State.Off)
            {
                player.Pause();
                state = State.Off;
            }
            else
            {
                player.Play();
                state = State.Unlock;
            }
        }
    }

    IEnumerator loginDelay()
    {
        btnFlag = true;
        yield return new WaitForSeconds(3);
        state = State.Unlock;
        rp.SetActive(false);
        btnFlag = false;
    }

    IEnumerator pwrOnDelay()
    {
        starting.SetActive(true);
        btnFlag = true; 
        yield return new WaitForSeconds(5);
        state = State.Lock;
        btnFlag = false;
        starting.SetActive(false);
    }
}
