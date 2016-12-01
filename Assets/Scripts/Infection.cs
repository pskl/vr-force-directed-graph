using UnityEngine;
using System.Collections;

public class Infection : MonoBehaviour {


    public Graph graph;

    public int incubation;
    public float chance;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {

            int infectedID = Random.Range(0, graph.nodes.Count-1);
            graph.nodes[infectedID].FixedInfection();
        }
    }
    
}
