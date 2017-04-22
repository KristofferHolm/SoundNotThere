using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwitch : MonoBehaviour {

    Ray ray;
    RaycastHit hit;
    public float Range;
    public string SoundBit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        ray.direction = transform.forward * Range;
        ray.origin = transform.position;
       
        if (Physics.Raycast(ray, out hit, Range))
        {
            if (hit.collider.tag == "PapFigur")
            {
                hit.transform.GetComponent<SoundHolder>().LookedAt();
                if (Input.GetButtonDown("LeftClick"))
                    PlaySound(hit.transform.gameObject);
                if (Input.GetButtonDown("RightClick"))
                    SwitchSound();
            }
            else if (Input.GetButtonDown("RightClick"))
            {
                ListenInHand();
            }
        }
        else if (Input.GetButtonDown("RightClick"))
        {
            ListenInHand();
        }

    }

    private void ListenInHand()
    {
        AkSoundEngine.PostEvent(SoundBit, gameObject);
       
    }

    private void SwitchSound()
    {
        print("Switch Sound");
    }

    private void PlaySound(GameObject go)
    {
        go.GetComponent<SoundHolder>().PlaySound();
    }
}
