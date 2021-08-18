using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGravity : MonoBehaviour
{
    public Rigidbody Rigidbody;
    private void OnValidate()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.OnGameStart += () => Rigidbody.useGravity = true;
    }
}
