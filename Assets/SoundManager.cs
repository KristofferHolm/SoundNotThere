using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    List<GameObject> ListOfGO;
	// Use this for initialization
	void Start () {
        ListOfGO = new List<GameObject>();
        ListOfGO.AddRange(GameObject.FindGameObjectsWithTag("Papfigur"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
