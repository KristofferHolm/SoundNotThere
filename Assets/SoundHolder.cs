using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHolder : MonoBehaviour {

    float highlightCD = 0f;
   // public float HighLightLookAtSeconds = 0.1f;
    bool highLighted = false;
    Vector3 originScale;
    bool ready2Play = true;
    float animationCD = 0;
    float targetEmissionAmount = 0;
    float EmissionAmount = 0;
    public float animationTime = 1;
    public int matINT = 0;
    [HideInInspector]
    public bool Completed = false;
    public string SoundBit;
    
    public Material MatSound, MatMute;
    public Mesh MeshSound, MeshMute;
    MeshFilter meshFilt;
    soundBoard SB;
    MeshRenderer meshRend;
    bool DOREN = false;
	// Use this for initialization
	void Start () {
        if (gameObject.name == "Dor")
            DOREN = true;
        originScale = transform.localScale;
        meshRend = GetComponent<MeshRenderer>();
        meshRend.materials[matINT] = MatMute;
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
        
        if (DOREN)
        {
            Completed = true;
            StartCoroutine(LightUp(1.0f, true));
            StartCoroutine(OpenDoor());
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<SoundManager>().RemoveMeFromList(gameObject);
            ready2Play = false;
            Completed = true;
            StartCoroutine(CompleteAnimation());
            StartCoroutine(LightUp(1.0f, true));
            //tag = "Untagged";
        }
    }

    private IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(2f);
        AkSoundEngine.PostEvent("Dor", gameObject);
        Quaternion startRot = transform.GetChild(1).localRotation;
        Quaternion endRot = transform.GetChild(1).localRotation * Quaternion.AngleAxis(-135,Vector3.up);
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / 2.5f;
            transform.GetChild(1).localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        t = 0;
        Image fadeOut = GameObject.Find("FadeOut").GetComponent<Image>();
        Color col = Color.white;
        col.a = 0;
        while (t<1)
        {
            t += Time.deltaTime;
            col.a = t;
            fadeOut.color = col;
            yield return null;
        }
        AkSoundEngine.PostEvent("Horn", gameObject);
        Image TheEnd = GameObject.Find("TheEnd").GetComponent<Image>();
        TheEnd.enabled = true;
        yield return new WaitForSeconds(3f);
        Application.Quit();
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
        
        if(ready2Play)
        {
            ready2Play = false;
            AkSoundEngine.PostEvent(SoundBit, gameObject);
            StartCoroutine(Animate());
        }
    }
    IEnumerator Animate()
    {
        Material[] mat = meshRend.materials;
        mat[matINT] = MatSound;
        meshRend.materials = mat;
        meshFilt.mesh = MeshSound;
        
        Vector3 pos = transform.position;
        float t = SB.GetsSoundLength(SoundBit);
        if(!DOREN)
        {
            while(t>0)
            { // SHAKE
                transform.position = pos + Time.deltaTime * Random.insideUnitSphere;
                t -= Time.deltaTime;
                yield return null;
            }
        }
        transform.position = pos;
        mat[matINT] = MatMute;
        meshRend.materials = mat;
        meshFilt.mesh = MeshMute;
        ready2Play = true;
        yield return null;
    }
    public void Banken()
    {
        StartCoroutine(BankeBanke());
    }

    private IEnumerator BankeBanke()
    {
        if (Completed)
            yield break;
        else
        {
            AkSoundEngine.PostEvent("Banken", gameObject);
            yield return new WaitForSeconds(Random.Range(3, 6));
            StartCoroutine(BankeBanke());
            yield return null;
        }
    }
}
