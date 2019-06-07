using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class InfectionManager : MonoBehaviour
{
    public Graph graph;
    
    public int incubation;
    public float infectionChance;

    public List<Node> infectedNodes = new List<Node>();
    public List<Node> evilNodes = new List<Node>();

    public Node evilMaster;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            CreateInfection();
        }

        foreach(Node node in infectedNodes)
        {
            node.InfectOthers();
        }

        CheckForGameOver();
    }

    public void CreateInfection()
    {
        graph.nodes = graph.nodes.OrderBy(x => x.attractionlist.Count).ToList();
        Node firstnode = graph.nodes.First();

        List<Node> possibleNodes = graph.nodes.FindAll(x => x.attractionlist.Count == firstnode.attractionlist.Count).ToList();

        Node infectionCandidate = possibleNodes[Random.Range(0, possibleNodes.Count - 1)];
        infectionCandidate.BecomeInfected();

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

    public void CheckForGameOver()
    {
        if (infectedNodes.Count > 0)
        {
            foreach(Node healthyNode in graph.nodes)
            {
                if (Vector3.Distance(healthyNode.transform.position, 
                infectedNodes.First().transform.position) < graph.gameOverThreshold && !graph.gameOver)
                {
                    graph.GameOver();
                }
            }
        }
    }


}
