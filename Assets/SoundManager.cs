using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public float minWait, maxWait;
    List<GameObject> ListOfGO;
    // Use this for initialization
    void Start()
    {
        ListOfGO = new List<GameObject>();
        ListOfGO.AddRange(GameObject.FindGameObjectsWithTag("PapFigur"));
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void RemoveMeFromList(GameObject removeGO)
    {
        ListOfGO.Remove(removeGO);
        if (ListOfGO.Count == 0)
            EndGameScenario();
    }
    public void StartGame()
    {
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
        print("the end is not there!");
    }

}
