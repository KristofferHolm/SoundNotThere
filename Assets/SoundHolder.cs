using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolder : MonoBehaviour {

    public float highlightCD = 0f;
    public float HighLightLookAtSeconds = 1f;
    bool highLighted = false;
    Vector3 originScale;
    public bool Spammable = true;
    bool ready2Play = true;
    float animationCD = 0;
    float animationTime = 1;
    public string SoundBit;
	// Use this for initialization
	void Start () {
        originScale = transform.localScale;
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
                AkSoundEngine.PostEvent(SoundBit, gameObject);
                StartCoroutine(Animate());
            }
        }
    }
    IEnumerator Animate()
    {
        //Shake & scale


        yield return null;
    }
}
