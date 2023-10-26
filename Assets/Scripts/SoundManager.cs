using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Make it a singleton
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [Header("Sounds")]
    [SerializeField] private AudioClip[] soundArray;
    [SerializeField] private AudioSource musicPlayer;
    private bool soundOn = true;

    public enum Sound
    {
        buttonPress,
        tileSelect,
        tileDrop,
        tileComplete,
        levelComplete,
        levelFail
    }
    public void PlaySound(Sound sound)
    {
        if (soundOn)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(soundArray[(int)sound]);
        }
    }

    public void SoundToggle()
    {
        soundOn = !soundOn;
    }

    private void Start()
    {
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    public void MusicToggle(float value)
    {
        if (value == 0)
            musicPlayer.Pause();
        else
            musicPlayer.UnPause();
    }
}
