using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{

    public AudioSource[] tracks;
    public Slider[] sliders;

    void Awake()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            int index = i;
            if (sliders[i] !=  null )
                sliders[i].onValueChanged.AddListener((value) => ChangeVolume(index, value));
        }
    }

    void Start()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i] != null)
                sliders[i].value = tracks[i].volume;
        }
    }

    private void ChangeVolume(int index, float value)
    {
        if (tracks[index] != null)
            tracks[index].volume = value;
    }
}
