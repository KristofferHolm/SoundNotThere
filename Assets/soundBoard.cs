using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBoard : MonoBehaviour {


    Dictionary<string, float> soundList;

    public string[] soundText;
    public float[] soundLength;
	// Use this for initialization
	void Awake () {

        soundList = new Dictionary<string, float>();

        for (int i = 0; i < soundText.Length; i++)
        {
            soundList.Add(soundText[i], soundLength[i]);
        }
	}
	public float GetsSoundLength(string key)
    {
        return soundList[key];
    }
	// Update is called once per frame
	void Update () {
		
	}
}
