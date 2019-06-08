using UnityEngine;
using System.Collections;


public class Edge : GraphObject
{
	public LineRenderer lineRenderer;

    public Node node1;
    public Node node2;
    public float weight;

    public bool endangered;
    public bool infected;
    public bool straining;
    public bool nodrag;

    public Material materialStandard;
	public Material materialEndangered;
	public Material materialInfected;
    public Material materialStrained;

    public Node Other(Node ask)
    {
        if (ask == node1)
            return node2;
        else if (ask == node2)
            return node1;
        else
            return null;
    }

    public void Strain(float factor)
    {
        straining = true;
        lineRenderer.material.Lerp(
            materialStandard,
            materialStrained,
            factor
        );
    }

    public override void UpdatePosition()
    {
		base.UpdatePosition();
        lineRenderer.SetPosition(
            0,
            node1.transform.position
        );
        lineRenderer.SetPosition(
            1,
            node2.transform.position
        );
        transform.position = Vector3.Lerp(
            node1.transform.position,
            node2.transform.position,
            0.5f
        );
    }

    public override void UpdateAppearance()
    {
		base.UpdateAppearance();
        if (!straining)
        {
            if (graph.gameOver)
            {
                lineRenderer.material = materialInfected;
            }
            else if (graph.infectionManager.evilNodes.Contains(node1) | graph.infectionManager.evilNodes.Contains(node2))
            {
                lineRenderer.material = materialStrained;
            }
            else if (endangered)
            {
                lineRenderer.material.Lerp(materialStandard, materialEndangered, Mathf.FloorToInt(Time.time) % 2);
            }
            else
            {
                lineRenderer.material = materialStandard;
            }
        }

        straining = false;
    }

	public override void OnDestroy()
	{
		graph.edges.Remove(this);
		node1.attractionlist.Remove(this);
		node2.attractionlist.Remove(this);
	}
}
