using UnityEngine;
using System.Collections;

public class GraphImporter : MonoBehaviour
{
	public static GraphImporter instance;
	public TextAsset miserables;
	public TextAsset miserablesEdges;
	public TextAsset miserablesNodes;
	public GameObject nodePrefab;
	public GameObject edgePrefab;


	public void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		Import ();
	}


	public void Import ()
	{
		Graph graph = Graph.instance;
		char[] archdelim = new char[]{ '\r', '\n' };
		string[] tableNodes = miserablesNodes.text.Split (archdelim);
		string[] tableEdges = miserablesEdges.text.Split (archdelim);

		foreach (string line in tableNodes) {

			var values = line.Split (',');
			if (values.Length > 0) {
				int parseResult;
				if (int.TryParse (values [0], out parseResult)) {
					Node.CreateNode (graph, parseResult, values [1]);
				}
			}
		}

		foreach (string line in tableEdges) {
			var values = line.Split (',');
			if (values.Length > 1) {
				float parseWeight;
				if (float.TryParse (values [2], out parseWeight)) {
					Edge.CreateEdge (graph, FindNode (graph, values [0]), FindNode (graph, values [1]), parseWeight);
				}
			}
		}


		foreach (Node n in graph.nodes) {
			n.RefreshRepulsionList ();
		}
	}

	public Node FindNode (Graph graph, string searchID)
	{
		int parseResult;
		if (int.TryParse (searchID, out parseResult))
			return graph.nodes.Find (x => x.id == parseResult);
		else
			return null;
	}
}
