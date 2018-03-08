using UnityEngine;
using System.Collections;
using System;
using SteeringNamespace;

public class DroneSteeringAnimations : MonoBehaviour
{
    private Animator thisMecanim;
    private DynoBehavior_TimelineControl DBTC;
    //Vector3 currentForce;
    //float currentTorque;

    void Start()
    {
        thisMecanim = GetComponent<Animator>();
        DBTC = GetComponent<DynoBehavior_TimelineControl>();
    }

    void Update()
    {
        //thisMecanim.SetFloat("angle") = DBTC.torque;
        var droneDirection = transform.TransformDirection(DBTC.force);
        thisMecanim.SetFloat("angle", droneDirection.z);
        thisMecanim.SetFloat("forward", -droneDirection.x);

    }

    
}