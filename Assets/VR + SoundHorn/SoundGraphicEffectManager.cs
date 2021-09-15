using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGraphicEffectManager : Singleton<SoundGraphicEffectManager>
{
    public GameObject SoundGraphicPrefab;
    public AnimationCurve HitAnimation, ListenAnimation;
    public AnimationCurve HitDistortion, ListenDistortion;
    public enum ScaleAnimation
    {
        HitAnimation,
        ListenAnimation
    }
        public float soundEffectLasts = 0.5f;
    /// <summary>
    /// when 
    /// </summary>
    public void CreateGraphicSoundEffect(Transform from, Vector3 dir, int numberOfGraphics, ScaleAnimation ani,float time)
    {
        StartCoroutine(SequenceOfEffects(from, dir, numberOfGraphics, ani, time));
     
    }
    IEnumerator SequenceOfEffects(Transform from, Vector3 dir, int numberOfGraphics, ScaleAnimation ani, float time)
    {
        float fractionOfTime = time / numberOfGraphics;
        AnimationCurve scaleAnimation = ListenAnimation;
        AnimationCurve distortionAnimation = ListenAnimation;
        switch (ani)    
        {
            case ScaleAnimation.HitAnimation:
                scaleAnimation = HitAnimation;
                distortionAnimation = HitDistortion;
                break;
            case ScaleAnimation.ListenAnimation:
                scaleAnimation = ListenAnimation;
                distortionAnimation = ListenDistortion;
                break; 
            default:
                break;
        }
        for (int i = 0; i < numberOfGraphics; i++)
        {
            ActivateEffect(from, dir, scaleAnimation, distortionAnimation, soundEffectLasts);
            yield return new WaitForSeconds(fractionOfTime);
        }
        yield return null;
    }

    void ActivateEffect(Transform from, Vector3 dir, AnimationCurve scaleBehaviour, AnimationCurve distortionAnimation, float time)
    {
        if (!PoolManager.Instance.GetObj("sgp", out var sgp))
            sgp = Instantiate(SoundGraphicPrefab);
        sgp.transform.position = from.position;
        sgp.transform.rotation = from.rotation * Quaternion.Euler(90,0,0);
        sgp.GetComponent<SoundEffectBehaviour>().Activate(dir, scaleBehaviour, distortionAnimation, time, () => PoolManager.Instance.PoolObj(sgp, "sgp"));
    }

}
