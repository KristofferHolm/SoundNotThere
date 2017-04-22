using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    Rigidbody rig;
    Transform cam;
    Vector3 moveDir;
    public float Speed;
    // Use this for initialization
    void Start () {
        cam = transform.GetChild(0);
        rig = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        moveDir = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        rig.velocity = moveDir * Time.deltaTime * Speed;
    }
}
