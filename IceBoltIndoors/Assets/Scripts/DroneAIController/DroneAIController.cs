using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoalNamespace;
using GraphNamespace;

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

        private bool atRest = true;

        void Start()
        {
            PC = GetComponent<PlayerController>();
            RB = GetComponent<Rigidbody>();
            SP = GetComponent<SteeringParams>();
        }

        public void SetPath(Stack<Vector3> _currentPath)
        {
            currentPath = _currentPath;
            currentGoal = currentPath.Pop();
        }

        public float Seek(Vector3 target, bool arrive)
        {
            var direction = currentGoal - transform.position;
            var distance = direction.magnitude;

            if (distance < goalRadius)
            {
                return 0f;
            }

            if (distance > slowRadius || !arrive)
            {
                return 1f;
            }

            // distance is less than or equal to slow radius. 
            return distance / slowRadius;
        }

        public float Align(Vector3 target)
        {
            return 0f;
        }

        void Update()
        {
            float seekOutput;
            if (currentPath.Count == 0)
            {
                if (!atRest)
                {
                    seekOutput = Seek(currentTarget, true);
                    if (seekOutput == 0f)
                        atRest = true;
                }
            }
            else
            {
                seekOutput = Seek(currentTarget, false);
                if (seekOutput == 0f)
                    currentTarget = currentPath.Pop();
            }
            
            

            //PC.SetInput(ds_force, ds_torque);

            //ds = new DynoSteering();
            //ds.force = ds_force.force;
            //ds.torque = ds_torque.torque;

            //kso = char_RigidBody.updateSteering(ds, Time.deltaTime);
            //transform.position = new Vector3(kso.position.x, transform.position.y, kso.position.z);
            //transform.rotation = Quaternion.Euler(0f, kso.orientation * Mathf.Rad2Deg, 0f);


        }
    }
}