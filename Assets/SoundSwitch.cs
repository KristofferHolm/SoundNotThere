using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSwitch : MonoBehaviour {

    Ray ray;
    RaycastHit hit;
    public float Range;
    Camera cam;
    bool pickedUpHorn = false;
    public string SoundBit;
    public GameObject AudioSphere;
    public AnimationCurve AC;
    public Image horn;
    soundBoard SB;
    PlayerMove movement;
    public Sprite Horn_0, Horn_1;
    float RightClickCD = 0;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        movement = transform.parent.GetComponent<PlayerMove>();
        SB = GameObject.Find("GameManager").GetComponent<soundBoard>();
    }
	
	// Update is called once per frame
	void Update () {

        ray.direction = transform.forward * Range;
        ray.origin = transform.position;
        if(!pickedUpHorn)
        {
            if (Physics.Raycast(ray, out hit, Range))
            {
                if (hit.collider.tag == "Horn")
                {
                    hit.transform.GetComponent<SoundHolder>().LookedAt();
                    if (Input.GetButtonDown("LeftClick"))
                        PickHorn(hit.transform.gameObject);
                }
            }
            return;
        }
        if (Physics.Raycast(ray, out hit, Range))
        {
            if (hit.collider.tag == "PapFigur")
            {
                print(hit.collider.tag);
                hit.transform.GetComponent<SoundHolder>().LookedAt();
                if (Input.GetButtonDown("LeftClick"))
                    PlaySound(hit.transform.gameObject);
                if (Input.GetButtonDown("RightClick"))
                    SwitchSound(hit.collider.GetComponent<SoundHolder>());
            }
            else if (Input.GetButtonDown("RightClick"))
            {
                ListenInHand();
            }
        }
        else if (Input.GetButtonDown("RightClick"))
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
            RightClickCD = SB.GetsSoundLength(SoundBit);
        AkSoundEngine.PostEvent(SoundBit, gameObject);
        StartCoroutine(ChangeHorn(RightClickCD));
    }
    IEnumerator ChangeHorn(float time)
    {
        horn.sprite = Horn_1;
        yield return new WaitForSeconds(time);
        horn.sprite = Horn_0;
        yield return null;
    }
    private void SwitchSound(SoundHolder SH)
    {
        if (RightClickCD > 0 || SH.Completed)
            return;
        else
            RightClickCD = 2.0f;
        StartCoroutine(ChangeHorn(2.0f));
        AkSoundEngine.PostEvent("Swap", gameObject);
        //sphere from papfigur
        StartCoroutine(AnimateAudioSphere(SH.SoundBit,hit.transform.position,transform.position + transform.forward * 0.05f, -transform.right));
        //sphere from player
        StartCoroutine(AnimateAudioSphere(SoundBit,transform.position+ transform.forward*0.05f, hit.transform.position, transform.right));
        if (SH.Completed)
            return;
        string temp = SoundBit;
        SoundBit = SH.SoundBit;
        SH.SoundBit = temp;
        if (SH.SoundBit == SH.name)
            SH.Complete();
    }

    private IEnumerator AnimateAudioSphere(string sound, Vector3 from, Vector3 to, Vector3 turn)
    {
        float t = 0;
        GameObject sphere = Instantiate<GameObject>(AudioSphere);
        movement.pickedUpHorn = false; // TO LOCK THE MOVEMENT
        AkSoundEngine.PostEvent(sound, sphere);
        while (t < 1f)
        {
            
            t += Time.deltaTime / 2.0f; // så det tager 2 sekunder
            cam.fieldOfView = 60 + AC.Evaluate(t) * 50; // 60 fov er standard
            sphere.transform.position = Vector3.Lerp(from, to, t) + turn * AC.Evaluate(t); ;
            yield return null;
        }
        movement.pickedUpHorn = true;
        Destroy(sphere);
        yield return null;
    }
    private void PlaySound(GameObject go)
    {
        if (RightClickCD > 0)
            return;
        go.GetComponent<SoundHolder>().PlaySound();
    }

    private void PickHorn(GameObject go)
    {
        movement.PickUpHorn();
        Destroy(go);
        pickedUpHorn = true;
    }
}
