using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFade : MonoBehaviour {

    public Text myText;
    public Transform myCam;
    public float maxDistanceInMeter;

	// Use this for initialization
	void Start () {
        myCam = GameObject.Find("Camera (eye)").transform;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        float disToMe = Vector3.Distance(transform.position, myCam.position);
        Color newColor = new Color(myText.color.r, myText.color.g, myText.color.b, 1 - disToMe/maxDistanceInMeter);
        myText.color = newColor;
	
	}
}
