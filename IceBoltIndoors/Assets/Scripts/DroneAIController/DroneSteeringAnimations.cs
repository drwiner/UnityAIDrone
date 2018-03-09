using UnityEngine;
using System.Collections;
using System;
using SteeringNamespace;

public class DroneSteeringAnimations : MonoBehaviour
{
    private Animator thisMecanim;
    private DynoBehavior_TimelineControl DBTC;
    private float tiltAmountVelocity, tiltVelocityForward, tiltAmountSideways, tiltAmountForward;
    //Vector3 currentForce;
    //float currentTorque;

    void Start()
    {
        thisMecanim = GetComponent<Animator>();
        DBTC = GetComponent<DynoBehavior_TimelineControl>();
    }
    //public float forceVelocity;
    void Update()
    {

        Swerve(DBTC.tiltAmountSideways);
        TiltForward(DBTC.tiltAmountForward);
        //Debug.Log(DBTC.force);
        //thisMecanim.SetFloat("angle") = DBTC.torque;
        //var droneDirection = transform.TransformDirection(DBTC.force);
        //var localDirection = transform.TransformDirection(DBTC.force);
        //var forwards = Mathf.SmoothDamp(DBTC.KinematicBody.getVelocity(), DBTC.targetVelocity, ref DBTC.force, 0.25f)
        //Mathf.SmoothDamp(currentRotation, wantedRotation, ref rotationVelocity, 0.25f);
        //var angle = Mathf.SmoothDamp(thisMecanim.GetFloat("angle"), localDirection.z, ref forceVelocity, .05f);
        //thisMecanim.SetFloat("angle", angle);
        //var forwards = Mathf.SmoothDamp(thisMecanim.GetFloat("angle"), localDirection.x, ref forceVelocity, .1f);
        //thisMecanim.SetFloat("forward", forwards);

    }

    void Swerve(float moveHorizontal)
    {
        
        if (Mathf.Abs(moveHorizontal) > 0.2f)
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -moveHorizontal, ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0f, ref tiltAmountVelocity, 0.1f);

        }

        thisMecanim.SetFloat("angle", tiltAmountSideways);
    }

    void TiltForward(float moveVertical)
    {
        if (moveVertical != 0f)
        {
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, moveVertical, ref tiltVelocityForward, 0.1f);

        }
        thisMecanim.SetFloat("forward", tiltAmountForward);
    }
}