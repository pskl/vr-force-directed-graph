using UnityEngine;
using System.Collections.Generic;

using System.Linq;

public class Graph : Basic
{
	public static Graph instance;
    public InfectionManager infectionManager;

	// public SteamVR_TrackedObject leftController;
	// public SteamVR_TrackedObject rightController;
	
    public GameObject nodePrefab;
    public GameObject edgePrefab;

    public Node grabLeft;
    public Node grabRight;

    public float initialGrabDistance;
    public float ripFactor;

	public GameObject center;
	public float gravity;
	public float scale;
	public float repulsionFactor;
	public float attractionFactor;

	public float speed;

    public bool gameOver;
    public float gameOverThreshold;

	
	Vector3 initialCenter;
	Vector3 initialGraphCenter;
	private Vector3 initialNodeScale = new Vector3 (0.03f, 0.03f, 0.03f);

	public List<Node> nodes = new List<Node> ();
	public List<Edge> edges = new List<Edge> ();

	public override void Awake ()
	{
		instance = this;
	}

	#region creation
    public Node CreateNode(int id, string name)
    {
        GameObject newNodeGO = GameObject.Instantiate(nodePrefab) as GameObject;
        newNodeGO.transform.SetParent(center.transform);
        newNodeGO.name = name;
        Node newNode = newNodeGO.GetComponent<Node>();
        newNode.id = id;

        newNode.graph = this;
        nodes.Add(newNode);

        return newNode;
    }
	
    public Edge CreateEdge(Node node1, Node node2, float weight)
    {
        GameObject newEdgeGO = GameObject.Instantiate(edgePrefab) as GameObject;
        newEdgeGO.transform.SetParent(center.transform);
        newEdgeGO.name = "Edge " + node1.name + "/" + node2.name + " (" + weight.ToString() + ")";
        Edge newEdge = newEdgeGO.GetComponent<Edge>();

        newEdge.node1 = node1;
        newEdge.node2 = node2;

        node1.attractionlist.Add(newEdge);
        node2.attractionlist.Add(newEdge);

        newEdge.weight = weight;

        newEdge.graph = this;
        edges.Add(newEdge);

        return newEdge;
    }
	#endregion
	
	// public void UpdateControllers()
	// {
	// 	var deviceLeft = SteamVR_Controller.Input ((int)leftController.index);
	// 	var deviceRight = SteamVR_Controller.Input ((int)rightController.index);

	// 	if (deviceLeft.GetTouchDown (SteamVR_Controller.ButtonMask.Grip) || deviceRight.GetTouchDown (SteamVR_Controller.ButtonMask.Grip)) {
	// 		initialDistance = Vector3.Distance (leftController.transform.position, rightController.transform.position);
	// 		initialCenter = Vector3.Lerp (leftController.transform.position, rightController.transform.position, 0.5f);
	// 		initialGraphCenter = center.transform.position;
	// 		initialRepulsion = repulsion;
	// 		initialPoition = transform.position;
	// 		initialGraphRotation = transform.rotation;
    //         //initialControllerAxis  = Vector3.Angle(rightController.transform.position, leftController.transform.position);

	// 		checkRotationGo = new GameObject ();

	// 		initialRot = transform.rotation;

	// 		checkRotationGo.transform.position = rightController.transform.position;
	// 		checkRotationGo.transform.LookAt (leftController.transform.position);

	// 		initalCheckRotation = checkRotationGo.transform.rotation;

	// 		initialForwardVec = transform.up;

	// 	}


    //     if (deviceLeft.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) || deviceRight.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
    //     {
    //         //Debug.Log("Trigger DOWN");
    //         initialGrabDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);

    //     }


    //     if (deviceLeft.GetTouchUp (SteamVR_Controller.ButtonMask.Grip))
	// 		firstRun = true;
 
	// 	if (deviceLeft.GetTouch (SteamVR_Controller.ButtonMask.Grip) && deviceRight.GetTouch (SteamVR_Controller.ButtonMask.Grip)) {
	// 		notTriggerPressed = false;
	// 		Zoom ();
	// 		DragCenter ();
	// 		//RotateGraph ();
	// 	} //else
	// 		//transform.forward = (leftController.transform.position - rightController.transform.position).normalized;
	// }

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("G A M E  O V E R");
        
    }

	// public void Zoom ()
	// {
	// 	float currentDistance = Vector3.Distance (leftController.transform.position, rightController.transform.position);
	// 	repulsion = initialRepulsion * (currentDistance / initialDistance);   
	// 	foreach (Node node in nodes) {
	// 		float factor = Mathf.Pow (repulsion, 0.3f);
	// 		node.gameObject.transform.localScale = initialNodeScale * factor;
	// 	}
            
	// }

	// public void DragCenter ()
	// {
	// 	Vector3 currentCenter = Vector3.Lerp (leftController.transform.position, rightController.transform.position, 0.5f);
	// 	center.transform.position = initialGraphCenter + (currentCenter - initialCenter);
	// 	transform.position = initialPoition + (currentCenter - initialCenter);
	// }

	// public void RotateGraph ()
	// {
	// 	//float currentControllerAxis = Vector3.Angle(rightController.transform.position, leftController.transform.position);
	// 	//transform.RotateAround(center.transform.position, Vector3.up, currentControllerAxis);

	// 	//checkRotationGo.transform.LookAt(leftController.transform.position);

	// 	//transform.rotation = checkRotationGo.transform.rotation * initalCheckRotation ;

	// 	//Quaternion lookRot = Quaternion.LookRotation(transform.position + (leftController.transform.position - rightController.transform.position).normalized, transform.up);
	// 	//Quaternion realRot = transform.rotation;
	// 	transform.LookAt ((transform.position + (leftController.transform.position - rightController.transform.position).normalized), transform.up);
	// 	//print(transform.rotation.eulerAngles);

	// 	//if (firstRun)
	// 	//{
	// 	//transform.rotation *= realRot;
	// 	//    firstRun = false;
	// 	//}
	// 	//transform.LookAt(rightController.transform.position);

	// }



}