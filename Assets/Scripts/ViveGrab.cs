using UnityEngine;
using System.Collections;

public class ViveGrab : MonoBehaviour
{
    public Graph graph;

	SteamVR_TrackedObject trackedObj;
	public GameObject grabbedObj;
	bool getColObj = false;

	Node colNode;

	FixedJoint joint;

	void Awake ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	
	void Update ()
	{

		var device = SteamVR_Controller.Input ((int)trackedObj.index);

		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
            //print("trigegerUp");
            grabbedObj.GetComponent<Node>().Accelerate(device.velocity);
			grabbedObj = null;

            if (graph.leftController == trackedObj)
                graph.grabLeft = null;
            else if (graph.rightController == trackedObj)
                graph.grabRight = null;
        }

		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			// print("triggerDown");
			getColObj = true;
		} else
			getColObj = false;

	}

	private void OnTriggerStay (Collider c)
	{
		//print("collision detect");
		//print(getColObj);
		if (getColObj && grabbedObj == null) {
			//print("SetGrabObj");

			if (c.GetComponent<Node> ()) {
				grabbedObj = c.gameObject;

                if (graph.leftController == trackedObj)
                {
                    Debug.Log("Grabbed Left " + grabbedObj.name);
                    graph.grabLeft = grabbedObj.GetComponent<Node>();
                }
                else if (graph.rightController == trackedObj)
                {
                    Debug.Log("Grabbed Right " + grabbedObj.name);
                    graph.grabRight = grabbedObj.GetComponent<Node>();
                }
            }
		}

             
	}

    
}
