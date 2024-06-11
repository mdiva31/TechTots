using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static BGMController instance;
    public AudioSource bgmSource;
    public float currentVolume;
    private void Awake()
    {
        if (instance != null) {
        
            Destroy(gameObject);

            return;
        }
        instance = this;
        bgmSource = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);

        }
        bgmSource.volume = PlayerPrefs.GetFloat("volume");
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", bgmSource.volume);

    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume" , volume);
        bgmSource.volume = volume;
    }
    public void ResetVolume()
    {
        bgmSource.volume = PlayerPrefs.GetFloat("volume");

    }

}
