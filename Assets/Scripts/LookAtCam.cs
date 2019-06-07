using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {

    public Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main ;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.rotation = Quaternion.LookRotation(
			transform.position - mainCamera.transform.position
		);
	}
}
