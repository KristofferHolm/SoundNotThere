using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findmehorn : MonoBehaviour
{
    float t = 0;
    bool activated;
    private void Update()
    {
        if (activated) return;
        t += Time.deltaTime;
        if (t > 12)
        {
            FlyingSignManager.Instance.CreateSign(transform, transform, Vector3.up * 0.4f + Vector3.right * 0.2f, "Pick me up with your right hand", "pickupHorn",2);
            activated = true;
        }
    }
    private void OnDisable()
    {
        FlyingSignManager.Instance.CloseSign("pickupHorn");
        FindObjectOfType<TooltipForControllers>().StartTeleportTip();
    }

}
