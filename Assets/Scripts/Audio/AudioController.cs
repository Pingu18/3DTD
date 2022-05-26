using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public void PlaySound(string sound)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s != null)
        {
            audioSource.volume = s.volume;
            audioSource.clip = s.clip;
            audioSource.playOnAwake = s.playOnAwake;
            audioSource.loop = s.loop;
            audioSource.Play();
        } else
        {
            print("Could not find audio clip: " + sound);
        }
    }
}
