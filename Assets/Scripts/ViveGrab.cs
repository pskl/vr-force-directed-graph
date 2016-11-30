using UnityEngine;
using System.Collections;

public class ViveGrab : MonoBehaviour {

    GameObject grabbedObj;

    Node colNode;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;

    bool getColObj = false;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        
    }

    void FixedUpdate()
    {
        
    }
	
	
	void Update ()
    {
        if (grabbedObj != null)
        {
           // print("setPosition");
            grabbedObj.transform.position = transform.position;
            if (grabbedObj.GetComponent<MateralChange>() != null)
            {
                print("color change");

                MateralChange matchange = grabbedObj.GetComponent<MateralChange>();
                grabbedObj.GetComponent<Node>().ChangeColors();
              
            }
        }
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            //print("trigegerUp");
            grabbedObj = null;
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
           // print("triggerDown");
            getColObj = true;
        }
        else
            getColObj = false;

       
    }

    private void OnTriggerStay(Collider c)
    {
        //print("collision detect");
        //print(getColObj);
        if (getColObj && grabbedObj == null)
        {
            //print("SetGrabObj");
            if (c.GetComponent<Node>())
                grabbedObj = c.gameObject;
        }

             
    }

    
}
