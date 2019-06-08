using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infection : Basic
{
    public InfectionManager infectionManager;
    public Node node;
    public float timeOfInfection;
    public float infectionChance;


    public override void Start()
    {
        base.Start();
        Debug.Log("Start infection " + node.name);
        InvokeRepeating("InfectionImpulse", 0.1f, 1.0f);
    }
    
    public void InfectionImpulse()
    {
        Debug.Log(name + " sends Infection impulses");
        foreach (Edge e in node.attractionlist)
        {
            e.Other(node).CheckForInfection(this);
        }
    }
}
