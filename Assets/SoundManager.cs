using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public float minWait, maxWait;
    GameObject Doren, SpriteA, SpriteB;
    List<GameObject> ListOfGO;
    // Use this for initialization
    void Start()
    {
        SpriteA = GameObject.Find("SpriteA");
        SpriteB = GameObject.Find("SpriteB");
        SpriteB.SetActive(false);
        Doren = GameObject.Find("Dor");
        Doren.SetActive(false);
        ListOfGO = new List<GameObject>();
        ListOfGO.AddRange(GameObject.FindGameObjectsWithTag("PapFigur"));
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
        SpriteA.SetActive(false);
        SpriteB.SetActive(true);
        AkSoundEngine.PostEvent("BGM_Play", gameObject);
        StartCoroutine(StartRandomSound());
    }

    private IEnumerator StartRandomSound()
    {
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
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
        AkSoundEngine.PostEvent("BGM_Stop", gameObject);
        Doren.SetActive(true);
        Doren.GetComponent<SoundHolder>().Banken();
        Doren.tag = "PapFigur";
    }
}
