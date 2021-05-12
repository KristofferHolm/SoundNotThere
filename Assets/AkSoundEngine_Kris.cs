using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AkSoundEngine_Kris
{
    public static AudioClip GetClipFromString(string sound)
    {
        throw new NotImplementedException();
    }

    public static void PostEvent(string sound, GameObject obj)
    {
        AudioSource source;
        if (!obj.TryGetComponent<AudioSource>(out source))
        {
            source = obj.AddComponent<AudioSource>();
        }
        source.clip = GetClipFromString(sound);
    }
}
