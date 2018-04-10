using BoltFreezer.Interfaces;
using BoltFreezer.PlanTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class UnityActionOperator : MonoBehaviour
{
    // A Unity wrapper for operators, give parameters from typed hierarchy, and write out preconditions and effects

    public string Name;

    [SerializeField]
    public List<GameObject> MutableParameters;

    [SerializeField]
    public List<MutablePredicate> MutablePreconditions;

    [SerializeField]
    public List<MutablePredicate> MutableEffects;

    [SerializeField]
    public List<NonEqualTuple> NonEqualityConstraints;

    [SerializeField]
    public Operator thisOp;

    //private DomainOperators OperatorComponent;

    public void Awake()
    {
        //GetOperatorComponent();
        CreateOperator();
    }

    public void Update()
    {
        //GetOperatorComponent();
        CreateOperator();
    }

    //public void GetOperatorComponent()
    //{
    //    if (OperatorComponent == null)
    //    {
    //        var actionHost = GameObject.FindGameObjectWithTag("ActionHost");
    //        OperatorComponent = actionHost.GetComponent<DomainOperators>();
    //    }
    //}

    public void CreateOperator()
    {
        // Instantiate Terms
        var terms = new List<ITerm>();
        for (int i = 0; i < MutableParameters.Count; i++)
        {
            var go = MutableParameters[i];
            var term = new Term(i.ToString());
            term.Type = go.name;
            terms.Add(term as ITerm);
        }

        // Instantiate Preconditions
        var preconditions = new List<IPredicate>();
        foreach (var precon in MutablePreconditions)
        {
            preconditions.Add(MutableToPredicate(precon, terms));
        }

        // Instantiate Effects
        var effects = new List<IPredicate>();
        foreach (var eff in MutableEffects)
        {
            effects.Add(MutableToPredicate(eff, terms));
        }

        // Instantiate Operator
        thisOp = new Operator(new Predicate(Name, terms, true), preconditions, effects);
        Debug.Log(thisOp.Name);

        // Instantiate Nonequality constraints
        thisOp.NonEqualities = new List<List<ITerm>>();
        foreach (var nonequality in NonEqualityConstraints)
        {
            thisOp.NonEqualities.Add(new List<ITerm>() { terms[nonequality.first], terms[nonequality.second]});
        }

    }

    public Predicate MutableToPredicate(MutablePredicate precon, List<ITerm> terms)
    {
        var preconTerms = new List<ITerm>();
        foreach (var t in precon.terms)
        {
            preconTerms.Add(terms[t] as ITerm);
        }
        var newPrecon = new Predicate(precon.predicateName, preconTerms, precon.sign);
        return newPrecon;
    }
}

[Serializable]
public class NonEqualTuple
{
    public int first;
    public int second;
}

[Serializable]
public class MutablePredicate
{
    public string predicateName;
    public List<int> terms;
    public bool sign;

    public MutablePredicate(string _name, List<int> _terms)
    {
        predicateName = _name;
        terms = _terms;
        sign = true;
    }

    public MutablePredicate(string _name, List<int> _terms, bool _sign)
    {
        predicateName = _name;
        terms = _terms;
        sign = _sign;
    }
}

[Serializable]
public class MutableTerm
{
    public string typeName;
}
