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

    public InputField input;
    public GameObject wp;
    void Start()
    {
        
    }

    void Update()
    {
        if (state == State.Off)
        { 
            black.SetActive(true);
            login.SetActive(false);
            apps.SetActive(false);
            sys.SetActive(false);
        }
        if (state == State.Lock)
        {
            black.SetActive(false);
            login.SetActive(true);
            apps.SetActive(false);
            sys.SetActive(false);
        }
        if (state == State.Unlock)
        {
            black.SetActive(false);
            login.SetActive(false);
            apps.SetActive(true);
            sys.SetActive(true);
        }
    }
}
