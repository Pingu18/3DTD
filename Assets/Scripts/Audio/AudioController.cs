using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public void PlaySound(string sound, GameObject parent)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s != null)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            if (parent != null)
            {
                soundObject.AddComponent<FollowObject>();
                soundObject.GetComponent<FollowObject>().toFollow = parent;
            }

            audioSource.volume = s.volume;
            audioSource.clip = s.clip;
            audioSource.playOnAwake = s.playOnAwake;
            audioSource.loop = s.loop;
            audioSource.Play();

            Destroy(soundObject, 2f);
        }
        else
        {
            print("Could not find audio clip: " + sound);
        }
    }
}
