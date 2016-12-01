using UnityEngine;
using System.Collections;

public class ViveGrab : MonoBehaviour
{
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
		if (grabbedObj != null) {
			// print("setPosition");
			//grabbedObj.transform.position = transform.position;
		}

		var device = SteamVR_Controller.Input ((int)trackedObj.index);
		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			//print("trigegerUp");
			grabbedObj = null;
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
			}
		}

             
	}

    
}
