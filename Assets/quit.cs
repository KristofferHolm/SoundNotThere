using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class quit : MonoBehaviour {


    PlayerLook playerLook;
    // Use this for initialization
    void Start () {
        var playLook = FindObjectOfType<PlayerLook>();
        if (playLook != null)
        {
            playerLook = playLook; 
        }
        GetComponent<Image>().enabled = false;
    }
    bool quitScreen = false;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitScreen = !quitScreen;
            GetComponent<Image>().enabled = quitScreen;
            if(playerLook != null) 
                playerLook.Active = !quitScreen;
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
