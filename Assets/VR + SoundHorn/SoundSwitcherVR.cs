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
    public LocomotionTeleport LocomotionTeleport;
    public OVRPlayerController OVRPlayerController;

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
                    SwitchSound(hit.collider.GetComponent<SoundHolder>(),hit.distance);
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
        int numberofwaves = (int)(time * 20f);
        SoundGraphicEffectManager.Instance.CreateGraphicSoundEffect(lazer.transform, lazer.transform.up * Range, numberofwaves, SoundGraphicEffectManager.TypeOfAnimation.ListenAnimation, time);
    }
    void SoundEffectHit(SoundHolder SH,float dist, float introTime,float OutroTime, float delayTime, Action callback)
    {
        SoundGraphicEffectManager.Instance.CreateGraphicSoundEffect(lazer.transform, lazer.transform.up * dist, (int)(introTime * 10f), SoundGraphicEffectManager.TypeOfAnimation.HitAnimation, introTime);
        AkSoundEngine.PostEvent(Sound, gameObject);

        StartCoroutine(SoundEffectHitSequence(introTime, delayTime, OutroTime,SH, callback));
    }
    IEnumerator SoundEffectHitSequence(float intro, float delay,float outro,SoundHolder SH, Action callback)
    {
        yield return new WaitForSeconds(intro);
        yield return new WaitForSeconds(delay);
        SoundGraphicEffectManager.Instance.CreateGraphicSoundEffect(SH.transform, transform.position - SH.transform.position, (int)(outro * 10f), SoundGraphicEffectManager.TypeOfAnimation.HitAnimation, outro);
        //AkSoundEngine.PostEvent(SH.SoundEnum, SH.gameObject);
        SH.PlaySound();
        yield return new WaitForSeconds(outro + SoundGraphicEffectManager.Instance.SoundEffectLasts);
        callback.Invoke();
    }

    private void ListenInHand()
    {
        SoundManager.Instance.FirstSqueeze?.Invoke();
        if (RightClickCD > 0)
            return;
        else
            RightClickCD = SB.GetsSoundLength(Sound);
        SoundEffectNoHit(SB.GetsSoundLength(Sound));
        AkSoundEngine.PostEvent(Sound, gameObject);
        StartCoroutine(ShakeHorn(RightClickCD));
    }

    IEnumerator ShakeHorn(float t)
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
    private void SwitchSound(SoundHolder SH, float dist)
    {
        SoundManager.Instance.FirstSqueeze?.Invoke();
        if (RightClickCD > 0 || SH.Completed)
            return;
        float swapTime = SB.GetsSoundLength(Sound) + SB.GetsSoundLength(SH.SoundEnum) + delayOfSwap;
        LockPlayer(true);
        StartCoroutine(ShakeHorn(SB.GetsSoundLength(Sound)));
        AkSoundEngine.PostEvent(SoundBoard.Sound.Swap, gameObject);
        SoundEffectHit(SH, dist, SB.GetsSoundLength(Sound), SB.GetsSoundLength(SH.SoundEnum), delayOfSwap, ()=>
        {
            LockPlayer(false);

            if (SH.Completed)
                return;
            var temp = Sound;
            Sound = SH.SoundEnum;
            SH.SoundEnum = temp;
            if (SH.SoundEnum.ToString() == SH.name)
                SH.CompleteVR();
        });
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
            LocomotionTeleport.EnableTeleportation(false);
            OVRPlayerController.SetHaltUpdateMovement(true);
        }
        else
            StartCoroutine(MoveTowardsParent());
            
    }

    IEnumerator MoveTowardsParent()
    {
        var t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.66f;
            transform.position = Vector3.Lerp(startPos, parent.position, t);
            transform.rotation = Quaternion.Lerp(startRot, parent.rotation, t);
            yield return null;
        }
        transform.parent = parent;
        transform.localPosition= Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        cantAct = false;
        LocomotionTeleport.EnableTeleportation(true);
        OVRPlayerController.SetHaltUpdateMovement(false);
    }
    private void PlaySound(GameObject go)
    {
        if (RightClickCD > 0)
            return;
        go.GetComponent<SoundHolder>().PlaySound();
        SoundManager.Instance.FirstPoke?.Invoke();
    }

}
