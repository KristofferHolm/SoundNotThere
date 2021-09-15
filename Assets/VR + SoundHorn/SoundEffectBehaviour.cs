using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectBehaviour : MonoBehaviour
{
    [SerializeField] MeshRenderer Renderer;
    public void Activate(Vector3 dir, AnimationCurve scaleBehaviour, AnimationCurve distortionAnimation, float time, Action poolcallback)
    {
        StartCoroutine(Fly(dir,scaleBehaviour, distortionAnimation, time, poolcallback));
    }
    IEnumerator Fly(Vector3 dir, AnimationCurve scaleBehaviour, AnimationCurve distortionAnimation, float time, Action poolcallback)
    {
        var startPos = transform.position;
        var endPos = dir + startPos;
        float timescale = 1f / time;
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.one * scaleBehaviour.Evaluate(t);
            Renderer.material.SetFloat("DistortionStrength", distortionAnimation.Evaluate(t));
            t += timescale * Time.deltaTime;
            yield return null;
        }
        poolcallback.Invoke();
    }
}
