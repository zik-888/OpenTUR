using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] protected Transform target;
    public int speedCam = 15;
    

    void FixedUpdate()
    {
        transform.LookAt(target);


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(target.position, transform.up, speedCam * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(target.position, -transform.up, speedCam * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * speedCam * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-Vector3.forward * speedCam * Time.deltaTime);
            
        }
    }
}
