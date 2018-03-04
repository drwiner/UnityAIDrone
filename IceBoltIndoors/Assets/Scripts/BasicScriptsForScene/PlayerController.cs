using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{

    //MovementUpDown
    public float upForce;

    // MovementForward
    private float movementForwardSpeed = 500f;
    private float tiltAmountForward = 0f;
    private float tiltVelocityForward = 0f;

    // MovementRotation
    private float wantedRotation;
    private float currentRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationVelocity;

    // Swerve
    private float sideMovementAmount = 300.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;

    private float moveHorizontal;
    private float moveVertical;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        MovementUpDown();
        MovementForward();
        MovementRotation();
        ClampingSpeedValues();
        Swerve();

        rb.AddRelativeForce(Vector3.up * upForce);
        rb.rotation = Quaternion.Euler(
                new Vector3(tiltAmountForward, currentRotation, tiltAmountSideways)
            );

    }

    
    void Swerve()
    {
        if (Mathf.Abs(moveHorizontal) > 0.2f)
        {
            rb.AddRelativeForce(Vector3.right * moveHorizontal * sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -20f * moveHorizontal, ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0f, ref tiltAmountVelocity, 0.1f);

        }
    }

    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        var mv = Math.Abs(moveVertical);
        var mh = Math.Abs(moveHorizontal);

        if (mv > 0.2f)
        {
            // time * f, where f is speed of clamp. 
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }

        if (mv < 0.2f && mh > 0.2f)
        {
            // time * f, where f is speed of clamp. 
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }

        if (mv < 0.2f && mh < 0.2f)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
      
    }

    void MovementForward()
    {
        if (moveVertical != 0f)
        {
            rb.AddRelativeForce(Vector3.forward * moveVertical * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * moveVertical, ref tiltVelocityForward, 0.1f);

        }
    }

    void MovementUpDown()
    {
        if (Mathf.Abs(moveVertical) > 0.2f || Mathf.Abs(moveHorizontal) > 0.2f)
        {
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, Time.deltaTime * 5), rb.velocity.z);
                upForce = 281f;
            }
            // if there's steering left or right (angular)
            if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, Time.deltaTime * 5), rb.velocity.z);
                upForce = 110f;
            }

            if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
            {
                upForce = 410f;
            }

        }

        if (Mathf.Abs(moveVertical) < 0.2f && Mathf.Abs(moveHorizontal) > 0.2f)
        {
            upForce = 135f;
        }

        if (Input.GetKey(KeyCode.I))
        {
            upForce = 450f;
            if (Mathf.Abs(moveHorizontal) > 0.2f)
            {
                upForce = 500f;
            }
        }
        else if (Input.GetKey(KeyCode.K))
        {
            upForce = -200f;
        }
        else if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(moveVertical) < 0.2f && Mathf.Abs(moveHorizontal) < 0.2f))
        {
            upForce = 98.1f;
        }
    }

    void MovementRotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantedRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantedRotation += rotateAmountByKeys;
        }

        currentRotation = Mathf.SmoothDamp(currentRotation, wantedRotation, ref rotationVelocity, 0.25f);
    }
    
}