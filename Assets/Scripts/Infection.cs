using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infection : MonoBehaviour
{
    public InfectionManager infectionManager;
    public Node node;
    public float timeOfInfection;
    public float infectionChance;

    public void InfectionImpulse()
    {
        if (Time.time - timeOfInfection > infectionManager.incubation)
        {
            foreach (Edge e in node.attractionlist)
            {
                e.endangered = true;
                e.Other(node).endangered = true;
                e.Other(node).CheckForInfection(this);
            }
        }
    }
}
