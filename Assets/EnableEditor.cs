using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEditor : MonoBehaviour
{
    public bool EnableInEditor = true;
    void Start()
    {

#if UNITY_EDITOR
        gameObject.SetActive(EnableInEditor);
#else
        gameObject.SetActive(!EnableInEditor);
#endif

    }

}