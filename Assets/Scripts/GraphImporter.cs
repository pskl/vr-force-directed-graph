using UnityEngine;
using System.Collections;

public class GraphImporter : Basic
{
    public Graph graph;
    public TextAsset miserables;
    public TextAsset miserablesEdges;
    public TextAsset miserablesNodes;

    public override void Awake()
    {
        Import();
    }

    public void Import()
    {
        char[] archdelim = new char[] { '\r', '\n' };

        ImportNodes(miserablesNodes.text.Split(archdelim));
        ImportEdges(miserablesEdges.text.Split(archdelim));
    }

    protected void ImportNodes(string[] tableNodes)
    {
        foreach (string line in tableNodes)
        {
            var values = line.Split(',');
            ImportNode(values);
        }
    }

    protected void ImportNode(string[] values)
    {
        if (values.Length > 0)
        {
            int parseResult;
            if (int.TryParse(values[0], out parseResult))
            {
                graph.CreateNode(parseResult, values[1]);
            }
        }
    }

    protected void ImportEdges(string[] tableEdges)
    {
        foreach (string line in tableEdges)
        {
            var values = line.Split(',');
            ImportEdge(values);
        }
    }

    protected void ImportEdge(string[] values)
    {
        if (values.Length > 1)
        {
            float parseWeight;
            if (float.TryParse(values[2], out parseWeight))
            {
                graph.CreateEdge(
                    FindNode(graph, values[0]), 
                    FindNode(graph, values[1]), 
                    parseWeight
                );
            }
        }
    }

    public Node FindNode(Graph graph, string searchID)
    {
        int parseResult;
        if (int.TryParse(searchID, out parseResult))
            return graph.nodes.Find(x => x.id == parseResult);
        else
            return null;
    }
}
