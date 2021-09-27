using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    Dictionary<SoundClip, AudioClip> soundsDictionary = new Dictionary<SoundClip, AudioClip>();
    AudioSource aSource;

    public enum SoundClip {Jump, Hit,};

    void Awake()
    {
        aSource = GetComponent<AudioSource>();

        soundsDictionary.Add(SoundClip.Jump, (AudioClip)Resources.Load("Sounds/Jump"));
        soundsDictionary.Add(SoundClip.Hit, (AudioClip)Resources.Load("Sounds/Hit"));
    }

    public void PlaySound(SoundClip sound)
    {
        AudioClip clip;
        if (soundsDictionary.TryGetValue(sound, out clip))
        {
            aSource.PlayOneShot(clip);
        }
        else
        {
            print($"Tried To Play Nonexistant Sound: {sound}");
        }
    }
}
