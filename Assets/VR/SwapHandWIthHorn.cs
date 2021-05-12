using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHandWIthHorn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (GetSwapObject(collision, out var so))
        {
            so.Swap();
            Debug.Log("GAME START");
            SoundManager.Instance.StartGame();
            gameObject.SetActive(false);
        }
    }
    bool GetSwapObject(Collision col, out SwapObjects so)
    {
        var obj = col.transform;
        so = obj.GetComponent<SwapObjects>();
        if (so == null)
            so = obj.GetComponentInParent<SwapObjects>();
        if (so == null)
            so = obj.GetComponentInChildren<SwapObjects>();
        return so != null;
    }
}
