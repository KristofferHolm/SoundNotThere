using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHolder : MonoBehaviour {

    public float highlightCD = 0f;
    public float HighLightLookAtSeconds = 1f;
    bool highLighted = false;
    Vector3 originScale;
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
}
