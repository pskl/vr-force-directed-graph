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
		string[] table = miserablesNodes.text.Split (archdelim);
		foreach (string line in table) {

			var values = line.Split (',');
			if (values.Length > 0) {
				int parseResult;
				if (int.TryParse (values [0], out parseResult)) {
					Debug.Log ("Import " + line);
					Node.CreateNode (graph, parseResult, values [1]);
				}
			}
		}

//		table = miserablesEdges.text.Split (archdelim);
//		foreach (string line in table) {
//			var values = line.Split (',');
//			Edge.CreateEdge (values [0], values [1]);
//
//		}
	}
}
