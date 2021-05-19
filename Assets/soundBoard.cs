using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoard : MonoBehaviour {

    public static SoundBoard Instance;
    Dictionary<Sound, SOSound.AudioID> soundList;
    public enum Sound
    {
        Null,
        Blender,
        Glimt,
        Hund,
        Kat,
        Koleskab,
        Mobiltelefon,
        Motorsav,
        Ugle,
        Vakkeur,
        Fro,
        Banken,
        Dor,
        Horn,
        Tom,
        Swap
    }
    public SOSound SOSound;
    public string[] soundText;
    public float[] soundLength;
	// Use this for initialization
	void Awake () {

        Instance = this;
        soundList = new Dictionary<Sound, SOSound.AudioID>();
        foreach (var audio in SOSound.SoundList)
        {
            soundList.Add(audio.Sound, audio);
        }
	}
	public float GetsSoundLength(Sound key)
    {
        soundList.TryGetValue(key,out var sound);
        return sound.Length;
    }
    public AudioClip GetAudioClip(Sound key)
    {
        soundList.TryGetValue(key, out var sound);
        return sound.Clip;
    }
}
