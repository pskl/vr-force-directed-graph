using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {

    public Transform myCam;

	// Use this for initialization
	void Start () {
        myCam = GameObject.Find("Camera (eye)").transform;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.rotation = Quaternion.LookRotation(transform.position - myCam.position);
	}
}
