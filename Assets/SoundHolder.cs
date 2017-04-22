using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolder : MonoBehaviour {

    public float highlightCD = 0f;
    public float HighLightLookAtSeconds = 0.1f;
    bool highLighted = false;
    Vector3 originScale;
    public bool Spammable = false;
    bool ready2Play = true;
    float animationCD = 0;
    float animationTime = 1;
    public string SoundBit;
    public Material MatSound, MatMute;
    public Mesh MeshSound, MeshMute;
    MeshFilter meshFilt;
    MeshRenderer meshRend;
	// Use this for initialization
	void Start () {
        originScale = transform.localScale;
        meshRend = GetComponent<MeshRenderer>();
        meshRend.materials[0] = MatMute;
        meshFilt = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
        if (highlightCD > 0)
        {
            highlightCD -= Time.deltaTime;
            if (highlightCD < 0)
            {
                highLighted = false;
                RefreshHighlight();
            }
        }
        if (animationCD > 0)
        {
            animationCD -= Time.deltaTime;
            if (animationCD < 0)
            {
                ready2Play = true;

            }
        }
        
    }

    public void LookedAt()
    {
        highlightCD = HighLightLookAtSeconds;
        highLighted = true;
        RefreshHighlight();
    }
    void RefreshHighlight()
    {
        //Insert Glow Effect
        if (highLighted)
            transform.localScale = originScale * 1.10f;
        else
            transform.localScale = originScale;
    }
    public void PlaySound()
    {
        if(Spammable)
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
        float t = animationTime;
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
