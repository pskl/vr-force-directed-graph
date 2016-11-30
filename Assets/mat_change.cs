using UnityEngine;
using System.Collections;

public class MatChange : MonoBehaviour {

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
    }

    public void ChangeTheColor()
    {
        nodeRenderer.material.color = Color.Lerp(startColor, selectColor, Mathf.PingPong(Time.time, 1));
        myLight.color = selectColor;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
