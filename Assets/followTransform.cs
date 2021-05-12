using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTransform : MonoBehaviour
{
    public Transform Follow;
    void Update()
    {
        transform.position = Follow.position;
    }
}
