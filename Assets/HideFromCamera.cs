using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFromCamera : MonoBehaviour
{
    Collider objCollider;
    Camera cam;
    Plane[] planes;
    bool visible = false;
    List<MeshRenderer> meshRenderers;
    void Start()
    {
        meshRenderers = new List<MeshRenderer>();
        meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>(true));
        cam = Camera.main;
        objCollider = GetComponent<Collider>();
        foreach (var rend in meshRenderers)
        {
            rend.enabled = false;
        }
    }

    void Update()
    {
        if (visible) return;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {

        }
        else
        {
            foreach (var rend in meshRenderers)
            {
                rend.enabled = true;
            }
            visible = true;
        }
    }
}
