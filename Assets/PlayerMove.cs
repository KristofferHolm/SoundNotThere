using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    Rigidbody rig;
    Transform cam;
    public float Speed;
    // Use this for initialization
    void Start () {
        cam = transform.GetChild(0);
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rig.velocity += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        rig.velocity += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        rig.drag = rig.velocity.magnitude;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            rig.velocity = Vector3.zero;
        
    }
}
