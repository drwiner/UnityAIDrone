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

        public void Arrive(Vector3 target)
        {
            
        }

        void Update()
        {
            if (currentPath.Count == 0)
            {
                if (!atRest)
                {
                    Arrive(currentTarget);
                }
            }
            


            // if we are not at last point on path
            if (currentPath.Count > 0)
            {
                // seek next point on path
                ds_force = seek.getSteering(currentGoal);

                // pop when seek says we've made it into range and seek the next target
                if (seek.changeGoal)
                {
                    nextTile = currentPath.Pop();
                    currentGoal = QuantizeLocalize.Localize(nextTile);
                    if (currentPath.Count > 0)
                        ds_force = seek.getSteering(currentGoal);
                    else
                        ds_force = arrive.getSteering(currentGoal);
                }
            }
            // otherwise, we are approaching the path goal.  we should arrive.
            else if (currentPath.Count == 0)
            {
                ds_force = arrive.getSteering(currentGoal);
            }


            ds_torque = align.getSteering(currentGoal);

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