using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Infection : MonoBehaviour {


    public Graph graph;

    public int incubation;
    public float chance;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SpreadInfection();
        }
    }


    public void SpreadInfection()
    {
        graph.nodes = graph.nodes.OrderBy(x => x.attractionlist.Count).ToList();
        Node firstnode = graph.nodes.First();

        List<Node> possibleNodes = graph.nodes.FindAll(x => x.attractionlist.Count == firstnode.attractionlist.Count).ToList();

        possibleNodes[Random.Range(0, possibleNodes.Count - 1)].FixedInfection();
    }
    
}
