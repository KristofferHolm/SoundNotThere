using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipForControllers : MonoBehaviour
{
    public Transform LeftHand, RightHand;
    public Transform[] TeleportPoints, SwapSoundPoints, ListenPoints, MovePoints; 


    OVRInput.Controller right, left;
    void Start()
    {
        right = OVRInput.Controller.RTouch;
        left = OVRInput.Controller.LTouch;
        SoundManager.Instance.OnGameStart += StartTooltips;
        StartTooltips();
    }

    public void StartTooltips()
    {
        FlyingSignManager.Instance.CreateSign(SwapSoundPoints, SwapSoundPoints[0], Vector3.left * 0.3f + Vector3.up * 0.2f, "Squeeze horn", "swap", 0.5f);
        FlyingSignManager.Instance.CreateSign(ListenPoints, ListenPoints[0], Vector3.right * 0.3f + Vector3.up * 0.2f, "Poke objects", "listen", 0.5f);
        FlyingSignManager.Instance.CreateSign(TeleportPoints, TeleportPoints[0], Vector3.left * 0.3f + Vector3.up * 0.2f, "Teleport", "teleport", 0.5f);
        FlyingSignManager.Instance.CreateSign(MovePoints, MovePoints[0], Vector3.right * 0.25f + Vector3.up * 0.2f, "Move", "move",0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger,right) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, right))
        {
            FlyingSignManager.Instance.CloseSign("swap");
        }
        if (OVRInput.GetDown(OVRInput.Button.One,right)|| OVRInput.GetDown(OVRInput.Button.Two, right))
        {
            FlyingSignManager.Instance.CloseSign("listen");
        }

        if (OVRInput.GetDown(OVRInput.Button.Start, left))
        {
            StartTooltips();
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).sqrMagnitude > 0)
        {
            FlyingSignManager.Instance.CloseSign("move");
        }


    }
}
