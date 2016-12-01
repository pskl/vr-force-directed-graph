using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
	public static Graph instance;
	public SteamVR_TrackedObject leftController;
	public SteamVR_TrackedObject rightController;

	public Material materialStandard;
	public Material materialHighlighted;

	public GameObject center;
	public float repulsion;
	public float attraction;
	public float springLength;
	public float damping;

	public List<Node> nodes = new List<Node> ();
	public List<Edge> edges = new List<Edge> ();

	float initialRepulsion;
	float initialDistance;
	Vector3 initialCenter;
	Vector3 initialGraphCenter;
	Vector3 initialPoition;
	Quaternion initialGraphRotation;
	float initialControllerAxis;

	GameObject checkRotationGo;
	Quaternion initalCheckRotation;
	private Vector3 initialNodeScale = new Vector3 (0.03f, 0.03f, 0.03f);

	Vector3 initialForwardVec;

	Quaternion initialRot;

	bool firstRun;
	bool notTriggerPressed = true;

	public void Awake ()
	{
		instance = this;

	}

	public void Update ()
	{   
		transform.position = center.transform.position;
     
		var deviceLeft = SteamVR_Controller.Input ((int)leftController.index);
		var deviceRight = SteamVR_Controller.Input ((int)rightController.index);

		if (deviceLeft.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger) || deviceRight.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			initialDistance = Vector3.Distance (leftController.transform.position, rightController.transform.position);
			initialCenter = Vector3.Lerp (leftController.transform.position, rightController.transform.position, 0.5f);
			initialGraphCenter = center.transform.position;
			initialRepulsion = repulsion;
			initialPoition = transform.position;
			initialGraphRotation = transform.rotation;
			//initialControllerAxis  = Vector3.Angle(rightController.transform.position, leftController.transform.position);

			checkRotationGo = new GameObject ();

			initialRot = transform.rotation;

			checkRotationGo.transform.position = rightController.transform.position;
			checkRotationGo.transform.LookAt (leftController.transform.position);

			initalCheckRotation = checkRotationGo.transform.rotation;

			initialForwardVec = transform.up;

		}

		if (deviceLeft.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger))
			firstRun = true;
            
 
		if (deviceLeft.GetTouch (SteamVR_Controller.ButtonMask.Trigger) && deviceRight.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			notTriggerPressed = false;
			Zoom ();
			DragCenter ();
			RotateGraph ();
		} else
			transform.forward = (leftController.transform.position - rightController.transform.position).normalized;
       

	}

	public void Zoom ()
	{
		float currentDistance = Vector3.Distance (leftController.transform.position, rightController.transform.position);
		repulsion = initialRepulsion * (currentDistance / initialDistance);   
		foreach (Node node in nodes) {
			float factor = Mathf.Pow (repulsion, 0.3f);
			node.gameObject.transform.localScale = initialNodeScale * factor;
		}
            
	}

	public void DragCenter ()
	{

		Vector3 currentCenter = Vector3.Lerp (leftController.transform.position, rightController.transform.position, 0.5f);
		center.transform.position = initialGraphCenter + (currentCenter - initialCenter);
		transform.position = initialPoition + (currentCenter - initialCenter);
	}

	public void RotateGraph ()
	{
		//float currentControllerAxis = Vector3.Angle(rightController.transform.position, leftController.transform.position);
		//transform.RotateAround(center.transform.position, Vector3.up, currentControllerAxis);

		//checkRotationGo.transform.LookAt(leftController.transform.position);

		//transform.rotation = checkRotationGo.transform.rotation * initalCheckRotation ;

		//Quaternion lookRot = Quaternion.LookRotation(transform.position + (leftController.transform.position - rightController.transform.position).normalized, transform.up);
		//Quaternion realRot = transform.rotation;
		transform.LookAt ((transform.position + (leftController.transform.position - rightController.transform.position).normalized), transform.up);
		//print(transform.rotation.eulerAngles);

		//if (firstRun)
		//{
		//transform.rotation *= realRot;
		//    firstRun = false;
		//}
		//transform.LookAt(rightController.transform.position);




	}



}