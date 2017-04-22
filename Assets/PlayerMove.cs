using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

    Rigidbody rig;
    Transform cam;
    Vector3 moveDir;
    public Image horn;
    public Transform hornMoveToPos;
    public AnimationCurve AC;
    Vector3 hornPos;
    bool moveToLeft = true;
    public float HornSpeed;
    public float Speed;
    public float t = 0;
    // Use this for initialization
    void Start () {
        hornPos = horn.transform.position;
        cam = transform.GetChild(0);
        rig = GetComponent<Rigidbody>();
        
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        moveDir = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        rig.velocity = moveDir * Time.deltaTime * Speed;
    
        
       
    }
    private void Update()
    {
        if (t < 0)
            t = 0;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")) == 0)
        {
            t -= Time.fixedDeltaTime / HornSpeed;
            
            if (t < 0)
                moveToLeft = true;
        }
        else if (moveToLeft)
        {
            t += Time.fixedDeltaTime / HornSpeed;

            if (t > 1)
                moveToLeft = false;
        }
        else
        {
            t -= Time.fixedDeltaTime / HornSpeed;
            if (t < 0)
                moveToLeft = true;
        }
        horn.rectTransform.position = Vector3.Lerp(hornPos, hornMoveToPos.position, t) + (Vector3.down * Screen.height/4f * AC.Evaluate(t));
    }
}
