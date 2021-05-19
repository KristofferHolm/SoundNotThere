using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AkSoundEngine
{
    public static void PostEvent(SoundBoard.Sound sound, GameObject obj)
    {
        AudioSource source;
        if (!obj.TryGetComponent<AudioSource>(out source))
        {
            source = obj.AddComponent<AudioSource>();
            SetupAudioSource(ref source);
        }
        source.clip = SoundBoard.Instance.GetAudioClip(sound);
        source.Play();
    }

    public static void SetupAudioSource(ref AudioSource AS)
    {
        AS.spatialBlend = 1f;
        AS.playOnAwake = false;
    }
}
