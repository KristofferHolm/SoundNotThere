using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class quit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Image>().enabled = false;
    }
    bool quitScreen = false;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitScreen = !quitScreen;
            if(quitScreen)
            {
                GetComponent<Image>().enabled = true;
            }
            else
            {
                GetComponent<Image>().enabled = false;
            }
        }
        if(quitScreen)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
            }
        }
	}
}
