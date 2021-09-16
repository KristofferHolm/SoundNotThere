using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipForControllers : MonoBehaviour
{
    public Transform LeftHand, RightHand;
    public Transform[] TeleportPoints, SwapSoundPoints, ListenPoints, MovePoints;

    bool firstpoked, firstsqueezed;
    OVRInput.Controller right, left;
    void Start()
    {
        right = OVRInput.Controller.RTouch;
        left = OVRInput.Controller.LTouch;
        SoundManager.Instance.OnGameStart += StartTeleportTip;
        SoundManager.Instance.OnGameStart += () => StopAllCoroutines();
        SoundManager.Instance.FirstPoke += () =>
        {
            FlyingSignManager.Instance.CloseSign("listen");
            firstpoked = true;
            StartSwapTip();
        };
        SoundManager.Instance.FirstSqueeze += () =>
        {
            FlyingSignManager.Instance.CloseSign("swap");
            firstsqueezed = true;
        };
        FlyingSignManager.Instance.CloseSign += CloseSignEvent;
        StartCoroutine(MoveTipAfter5sec());
        //StartTooltips();
    }
    void CloseSignEvent(string type)
    {
        if (type == "teleport")
        {
            StartPokeTip();
            FlyingSignManager.Instance.CloseSign -= CloseSignEvent;
        }

    }
    IEnumerator MoveTipAfter5sec()
    {
        yield return new WaitForSeconds(5f);
        StartMoveTip();
    }
    void StartSwapTip()
    {
        if (firstsqueezed) return;
        FlyingSignManager.Instance.CreateSign(SwapSoundPoints, SwapSoundPoints[0], Vector3.left * 0.15f + Vector3.up * 0.1f, "Squeeze horn", "swap", 0.5f);

    }
    void StartPokeTip()
    {
        if (firstpoked) return;
        FlyingSignManager.Instance.CreateSign(ListenPoints, ListenPoints[0], Vector3.right * 0.15f + Vector3.up * 0.1f, "Poke objects", "listen", 0.5f);

    }
    public void StartTeleportTip()
    {
        FlyingSignManager.Instance.CreateSign(TeleportPoints, TeleportPoints[0], Vector3.left * 0.15f + Vector3.up * 0.1f, "Teleport", "teleport", 0.5f);
    }
    void StartMoveTip()
    {
        FlyingSignManager.Instance.CreateSign(MovePoints, MovePoints[0], Vector3.right * 0.15f + Vector3.up * 0.1f, "Move", "move",0.5f);
    }

    void GetAllToolTips()
    {
        FlyingSignManager.Instance.CreateSign(TeleportPoints, TeleportPoints[0], Vector3.left * 0.15f + Vector3.up * 0.1f, "Teleport", "teleport", 0.5f);
        FlyingSignManager.Instance.CreateSign(ListenPoints, ListenPoints[0], Vector3.right * 0.15f + Vector3.up * 0.1f, "Poke objects", "listen", 0.5f);
        FlyingSignManager.Instance.CreateSign(SwapSoundPoints, SwapSoundPoints[0], Vector3.left * 0.15f + Vector3.up * 0.1f, "Squeeze horn", "swap", 0.5f);
        FlyingSignManager.Instance.CreateSign(MovePoints, MovePoints[0], Vector3.right * 0.15f + Vector3.up * 0.1f, "Move", "move",0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        if (firstsqueezed && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger,right) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, right))
        {
            FlyingSignManager.Instance.CloseSign("swap");
        }
        if (firstpoked && OVRInput.GetDown(OVRInput.Button.One,right)|| OVRInput.GetDown(OVRInput.Button.Two, right))
        {
            FlyingSignManager.Instance.CloseSign("listen");
        }

        if (OVRInput.GetDown(OVRInput.Button.Start, left))
        {
            GetAllToolTips();
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).sqrMagnitude > 0)
        {
            FlyingSignManager.Instance.CloseSign("move");
        }


    }
}
