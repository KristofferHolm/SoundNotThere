using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(FadeIn());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator FadeIn()
    {
        Image img = GetComponent<Image>();
        float t = 0;
        Color col = Color.black;
        while(t<1)
        {
            t += Time.deltaTime / 2;
            col.a = 1.0f - t;
            img.color = col;
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
