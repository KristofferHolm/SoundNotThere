using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCanera : MonoBehaviour
{
    Camera MainCam;
    void Update()
    {
        if (MainCam == null)
            MainCam = Camera.main;
        transform.LookAt(MainCam.transform);
    }
}
