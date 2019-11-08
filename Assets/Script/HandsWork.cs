using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsWork : MonoBehaviour {

    [SerializeField] protected Kinematic kinematic;

    [SerializeField] protected GameObject leftHand;
    [SerializeField] protected GameObject rightHand;

    protected float endPos = 0.4f;
    protected float startPos;

    Vector3 leftPos;
    Vector3 rightPos;

    // Use this for initialization
    void Start () {

        startPos = leftHand.transform.localScale.x / 2;

        leftPos = leftHand.transform.localPosition;
        rightPos = rightHand.transform.localPosition;

    }
	
	// Update is called once per frame
	void Update () {
        if (kinematic.InpJoint.J6 != leftPos.x)
        {
            SetPos(kinematic.InpJoint.J6);
        }
        

    }


    protected void SetPos(float input) // input 0 -- 100
    {
        float value = Mathf.Lerp(startPos, endPos, input/100);

        leftPos.x = value;
        rightPos.x = - value;

        leftHand.transform.localPosition = leftPos;
        rightHand.transform.localPosition = rightPos;
    }
    
}
