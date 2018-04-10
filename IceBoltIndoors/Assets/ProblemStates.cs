using BoltFreezer.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BoltFreezer.PlanTools;

[ExecuteInEditMode]
public class ProblemStates : MonoBehaviour {

    public List<string> InitialState;
    public List<string> GoalState;

    public List<IPredicate> initialPredicateList;
    public List<IPredicate> goalPredicateList;

    public bool reset;

    public void Awake()
    {
        ReadProblem();
        reset = false;
    }

    public void Update()
    {
        if (reset)
        {
            ReadProblem();
            reset = false;
        }
    }

    public void ReadProblem()
    {
        initialPredicateList = new List<IPredicate>();
        goalPredicateList = new List<IPredicate>();

        if (InitialState.Count > 0)
        {
            Debug.Log("Initial State");
            foreach (var stringItem in InitialState)
            {
                var pred = ProcessStringItem(stringItem);
                initialPredicateList.Add(pred as IPredicate);
                Debug.Log(pred.ToString());
            }
        }
        if (GoalState.Count > 0)
        {
            Debug.Log("Goal State");
            foreach (var stringItem in GoalState)
            {
                var pred = ProcessStringItem(stringItem);
                goalPredicateList.Add(pred as IPredicate);
                Debug.Log(pred.ToString());
            }
        }
    }

    public Predicate ProcessStringItem(string stringItem)
    {
        var signage = true;
        var stringArray = stringItem.Split(' ');
        if (stringArray[0].Equals("not"))
        {
            signage = false;
            stringArray = stringArray.Skip(1).ToArray();
        }
        var predName = stringArray[0];
        var terms = new List<ITerm>();
        foreach (var item in stringArray.Skip(1))
        {
            var go = GameObject.Find(item);
            var newObj = new Term(item, go.name, go.tag);
            terms.Add(newObj as ITerm);
        }
        var newPredicate = new Predicate(predName, terms, signage);
        return newPredicate;
    }
}
