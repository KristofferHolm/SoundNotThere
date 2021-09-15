using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSwitcherVR : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject lazer;
    public GameObject SoundEffectPrefab;
    private MeshRenderer lazerMesh;
    public float Range;
    public string SoundBit;
    public SoundBoard.Sound Sound = SoundBoard.Sound.Null;
    public GameObject AudioSphere;
    public AnimationCurve AC;
    public Material matEmission;
    SoundBoard SB;
    float RightClickCD = 0;
    LayerMask layerMask;
    bool cantAct = false;
    float delayOfSwap = .25f;

    void Start()
    {
        lazerMesh = lazer.GetComponent<MeshRenderer>();
        layerMask = LayerMask.NameToLayer("Interactable");
        layerMask = ~layerMask;
        SB = GameObject.Find("GameManager").GetComponent<SoundBoard>();
    }
    private void OnValidate()
    {
        if (Sound == SoundBoard.Sound.Null)
        {
            System.Enum.TryParse(SoundBit, out SoundBoard.Sound sEnum);
            Sound = sEnum;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (cantAct) return;
        ray.direction = lazer.transform.up * Range;
        ray.origin = lazer.transform.position;
        lazerMesh.enabled = OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.Get(OVRInput.RawButton.RHandTrigger) || OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Two);
        if (Physics.Raycast(ray, out hit, Range, layerMask))
        {
            if (hit.collider.tag == "PapFigur")
            {
               
                hit.transform.GetComponent<SoundHolder>().LookedAt();
                if (OVRInput.GetUp(OVRInput.Button.One) || OVRInput.GetUp(OVRInput.Button.Two))
                    PlaySound(hit.transform.gameObject);
                if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
                    SwitchSound(hit.collider.GetComponent<SoundHolder>());
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
            {
                ListenInHand();
            }
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            ListenInHand();
        }
        if (RightClickCD > 0)
            RightClickCD -= Time.deltaTime;
    }

    void SoundEffectNoHit(float time)
    {
        int numberofwaves = (int)(time * 10f);
        SoundGraphicEffectManager.Instance.CreateGraphicSoundEffect(lazer.transform, lazer.transform.up * Range, numberofwaves, SoundGraphicEffectManager.ScaleAnimation.ListenAnimation, time);
    }
    void SoundEffectHit(SoundHolder SH, float introTime,float OutroTime, float delayTime, Action callback)
    {
        SoundGraphicEffectManager.Instance.CreateGraphicSoundEffect(lazer.transform, lazer.transform.up * Range, (int)(introTime * 10f), SoundGraphicEffectManager.ScaleAnimation.ListenAnimation, introTime);
        AkSoundEngine.PostEvent(Sound, gameObject);

        StartCoroutine(SoundEffectHitSequence(delayTime, OutroTime,SH, callback));
    }
    IEnumerator SoundEffectHitSequence(float delay,float outro,SoundHolder SH, Action callback)
    {
        yield return new WaitForSeconds(delay);
        SoundGraphicEffectManager.Instance.CreateGraphicSoundEffect(lazer.transform, lazer.transform.up * Range, (int)(outro * 10f), SoundGraphicEffectManager.ScaleAnimation.ListenAnimation, outro);
        AkSoundEngine.PostEvent(SH.SoundEnum, SH.gameObject);
        yield return new WaitForSeconds(outro);
        callback.Invoke();
    }

    private void ListenInHand()
    {
        if (RightClickCD > 0)
            return;
        else
            RightClickCD = SB.GetsSoundLength(Sound);
        SoundEffectNoHit(SB.GetsSoundLength(Sound));
        AkSoundEngine.PostEvent(Sound, gameObject);
        StartCoroutine(ChangeHorn(RightClickCD));
    }

    IEnumerator ChangeHorn(float t)
    {
        //change graphic
        matEmission.SetColor("_EmissionColor", Color.white * 5);
        Vector3 pos = transform.localPosition;
        while (t > 0)
        { 
            // SHAKE
            transform.localPosition = pos + Time.deltaTime * UnityEngine.Random.insideUnitSphere * 0.25f;
            t -= Time.deltaTime;
            yield return null;
        }
        transform.localPosition = pos;
        matEmission.SetColor("_EmissionColor", Color.white);
        //change back
        yield return null;
    }
    private void SwitchSound(SoundHolder SH)
    {
        if (RightClickCD > 0 || SH.Completed)
            return;
        float swapTime = SB.GetsSoundLength(Sound) + SB.GetsSoundLength(SH.SoundEnum) + delayOfSwap;
        RightClickCD = swapTime;
        LockPlayer(true);
        SoundEffectHit(SH, SB.GetsSoundLength(Sound), SB.GetsSoundLength(SH.SoundEnum), delayOfSwap, ()=> LockPlayer(false));
        StartCoroutine(ChangeHorn(SB.GetsSoundLength(Sound)));
        AkSoundEngine.PostEvent(SoundBoard.Sound.Swap, gameObject);
        
        ////sphere from papfigur
        //StartCoroutine(AnimateAudioSphere(SH.SoundEnum, hit.transform.position, transform.position + transform.forward * 0.05f, -transform.right));
        ////sphere from player
        //StartCoroutine(AnimateAudioSphere(Sound, transform.position + transform.forward * 0.05f, hit.transform.position, transform.right));
        if (SH.Completed)
            return;
        var temp = Sound;
        Sound = SH.SoundEnum;
        SH.SoundEnum = temp;
        if (SH.SoundEnum.ToString() == SH.name)
            SH.Complete();
    }
    Transform parent;
    private void LockPlayer(bool locked)
    {
        if (cantAct == locked) return;
        if (parent == null)
            parent = transform.parent;
        if (locked)
        {
            transform.parent = null;
            cantAct = true;
        }
        else
            StartCoroutine(MoveTowardsParent());
            
    }

    IEnumerator MoveTowardsParent()
    {
        while (Vector3.Distance(transform.position, parent.position) > 0.5f)
        {
            Vector3.MoveTowards(transform.position, parent.position, Time.deltaTime);
            yield return null;
        }
        transform.parent = parent;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        cantAct = false;
    }

    private IEnumerator AnimateAudioSphere(SoundBoard.Sound sound, Vector3 from, Vector3 to, Vector3 turn)
    {
        float t = 0;
        GameObject sphere = Instantiate<GameObject>(AudioSphere);
        AkSoundEngine.PostEvent(sound, sphere);
        sphere.transform.position = transform.position;
        while (t < 1f)
        {

            t += Time.deltaTime / 2.0f; // så det tager 2 sekunder
            sphere.transform.position = Vector3.Lerp(from, to, t) + turn * AC.Evaluate(t); ;
            yield return null;
        }
        Destroy(sphere);
        yield return null;
    }
    private void PlaySound(GameObject go)
    {
        if (RightClickCD > 0)
            return;
        go.GetComponent<SoundHolder>().PlaySound();
    }

}
