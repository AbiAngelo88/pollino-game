using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper
{
    public enum Sounds {
        Fargo,
        Menu, Level,
        Win, Loose,
        VolumeChange, ButtonClick, CountDownScore, Trophey,
        PickedChestnut,
        CyclistRun, CyclistJump,
        PyromaniacHurt, PyromaniacDestroy,
        FriendSave,
        AiDestroy,
        Wind, Water, Birds, Fox, Boar
    }

    public delegate void PlaySound(Sounds name);
    public static PlaySound PlayOneShotEmitter;
    public static PlaySound PlayBackGroundEmitter;

    public delegate void PauseBackGroundSound();
    public static PauseBackGroundSound PauseBackGroundEmitter;


    private static Dictionary<Sounds, float> delayedSounds = new Dictionary<Sounds, float>() {
        { Sounds.CyclistRun, 0.5f }
    };

    private static Dictionary<Sounds, float> timeCounter = new Dictionary<Sounds, float>();

    public static void PlayBackGroundMusic(Sounds name)
    {
        PlayBackGroundEmitter?.Invoke(name);
    }

    public static void PlayOneShotSound(Sounds name)
    {
        if(CanPlaySound(name))
            PlayOneShotEmitter?.Invoke(name);
    }

    private static bool CanPlaySound(Sounds name)
    {
        float delay;

        if (delayedSounds.TryGetValue(name, out delay))
        {
            if (timeCounter.ContainsKey(name))
            {
                float lastTimePlayed = timeCounter[name];
                if ((lastTimePlayed + delay) < Time.time)
                {
                    timeCounter[name] = Time.time;
                    return true;
                }
                else
                    return false;

            }
            else
            {
                timeCounter.Add(name, Time.time);
                return true;
            }
        }
        else
            return true;
    }

}



[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    public AudioHelper.Sounds name;

    public void SetAudioClip(AudioClip audioClip)
    {
        this.audioClip = audioClip;
    }

    public AudioClip GetAudioClip()
    {
        return this.audioClip;
    }

    public void SetName(AudioHelper.Sounds name)
    {
        this.name = name;
    }

    public AudioHelper.Sounds SetName()
    {
        return this.name;
    }

}
