using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBehavoiur : MonoBehaviour
{
    public LocomotionTeleport Teleportation;

    private void OnValidate()
    {
        if(Teleportation == null)
            transform.parent.GetComponentInChildren<LocomotionTeleport>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Teleportation.enabled = false;
        SoundManager.Instance.OnGameStart += () => Teleportation.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
