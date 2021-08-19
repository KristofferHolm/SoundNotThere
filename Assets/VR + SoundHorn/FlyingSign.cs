using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSign : MonoBehaviour
{

    public GameObject Sign;
    public TMPro.TextMeshPro Text;
    public LineRenderer Line;

    private Transform point;
    private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        UpdateLineRenderer();
        UpdatePosition();
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    void Activate(Transform point, Transform parent, Vector3 offset, string text, Action callback)
    {
        this.point = point;
        Text.text = text;
        transform.parent = parent;
        this.offset = offset;
        callback += Close;
    }
    void UpdatePosition()
    {
        transform.localPosition = offset;
    }
    void UpdateLineRenderer()
    {
        var linePos = new Vector3[2];
        linePos[0] = Sign.transform.position;
        linePos[1] = point ? point.position : Vector3.zero;
        Line.SetPositions(linePos);
    }
}
