using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoalNamespace;
using GraphNamespace;
using System;

namespace SteeringNamespace
{

    public class DroneAIController : MonoBehaviour
    {
        // path Stack
        private Stack<Vector3> currentPath = new Stack<Vector3>();
        private Vector3 currentGoal;
        private Vector3 prevGoal;

        private Vector3 currentTarget;
        //private Node nextTile;

        private PlayerController PC;
        private Rigidbody RB;
        private SteeringParams SP;

        public float goalRadius = 0.5f;
        public float slowRadius = 2.5f;
        public float angularSlowRadius = 60f;
        public float arriveTime = 1f;

        //private Vector3 direction;

        public bool steering = false;
        private float force, torque;

        private Action seekTask;
        private Action alignTask;

        void Start()
        {
            PC = GetComponent<PlayerController>();
            RB = GetComponent<Rigidbody>();
            SP = GetComponent<SteeringParams>();
        }

        public void PushGoal(Vector3 _newGoal)
        {
            currentGoal = _newGoal;
        }

        public void SetPath(Stack<Vector3> _currentPath)
        {
            currentPath = _currentPath;
            currentGoal = currentPath.Pop();
        }

        public void Steer(Vector3 origin, Vector3 target, bool departed, bool arrive)
        {
            PushGoal(target);
            var direction = currentGoal - transform.position;

            //var force = Seek(arrive);
            //var torque = Align();

            seekTask = () => Seek(arrive);
            alignTask = Align;

            if (departed)
            {
                var currentVelocity = (transform.position + direction).normalized * SP.MAXSPEED;
                RB.velocity = currentVelocity;
                transform.LookAt(target);
            }
        }

        public void Seek(bool arrive)
        {
            var distance = (currentGoal - transform.position).magnitude;

            if (distance < goalRadius)
            {
                force = 0f;
                return;
            }

            if (distance > slowRadius || !arrive)
            {
                force = 1f;
                return;
            }

            // distance is less than or equal to slow radius. 
            force = distance / slowRadius;
        }

        public void Align()
        {

            var direction = currentGoal - transform.position;
            //var rotate = direction - transform.eulerAngles;
            var rotation = mapToRange(Mathf.Atan2(direction.z, -direction.x));
            var rotationSize = Mathf.Abs(rotation);
            float targetRotation;
            if (rotationSize < 0.05f)
            {
                torque = 0f;
                return;
            }
            if (rotationSize > angularSlowRadius)
            {
                targetRotation = SP.MAXROTATION;
            }
            else
            {
                targetRotation = SP.MAXROTATION * rotationSize / angularSlowRadius;
            }

            // Final target rotation combines speed (already in variable) with rotation direction
            targetRotation = targetRotation * rotation / rotationSize;

            var newTorque = targetRotation - mapToRange(Mathf.Atan2(RB.velocity.z, -RB.velocity.x));
            newTorque = newTorque / arriveTime;

            var angularAcceleration = Mathf.Abs(newTorque);

            if (angularAcceleration > SP.MAXANGULAR)
            {
                newTorque /= angularAcceleration;
                torque = newTorque * SP.MAXANGULAR;
                return;
            }
            torque = newTorque;

        }


        public static float mapToRange(float radians)
        {
            float targetRadians = radians;
            while (targetRadians <= -Mathf.PI)
            {
                targetRadians += Mathf.PI * 2;
            }
            while (targetRadians >= Mathf.PI)
            {
                targetRadians -= Mathf.PI * 2;
            }
            return targetRadians;
        }


        void Update()
        {
            if (steering)
            {
                seekTask();
                alignTask();
                PC.SetInput(force, torque);
                //if (isDone())
                steering = false;
            }
        }

        public bool isDone()
        {
            if ((transform.position - currentGoal).magnitude < goalRadius)
            {
                return true;
            }
            return false;
        }
        //void Update()
        //{
        //    float seekOutput;
        //    if (currentPath.Count == 0)
        //    {
        //        if (!atRest)
        //        {
        //            seekOutput = Seek(currentTarget, true);
        //            if (seekOutput == 0f)
        //                atRest = true;
        //        }
        //    }
        //    else
        //    {
        //        seekOutput = Seek(currentTarget, false);
        //        if (seekOutput == 0f)
        //            currentTarget = currentPath.Pop();
        //            seekOutput = Seek(currentTarget, false);
        //    }

        //    if (!atRest) {
        //        var currentVelocity = (direction * seekoutput).Normalize() * ;
        //        PC.SetInput(seekOutput, 0f);
        // }
        //PC.SetInput(ds_force, ds_torque);

        //ds = new DynoSteering();
        //ds.force = ds_force.force;
        //ds.torque = ds_torque.torque;

        //kso = char_RigidBody.updateSteering(ds, Time.deltaTime);
        //transform.position = new Vector3(kso.position.x, transform.position.y, kso.position.z);
        //transform.rotation = Quaternion.Euler(0f, kso.orientation * Mathf.Rad2Deg, 0f);


        // }
    }
}