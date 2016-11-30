using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
	public static Graph instance;
	public SteamVR_TrackedObject leftController;
	public SteamVR_TrackedObject rightController;
	public GameObject center;
	public float repulsion;
	public float attraction;
	public float springLength;
	public float damping;

	public List<Node> nodes = new List<Node> ();
	public List<Edge> edges = new List<Edge> ();


	public void Awake ()
	{
		instance = this;
	}

	public void Update ()
	{
		var device = SteamVR_Controller.Input ((int)leftController.index);
		if (device.GetTouchDown (Valve.VR.EVRButtonId.k_EButton_DPad_Down) && repulsion > 1)
			repulsion -= 0.5f;
		else if (device.GetTouchUp (Valve.VR.EVRButtonId.k_EButton_DPad_Up) && repulsion < 100)
			repulsion += 0.5f;
		if (device.GetTouchDown (Valve.VR.EVRButtonId.k_EButton_DPad_Down) && repulsion > 1)
			repulsion -= 0.5f;
		else if (device.GetTouchUp (Valve.VR.EVRButtonId.k_EButton_DPad_Up) && repulsion < 100)
			repulsion += 0.5f;		
	}

}