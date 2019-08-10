using UnityEngine;
using System.Collections;
using Assets.Scripts.System;

public class SoundManager : MonoBehaviour
{
    public static bool SoundActive = true;

    public AudioClip TurnReadySound;
    public AudioClip CardPlaySound;
    public AudioClip DealSound;
    public AudioClip CountdownSound;
    public AudioSource AudioSource;

    private System.DateTime lastPlayedTurnSound = System.DateTime.MinValue;

    void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            SoundActive = PlayerPrefs.GetInt("Sound") == 0 ? false : true;
        }
    }

    void Update()
    {
        if (!SoundActive && AudioSource.isPlaying)
        {
            AudioSource.Stop();
        }
    }

    public void PlayCardPlaySound()
    {
        if (SoundActive && (LobbyPlayerStats.IsShown || PlayerStats.IsShown))
        {
            AudioSource.clip = CardPlaySound;
            AudioSource.Play();
        }
    }

    public void PlayDealSound()
    {
        if (SoundActive && (LobbyPlayerStats.IsShown || PlayerStats.IsShown))
        {
            AudioSource.clip = DealSound;
            AudioSource.Play();
        }

    }

    public void PlayTurnReadySound()
    {
        if (SoundActive && (LobbyPlayerStats.IsShown || PlayerStats.IsShown) && (System.DateTime.Now - lastPlayedTurnSound).Seconds > 5)
        {
            AudioSource.clip = TurnReadySound;
            AudioSource.Play();

            lastPlayedTurnSound = System.DateTime.Now;
        }
    }

    public void PlayAboutToTimeoutSound()
    {
        if (SoundActive && (LobbyPlayerStats.IsShown || PlayerStats.IsShown))
        {
            AudioSource.clip = CountdownSound;
            if (!AudioSource.isPlaying)
            {
                AudioSource.Play();
            }
        }
    }

    public void StopAboutToTimeoutSound()
    {
        AudioSource.Stop();
    }

}
