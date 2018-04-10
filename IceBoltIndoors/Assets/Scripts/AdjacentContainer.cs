using BoltFreezer.Utilities;
using SteeringNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AdjacentContainer : MonoBehaviour {

    //[SerializeField]
    [SerializeField]
    public List<LocationTuple> Edges = new List<LocationTuple>();

    public GameObject ActorHost;

    public void Awake()
    {
        CompileLocations();
    }

    public void Update()
    {
        CompileLocations();
    }

    public void CompileLocations()
    {
        if (ActorHost == null)
        {
            ActorHost = GameObject.FindGameObjectWithTag("ActorHost");
        }

        var velocityMaxPairs = new List<KeyValuePair<StoreActionTiming, double>>();
        for (int i = 0; i < ActorHost.transform.childCount; i++)
        {
            var child = ActorHost.transform.GetChild(i).gameObject;
            var childSteering = child.GetComponent<SteeringParams>();
            if (childSteering != null)
            {
                var sat = child.GetComponent<StoreActionTiming>();
                if (sat == null)
                {
                    sat = child.AddComponent<StoreActionTiming>();
                }
                sat.Reset();
                velocityMaxPairs.Add(new KeyValuePair<StoreActionTiming, double>(sat, childSteering.MAXSPEED));
            }
        }

        foreach (var edge in Edges)
        {
            edge.distance = getDistance(edge.source.transform, edge.sink.transform);
            var vectorOfTravel = edge.sink.transform.position - edge.source.transform.position;
            edge.firstThird = edge.source.transform.position + vectorOfTravel / 3;
            edge.halfWay = edge.source.transform.position + vectorOfTravel / 2;
            edge.finalThird = edge.source.transform.position + (2 / 3) * vectorOfTravel;
            foreach (var actorspeedpair in velocityMaxPairs)
            {
                var SatComponent = actorspeedpair.Key;
                var newTravelTime = new TravelTime(edge, edge.distance / actorspeedpair.Value);
                SatComponent.TravelTimes.Add(newTravelTime);
            }
        }
    }

    

    public static double getDistance(Transform a, Transform b)
    {
        return Math.Round(Vector3.Distance(a.position, b.position),2);
    }

}

[Serializable]
public class LocationTuple
{
    [SerializeField]
    public GameObject source;
    [SerializeField]
    public GameObject sink;
    [SerializeField]
    public double distance = 0;

    [SerializeField]
    public Vector3 firstThird;
    [SerializeField]
    public Vector3 halfWay;
    [SerializeField]
    public Vector3 finalThird;
}

[Serializable]
public class TravelTime
{
    [SerializeField]
    public LocationTuple Edge;

    [SerializeField]
    public double timeToTravel = 0;

    public TravelTime(LocationTuple edge, double travelTime)
    {
        Edge = edge;
        timeToTravel = travelTime;
    }

}

public class StoreActionTiming : MonoBehaviour
{
    public List<TravelTime> TravelTimes = new List<TravelTime>();

    public void Reset()
    {
        TravelTimes = new List<TravelTime>();
    }
}