﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolder : MonoBehaviour {

    float highlightCD = 0f;
   // public float HighLightLookAtSeconds = 0.1f;
    bool highLighted = false;
    Vector3 originScale;
    bool Spammable = false;
    bool ready2Play = true;
    float animationCD = 0;
    float targetEmissionAmount = 0;
    float EmissionAmount = 0;
    public float animationTime = 1;
    [HideInInspector]
    public bool Completed = false;
    public string SoundBit;
    public Material MatSound, MatMute;
    public Mesh MeshSound, MeshMute;
    MeshFilter meshFilt;
    soundBoard SB;
    MeshRenderer meshRend;
	// Use this for initialization
	void Start () {
        originScale = transform.localScale;
        meshRend = GetComponent<MeshRenderer>();
        meshRend.materials[0] = MatMute;
        meshFilt = GetComponent<MeshFilter>();
        SB = GameObject.Find("GameManager").GetComponent<soundBoard>();
        animationTime = SB.GetsSoundLength(SoundBit);
        EmissionAmount = 0;
        MatSound.SetColor("_EmissionColor", Color.white * EmissionAmount);
        MatMute.SetColor("_EmissionColor", Color.white * EmissionAmount);
    }
	
	// Update is called once per frame
	void Update () {
        if (highlightCD >= 0.0f)
        {
            highlightCD -= Time.deltaTime;
            if(highlightCD< 0.0f)
            {
                highLighted = false;
                RefreshHighlight();
            }
        }

        if (!Completed)
        {
            if (Mathf.Abs(EmissionAmount - targetEmissionAmount) > 0.01f)
            {
                if (EmissionAmount < targetEmissionAmount)
                EmissionAmount += Time.deltaTime;
            else if (EmissionAmount > targetEmissionAmount)
                EmissionAmount -= Time.deltaTime;

                MatSound.SetColor("_EmissionColor", Color.white * EmissionAmount);
                MatMute.SetColor("_EmissionColor", Color.white * EmissionAmount);
            }
        }
    }
    public void Complete()
    {
        ready2Play = false;
        Completed = true;
        StartCoroutine(CompleteAnimation());
        StartCoroutine(LightUp(1.0f, true));
        //tag = "Untagged";
    }
    private IEnumerator CompleteAnimation()
    {
        yield return new WaitForSeconds(5f); // 2 + 2 + 1 
        AkSoundEngine.PostEvent(SoundBit, gameObject);
        StartCoroutine(Animate());
    }

    private IEnumerator LightUp(float target, bool overwrite) // kun til Complete();
    {
        if (overwrite && target == 1.0f)
        {
            yield return new WaitForSeconds(2f);
            AkSoundEngine.PostEvent("Glimt", gameObject);
        }
        else if (overwrite && target == 0.0f)
            yield return new WaitForSeconds(1.0f);
        float t = 0;
        if(EmissionAmount < target)
        {
            while (EmissionAmount<target)
            {
                t += Time.deltaTime;
                EmissionAmount += Time.deltaTime;
                MatSound.SetColor("_EmissionColor", Color.white * EmissionAmount);
                MatMute.SetColor("_EmissionColor", Color.white * EmissionAmount);
                if (t > 2f || Completed && !overwrite)
                    break;
                yield return null;
            }
                       
        }
        else
        {
            while (EmissionAmount > target)
            {
                t += Time.deltaTime;
                EmissionAmount -= Time.deltaTime;
                MatSound.SetColor("_EmissionColor", Color.white * EmissionAmount);
                MatMute.SetColor("_EmissionColor", Color.white * EmissionAmount);
                if (t > 2f || Completed && !overwrite)
                    break;
                yield return null;
            }

        }
        if(overwrite && EmissionAmount > 0) // FADE OUT EXTREME CAUTION !
        {
          
            StartCoroutine(LightUp(0.0f, true));
        }
        yield return null;
    }

    public void LookedAt()
    {
        if (Completed)
            return;
        highLighted = true;
        highlightCD = 0.1f;
        RefreshHighlight();


    }
    void RefreshHighlight()
    {
        //Insert Glow Effect
        if (highLighted)
            targetEmissionAmount = 0.3f;
        else
            targetEmissionAmount = 0.0f;
    }
    public void PlaySound()
    {
      //  if (Completed)
      //      return;
        if (Spammable)
        {
            
        }
        else
        {
            if(ready2Play)
            {
                ready2Play = false;
                AkSoundEngine.PostEvent(SoundBit, gameObject);
                StartCoroutine(Animate());
            }
        }
    }
    IEnumerator Animate()
    {
        meshRend.material = MatSound;
        meshFilt.mesh = MeshSound;
        
        Vector3 pos = transform.position;
        float t = SB.GetsSoundLength(SoundBit);
        while(t>0)
        { // SHAKE
            transform.position = pos + Time.deltaTime * Random.insideUnitSphere;
            t -= Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
        meshRend.material = MatMute;
        meshFilt.mesh = MeshMute;
        ready2Play = true;
        yield return null;
    }
 
}
