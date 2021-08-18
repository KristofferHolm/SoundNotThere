using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSwitcherVR : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public GameObject lazer;
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

    private void ListenInHand()
    {
        if (RightClickCD > 0)
            return;
        else
            RightClickCD = SB.GetsSoundLength(Sound);
        AkSoundEngine.PostEvent(Sound, gameObject);
        StartCoroutine(ChangeHorn(RightClickCD));
    }
    IEnumerator ChangeHorn(float t)
    {
        //change graphic
        matEmission.SetColor("_EmissionColor", Color.white * 5);
        Vector3 pos = transform.localPosition;
        while (t > 0)
        { // SHAKE
            transform.localPosition = pos + Time.deltaTime * UnityEngine.Random.insideUnitSphere;
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
        else
            RightClickCD = 2.0f;
        StartCoroutine(ChangeHorn(2.0f));
        AkSoundEngine.PostEvent(SoundBoard.Sound.Swap, gameObject);
        //sphere from papfigur
        StartCoroutine(AnimateAudioSphere(SH.SoundEnum, hit.transform.position, transform.position + transform.forward * 0.05f, -transform.right));
        //sphere from player
        StartCoroutine(AnimateAudioSphere(Sound, transform.position + transform.forward * 0.05f, hit.transform.position, transform.right));
        if (SH.Completed)
            return;
        var temp = Sound;
        Sound = SH.SoundEnum;
        SH.SoundEnum = temp;
        if (SH.SoundEnum.ToString() == SH.name)
            SH.Complete();
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
