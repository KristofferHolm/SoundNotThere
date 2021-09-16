using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGraphicEffectManager : Singleton<SoundGraphicEffectManager>
{
    public GameObject SoundGraphicPrefab;
    public AnimationCurve HitAnimation, ListenAnimation;
    public AnimationCurve HitDistortion, ListenDistortion;
    public enum TypeOfAnimation
    {
        HitAnimation,
        ListenAnimation
    }
    public float SoundEffectLasts = 0.5f;
    /// <summary>
    /// when 
    /// </summary>
    public void CreateGraphicSoundEffect(Transform from, Vector3 dir, int numberOfGraphics, TypeOfAnimation ani,float time)
    {
        StartCoroutine(SequenceOfEffects(from, dir, numberOfGraphics, ani, time));
     
    }
    IEnumerator SequenceOfEffects(Transform from, Vector3 dir, int numberOfGraphics, TypeOfAnimation ani, float time)
    {
        float fractionOfTime = time / numberOfGraphics;
        AnimationCurve scaleAnimation = ListenAnimation;
        AnimationCurve distortionAnimation = ListenAnimation;
        switch (ani)    
        {
            case TypeOfAnimation.HitAnimation:
                scaleAnimation = HitAnimation;
                distortionAnimation = HitDistortion;
                break;
            case TypeOfAnimation.ListenAnimation:
                scaleAnimation = ListenAnimation;
                distortionAnimation = ListenDistortion;
                break; 
            default:
                break;
        }
        for (int i = 0; i < numberOfGraphics; i++)
        {
            //super hotfix
            if (ani == TypeOfAnimation.ListenAnimation)
                dir = from.up * dir.magnitude;
            ActivateEffect(from, dir, scaleAnimation, distortionAnimation, SoundEffectLasts);
            yield return new WaitForSeconds(fractionOfTime);
        }
        yield return null;
    }

    void ActivateEffect(Transform from, Vector3 dir, AnimationCurve scaleBehaviour, AnimationCurve distortionAnimation, float time)
    {
        if (!PoolManager.Instance.GetObj("sgp", out var sgp))
            sgp = Instantiate(SoundGraphicPrefab);
        sgp.transform.position = from.position;
        var rot = Quaternion.LookRotation((from.position + dir) - from.position, Vector3.up);
        sgp.transform.rotation = rot * Quaternion.Euler(90f, 0f, 0f);
        sgp.GetComponent<SoundEffectBehaviour>().Activate(dir, scaleBehaviour, distortionAnimation, time, () => PoolManager.Instance.PoolObj(sgp, "sgp"));
    }

}
