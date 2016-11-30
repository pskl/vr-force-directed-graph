using UnityEngine;
using System.Collections;

public class ViveGrab : MonoBehaviour {

    GameObject grabbedObj;

    Node colNode;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        
    }

    void FixedUpdate()
    {
        
    }
	
	
	void Update ()
    {
        if (colNode != null)
        {
            print("setPosition");
            colNode.transform.position = transform.position;
        }
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("trigegerUp");
            colNode = null;
        }
    }

    private void OnTriggerStay(Collider c)
    {

        print("triggerIsEneter");
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("triggerDown");
            colNode = c.GetComponent<Node>();
        }
    }

    
}
