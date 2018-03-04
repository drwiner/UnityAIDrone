using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinFan : MonoBehaviour {

    public float rotationSpeed = 360f;

    void Update () {
        //Vector3 rotationToApply = new Vector3(transform.eulerAngles)
        //transform.RotateAroundLocal(Vector3.up, Time.deltaTime * rotationSpeed);
        //transform.Rotate(Vector3.up)
        //transform.RotateAround(transform.position, rotationSpeed);
        //transform.Rotate(transform.rotation.eulerAngles);
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
    }
}
