using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class InfectionManager : Basic
{
    public Graph graph;
    public GameObject infectionPrefab;

    public List<Infection> infections = new List<Infection>();
    public List<Node> evilNodes = new List<Node>();

    public Node evilMaster;


    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            RandomInfection();
        }
    }

    public void RandomInfection()
    {
        List<Node> possibleNodes = graph.nodes.
            Where(x=>!x.infection).
            OrderBy(x => x.attractionlist.Count).
            ToList();

        Node infectionCandidate = possibleNodes[Random.Range(0, possibleNodes.Count - 1)];
        infectionCandidate.BecomeInfected();
    }

    public Infection InstantiateInfection(Node node)
    {
        GameObject newInfectionGO = GameObject.Instantiate(infectionPrefab) as GameObject;
        newInfectionGO.transform.SetParent(node.transform);
        Infection newInfection = newInfectionGO.GetComponent<Infection>();
        newInfection.infectionManager = this;

        newInfection.node = node;
        node.infection = newInfection;
        newInfection.timeOfInfection = Time.time;

        infections.Add(newInfection);

        return newInfection;
    }

    public void AddToEvilNodes(Node node)
    {
        if (evilNodes.Count == 0)
        {
            evilMaster = node;
            node.dontRepel = true;
        }

        foreach (Node evilNode in evilNodes)
        {
            graph.CreateEdge(evilNode, node, 1f);
        }

        evilNodes.Add(node);
    }


}
