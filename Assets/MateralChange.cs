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

    public void ChangeTheColor()
    {
        print("change color");
        nodeRenderer.material.SetColor("_RimColor", selectColor);
        nodeRenderer.material.SetColor("_Color", selectColor);
        myLight.color = selectColor;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
