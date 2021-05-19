using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour 
{
    public static SoundManager Instance;
    public float minWait, maxWait;
    GameObject Doren, SpriteA, SpriteB;
    List<GameObject> ListOfGO;
    public Action OnGameStart;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        SpriteA = GameObject.Find("SpriteA");
        SpriteB = GameObject.Find("SpriteB");
        SpriteB.SetActive(false);
        Doren = GameObject.Find("Dor");
        Doren.SetActive(false);
        ListOfGO = new List<GameObject>();
        ListOfGO.AddRange(GameObject.FindGameObjectsWithTag("PapFigur"));
        Cursor.visible = false;
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void RemoveMeFromList(GameObject removeGO)
    {
        print("hvad sker der?! " + removeGO.name);
        if (SpriteB.activeSelf)
            SpriteB.SetActive(false);
        ListOfGO.Remove(removeGO);
        if (ListOfGO.Count == 0)
            EndGameScenario();
    }
    public void StartGame()
    {
        OnGameStart?.Invoke();
        SpriteA.SetActive(false);
        SpriteB.SetActive(true);
        var aS = GetComponent<AudioSource>();
        aS.Play();
        StartCoroutine(FadeInBGM(aS));
        StartCoroutine(StartRandomSound());
    }
    private IEnumerator FadeInBGM( AudioSource audioSource)
    {
        audioSource.volume = 0f;
        yield return null;
        while (audioSource.volume < 1f)
        {
            audioSource.volume += 0.5f * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator StartRandomSound()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minWait, maxWait));
        if(ListOfGO.Count == 0)
            yield return null;
        else
        {
            GameObject temp = ListOfGO[0];
            ListOfGO.Remove(ListOfGO[0]);
            temp.GetComponent<SoundHolder>().PlaySound();
            ListOfGO.Add(temp);
            StartCoroutine(StartRandomSound());
            yield return null;
        }
    }

    public void EndGameScenario()
    {
        //AkSoundEngine.PostEvent("BGM_Stop", gameObject);
        GetComponent<AudioSource>().Stop();
        Doren.SetActive(true);
        Doren.GetComponent<SoundHolder>().Banken();
        Doren.tag = "PapFigur";
    }
}
