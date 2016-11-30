using UnityEngine;
using System.Collections;

public class MateralChange : MonoBehaviour {
    public Color startColor;
    public Color selectColor;
    private Renderer nodeRenderer;
    private Light myLight;

    // Use this for initialization
    void Start()
    {
        nodeRenderer = GetComponent<Renderer>();
        nodeRenderer.material.color = startColor;
        myLight = GetComponent<Light>();
        myLight.color = startColor;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
