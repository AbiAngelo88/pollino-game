using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper
{

    public delegate void PlayOneShot(Sound.Sounds name);
    public static PlayOneShot PlayOneShotEmitter;

    public static void PlayOneShotSound(Sound.Sounds name)
    {
        PlayOneShotEmitter?.Invoke(name);
    }
}
