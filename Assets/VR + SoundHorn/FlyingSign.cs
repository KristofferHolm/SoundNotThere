using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSign : MonoBehaviour
{
    public float AnimationSpeed = 1f;
    public GameObject Sign;
    public TMPro.TextMeshPro Text;
    public LineRenderer Line;
    List<LineRenderer> lines;
    public Transform LeftCorner, RightCorner;

    bool parenting = false;
    float scaleOfSign = 1f;
    bool animating;
    private Transform followPoint;
    private Transform[] point;
    private Vector3 offset;

    private Transform cam
    {
        get
        {
            if(_cam == null)
                _cam = Camera.main.transform;
            return _cam;
        }
    }
    Transform _cam;
    // Update is called once per frame
    void Update()
    {
        UpdatePosition(); // rotate based on the cameras y rotation and offsets the sign
       // UpdateRotation(); // rotate to look at camera
        UpdateLineRenderer(); // update lines
    }

    public void Activate(Transform[] points, Transform follow, Vector3 offset, string text, float scale = 1f, bool parenting = false)
    {
        lines = new List<LineRenderer>();
        lines.Add(Line);
        for (int i = 1; i < points.Length; i++)
        {
            lines.Add(Instantiate(Line, transform));
        }
        point = new Transform[points.Length];
        this.point = points;
        Text.text = text;
        scaleOfSign = scale;

        //if parenting
        //transform.parent = parent;
        //else#
        parenting = true;
        followPoint = follow;
        transform.parent = null;

        this.offset = offset;
        StartCoroutine(ActivationAnimation());
    }
    public void Close(Action callback)
    {
        StopAllCoroutines();
        StartCoroutine(CloseAnimation(callback));
    }

    IEnumerator ActivationAnimation()
    {
        animating = true;
        float t = 0;
        Sign.transform.localScale = OneScale(0);
        while (t<1)
        {
            t += Time.deltaTime * AnimationSpeed;
            Sign.transform.localScale = OneScale(t * scaleOfSign);
            transform.position = followPoint.position + offset * t;
            yield return null;
        }
        Sign.transform.localScale = OneScale(scaleOfSign);
        animating = false;
        yield return null;
    }
    IEnumerator CloseAnimation(Action callback)
    {
        animating = true;
        float t = Sign.transform.localScale.y;
        while (t > 0)
        {
            t -= Time.deltaTime * AnimationSpeed *2f; //double speed back
            Sign.transform.localScale = OneScale(t* scaleOfSign);
            var rot = GetDirToLookRot();
            transform.rotation = rot;
            transform.position = followPoint.position + rot * offset * t;
            yield return null;
        }
        Sign.transform.localScale = OneScale(0);
        animating = false;
        callback?.Invoke();
        yield return null;
    }

    Vector3 OneScale(float t)
    {
        return new Vector3(t, t, t);
    }

    void UpdatePosition()
    {
        var rot = GetDirToLookRot();
        transform.rotation = rot;
        if(!animating)
            transform.position = followPoint.position + rot * offset;
    }
    Quaternion GetDirToLookRot()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        Vector3 camPos = cam.position;
        camPos.y = 0;
        Vector3 dirToLookAt = camPos - pos;
        return Quaternion.LookRotation(-dirToLookAt);
        
    }
    void UpdateLineRenderer()
    {
        var linePos = new Vector3[2];
        for (int i = 0; i < point.Length; i++)
        {
            linePos[0] = GetClosestPos(point[i].position);
            linePos[1] = point[i].position;
            lines[i].SetPositions(linePos);
        }
    }
    
    Vector3 GetClosestPos(Vector3 point)
    {
        var dist1 = Vector3.Distance(LeftCorner.position, point);
        if (dist1 < Vector3.Distance(RightCorner.position, point))
            return LeftCorner.position;
        else
            return RightCorner.position;
    }

    void UpdateRotation()
    {
        // lets try only rotate in the x axis
        Vector3 pos = transform.position;
       // pos.z = 0;
       // pos.x = 0;
        Vector3 camPos = cam.position;
       // camPos.z = 0;
        //camPos.x = 0;
        //Vector3 dirToLookAt = camPos - pos;
        Quaternion rot = Quaternion.LookRotation(camPos - pos);
        transform.rotation = rot;
        return;
        var point = transform.position * 2.0f - cam.position;
        transform.LookAt(point);
    }
}
