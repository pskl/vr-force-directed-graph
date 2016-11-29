using UnityEngine;
using System.Collections;
using System.IO;

public class Graph : MonoBehaviour
{


	public int repulsion;
	public int attraction;
	public int springLength;
	public TextAsset miserables;


	// Use this for initialization
	void Start ()
	{
		
 
	}

	void Awake ()
	{	
//		string text_content = Resources.Load("miserables");
		Debug.Log (miserables.text);
		DataStore parsed = JsonUtility.FromJson<DataStore> (miserables.text);
		Debug.Log (parsed.nodes);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}

public class DataStore
{
	public ArrayList nodes;
	public ArrayList links;

}
