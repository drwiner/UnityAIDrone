using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMecanimParams : MonoBehaviour {

    public float currentVelocity = 0f;
    private Rigidbody thisRigidBody;
    private Animator thisMecanim;
	// Use this for initialization
	void Start () {
        thisRigidBody = GetComponent<Rigidbody>();
        thisMecanim = GetComponent<Animator>();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
    }
	
	// Update is called once per frame
	void Update () {
        thisMecanim.SetFloat("Velocity", thisRigidBody.velocity.magnitude);
    }
}
