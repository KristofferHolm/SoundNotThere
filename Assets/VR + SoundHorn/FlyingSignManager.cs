using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSignManager : MonoBehaviour
{
    static public FlyingSignManager Instance;
    public GameObject FlyingSign;

    private void Awake()
    {
        Instance = this;
    }
    
}
