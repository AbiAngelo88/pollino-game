using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public enum Sounds { Fargo }

    public AudioClip audioClip;
    public Sounds name;

    public void SetAudioClip(AudioClip audioClip)
    {
        this.audioClip = audioClip;
    }

    public AudioClip GetAudioClip()
    {
        return this.audioClip;
    }

    public void SetName(Sounds name)
    {
        this.name = name;
    }

    public Sounds SetName()
    {
        return this.name;
    }

}
