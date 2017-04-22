using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwitch : MonoBehaviour {

    Ray ray;
    RaycastHit hit;
    public float Range;
    public string SoundBit;
    public GameObject AudioSphere;
    float RightClickCD = 0;
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
                print(hit.collider.tag);
                hit.transform.GetComponent<SoundHolder>().LookedAt();
                if (Input.GetButtonDown("LeftClick"))
                    PlaySound(hit.transform.gameObject);
                if (Input.GetButtonDown("RightClick"))
                    SwitchSound(hit.collider.GetComponent<SoundHolder>());
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
        if (RightClickCD > 0)
            RightClickCD -= Time.deltaTime;
    }

    private void ListenInHand()
    {
        if (RightClickCD > 0)
            return;
        else
            RightClickCD = 1.0f;
        AkSoundEngine.PostEvent(SoundBit, gameObject);
       
    }

    private void SwitchSound(SoundHolder SH)
    {
        if (RightClickCD > 0)
            return;
        else
            RightClickCD = 2.0f;

        //sphere from papfigur
        StartCoroutine(AnimateAudioSphere(SH.SoundBit,hit.point,transform.position));
        //sphere from player
        StartCoroutine(AnimateAudioSphere(SoundBit,transform.position, hit.point));
        if (SH.Completed)
            return;
        string temp = SoundBit;
        SoundBit = SH.SoundBit;
        SH.SoundBit = temp;
        if (SH.SoundBit == SH.name)
            SH.Complete();
    }

    private IEnumerator AnimateAudioSphere(string sound, Vector3 from, Vector3 to)
    {
        float t = 0;
        GameObject sphere = Instantiate<GameObject>(AudioSphere);
        //sphere.transform.position = from;
        AkSoundEngine.PostEvent(sound, sphere);
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            sphere.transform.position = Vector3.Lerp(from, to, t);
            yield return null;

        }
        Destroy(sphere);
        yield return null;
    }

    private void PlaySound(GameObject go)
    {
        go.GetComponent<SoundHolder>().PlaySound();
    }
}
