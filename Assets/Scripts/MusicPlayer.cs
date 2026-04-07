using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; 

public class MusicPlayer : MonoBehaviour, IPointerDownHandler
{
    [System.Serializable]
    public class Track
    {
        public string title;
        public string artist;
        public AudioClip clip;
    }

    [Header("Data")]
    public List<Track> playlist;
    private int currentTrackIndex = 0;
    private bool isPlayerClicked = false;

    [Header("UI References")]
    public GameObject playerPanel; 
    public TextMeshProUGUI trackTitleText;
    public TextMeshProUGUI artistText;
    public Slider progressSlider;
    public Slider volumeSlider;
    public TextMeshProUGUI playPauseText;

    [Header("Audio")]
    public AudioSource audioSource;
    private bool isDragging = false;

    void Start()
    {
        LoadTrack(0);
        SetVolume();
    }

    void Update()
    {
        if (playerPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!isPlayerClicked && !IsPointerOverPlayer())
            {
                ClosePlayer();
            }
            isPlayerClicked = false; 
        }

        if (audioSource.isPlaying && !isDragging && progressSlider != null)
        {
            progressSlider.value = audioSource.time / audioSource.clip.length;
            if (audioSource.time >= (audioSource.clip.length - 0.1f))
            {
                NextTrack();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPlayerClicked = true;
    }

    private bool IsPointerOverPlayer()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var res in results)
        {
            if (res.gameObject.transform.IsChildOf(this.transform)) return true;
        }
        return false;
    }

    public void TogglePlayer()
    {
        bool isActive = playerPanel.activeSelf;
        playerPanel.SetActive(!isActive);

        if (!isActive) transform.SetAsLastSibling();
    }

    public void ClosePlayer()
    {
        playerPanel.SetActive(false);
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying) audioSource.Pause();
        else audioSource.UnPause();
        UpdatePlayPauseUI();
    }

    public void LoadTrack(int index)
    {
        if (playlist.Count == 0) return;
        currentTrackIndex = index;
        audioSource.clip = playlist[index].clip;
        trackTitleText.text = playlist[index].title;
        artistText.text = playlist[index].artist;
        audioSource.Play();
        UpdatePlayPauseUI();
    }

    public void NextTrack() { LoadTrack((currentTrackIndex + 1) % playlist.Count); }
    public void PreviousTrack() { LoadTrack(currentTrackIndex == 0 ? playlist.Count - 1 : currentTrackIndex - 1); }
    public void SetVolume() { audioSource.volume = volumeSlider.value; }
    public void OnSliderDragStart() { isDragging = true; }
    public void OnSliderDragEnd() { isDragging = false; audioSource.time = progressSlider.value * audioSource.clip.length; }

    void UpdatePlayPauseUI() { if (playPauseText) playPauseText.text = !audioSource.isPlaying ? "Пауза" : "Воспроизведение"; }
}