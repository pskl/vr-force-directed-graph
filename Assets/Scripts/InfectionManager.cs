using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class InfectionManager : MonoBehaviour
{
    public Graph graph;
    public GameObject infectionPrefab;
    
    public int incubation;

    public List<Infection> infections = new List<Infection>();
    public List<Node> evilNodes = new List<Node>();

    public Node evilMaster;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            RandomInfection();
        }

        foreach(Infection infection in infections)
        {
            infection.InfectionImpulse();
        }
    }

    public void RandomInfection()
    {
        Node firstnode = graph.nodes.First();

        List<Node> possibleNodes = graph.nodes.
            OrderBy(x => x.attractionlist.Count).
            Where(x => x.attractionlist.Count == firstnode.attractionlist.Count).
            ToList();

        Node infectionCandidate = possibleNodes[Random.Range(0, possibleNodes.Count - 1)];
        infectionCandidate.BecomeInfected();
    }

    public Infection InstantiateInfection(Node node, float time)
    {
        GameObject newInfectionGO = GameObject.Instantiate(infectionPrefab) as GameObject;
        newInfectionGO.transform.SetParent(node.transform);
        Infection newInfection = newInfectionGO.GetComponent<Infection>();
        newInfection.infectionManager = this;

        newInfection.node = node;
        node.infection = newInfection;
        newInfection.timeOfInfection = time;

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
