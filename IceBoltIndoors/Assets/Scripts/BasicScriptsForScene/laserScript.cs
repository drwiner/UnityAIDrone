using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour {
	public Transform startPoint;
	public Transform endPoint;
    public Material laserMaterial;
	LineRenderer laserLine;
    public ParticleSystem PS;

    public bool firing = false;
	// Use this for initialization
	void Awake () {
		laserLine = GetComponent<LineRenderer> ();
		laserLine.SetWidth (.1f, .1f);
        laserLine.material = laserMaterial;
    }
	
	// Update is called once per frame
	void Update () {
        if (firing)
        {
            Fire();
        }
        else
        {
            Stop();
        }

	}

    public void Fire()
    {
        if (!PS.isPlaying)
        {
            laserLine.enabled = false;

            var main = PS.main;
            var distance = Vector3.Distance(startPoint.position, endPoint.position);
            Debug.Log(distance);
            var neededTime = distance / main.startSpeed.constantMax;

            //PS.startLifetime = neededTime;

            
            //main.duration = neededTime;
            //main.startSpeed = neededTime;
            main.startLifetime = neededTime * 3;

            PS.Play();
            transform.LookAt(endPoint);
            laserLine.SetPosition(0, startPoint.position);
            laserLine.SetPosition(1, endPoint.position);
        }
    }

    public void Stop()
    {
        PS.Stop();
        laserLine.enabled = false;
    }

}
