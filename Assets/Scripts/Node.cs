using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Node : GraphObject
{
    public int id;
    public Text namefield;

    public Infection infection;

    public bool endangered = false;

    public bool hideconnections;
    public bool dontRepel;

    //public Vector3 savedPosition;

    public Vector3 forceVelocity;
    //public Vector3 throwVelocity;

    public List<Node> repulsionlist = new List<Node>();
    public List<Edge> attractionlist = new List<Edge>();
    public List<Node> connectedNodes = new List<Node>();

    public Material materialStandard;
    public Material materialHighlighted;
    public Material materialInfected;
    public Material materialEndangered;
    public Material materialImmune;
    public Material materialEvil;
    public Material materialEvilMaster;

    #region showandhide

    public override void Hide()
    {
        base.Hide();
        GetComponent<Renderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        CanvasGroup cv = GetComponent<CanvasGroup>();
        cv.alpha = 0;
        cv.blocksRaycasts = false;
        cv.interactable = false;
    }

    public override void Show()
    {
        base.Show();
        GetComponent<Renderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;

        CanvasGroup cv = GetComponent<CanvasGroup>();
        cv.alpha = 1;
        cv.blocksRaycasts = true;
        cv.interactable = true;

        namefield.transform.localPosition = Vector3.zero;
    }

    public GameObject GrabbedBy()
    {
        return null;
        // if (graph.leftController.GetComponent<ViveGrab>().grabbedObj == gameObject) { // GRABBED BY LEFT
        // 	grabbedBy = graph.leftController.gameObject;
        // } else if (graph.rightController.GetComponent<ViveGrab>().grabbedObj == gameObject) { // GRABBED BY RIGHT
        // 	grabbedBy = graph.rightController.gameObject;
        // } else // NOT GRABBED
        // 	grabbedBy = null;
    }

    public void HideConnections()
    {
        hideconnections = true;
    }

    public void ShowConnections()
    {
        hideconnections = false;
    }

    public void Remove()
    {
        dontRepel = true;
        HideConnections();
        Invoke("Destroy", 1.5f);
    }

    protected virtual void Destroy() // IS INVOKED
    {
        Destroy(gameObject);
    }

    #endregion


    public void Start()
    {
        namefield.text = name;
        ResetPosition();
        foreach (Edge e in attractionlist)
        {
            connectedNodes.Add(e.Other(this));
        }
    }
    
    public override void Update()
    {
        base.Update();

        if(GrabbedBy() != null)
            CheckForBreak();
    }

    #region infection


    public void CheckForInfection(Infection infector)
    {
        if (!infection 
            && Random.value > infector.infectionChance)
        {
            BecomeInfected();
        }
    }

    public void BecomeInfected()
    {
        Debug.Log(name + " became infected at " + Time.time);
        graph.infectionManager.InstantiateInfection(this, Time.time);
    }
    #endregion


    public void CheckForBreak()
    {
        Node otherNode = null;
        if (graph.grabLeft == this && graph.grabRight != null)
            otherNode = graph.grabRight;
        else if (graph.grabRight == this && graph.grabLeft != null)
            otherNode = graph.grabLeft;

        if (connectedNodes.Contains(otherNode))
        {
            float currentDistance = Vector3.Distance(
                transform.position, 
                otherNode.transform.position
            );

            Edge findEdge = attractionlist.Find(x => x.Other(this) == otherNode);

            float strain = (float)(currentDistance / (graph.initialGrabDistance * graph.ripFactor));
            findEdge.Strain(strain);

            if (currentDistance > graph.initialGrabDistance * graph.ripFactor)
            {
                Debug.Log("BREAK!");
                BreakConnection(otherNode, findEdge);

                if (attractionlist.Count == 0)
                    Separate();

            }
        }
    }

    protected void BreakConnection(Node otherNode, Edge connectingEdge)
    {
        otherNode.connectedNodes.Remove(this);
        connectedNodes.Remove(otherNode);

        if (otherNode.endangered)
            otherNode.endangered = false;

        Destroy(connectingEdge.gameObject);
    }
    
    protected void Separate()
    {
        graph.nodes.Remove(this);
        graph.infectionManager.AddToEvilNodes(this);
    }

    public void RefreshRepulsionList()
    {
        repulsionlist.Clear();
        GameObject[] allnodes = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject go in allnodes)
        {
            if (go != gameObject)
                repulsionlist.Add(go.GetComponent<Node>());
        }
    }

    public void ResetPosition()
    {
        transform.localPosition = graph.center.transform.position + 
            new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
    }

    // public void Accelerate(Vector3 force)
    // {
    //     throwVelocity = force;
    // }


    #region physics
    public override void UpdatePosition()
    {
        base.UpdatePosition();

        if (GrabbedBy() != null)
        {
            //Debug.Log (name + " grabbed by " + grabbedBy);
            transform.position = GrabbedBy().transform.position;
        }
        else
        {
            CalculateForces();

            // if (graph.infectionManager.evilMaster == this)
            // {
            //     CalculateEvilDrag();
            // }

            if (!graph.gameOver)
            {
                ApplyForces();
            }
        }
    }
    
    protected void CalculateForces()
    {
        forceVelocity = Vector3.zero;

        // REPULSION
        foreach (Node rn in repulsionlist)
            forceVelocity += CalcRepulsion(rn);

        //ATTRACTION
        foreach (Edge e in attractionlist)
            forceVelocity += CalcAttraction(e.Other(this), e.weight);

        //ATTRACTION TO CENTER
        forceVelocity += CalcAttractionToCenter();
    }

    protected Vector3 CalcAttractionToCenter()
    {
        Vector3 a = transform.position;
        Vector3 b = graph.center.transform.position;
        return (b - a).normalized * graph.attraction * (Vector3.Distance(a, b) / graph.springLength);
    }

    protected Vector3 CalcAttraction(Node otherNode, float weight)
    {
        if (otherNode)
        {
            Vector3 a = transform.localPosition;
            Vector3 b = otherNode.transform.localPosition;

            return (b - a).normalized * 
                (graph.attraction + weight) * 
                    (Vector3.Distance(a, b) / graph.springLength);

        }
        else
            return Vector3.zero;
    }

    protected Vector3 CalcRepulsion(Node otherNode)
    {
        if (!dontRepel)
        {
            // Coulomb's Law: F = k(Qq/r^2)
            float distance = Vector3.Distance(transform.localPosition, otherNode.transform.localPosition);
            Vector3 returnvector = ((transform.localPosition - otherNode.transform.localPosition).normalized * graph.repulsion) / (distance * distance);

            if (!float.IsNaN(returnvector.x) && !float.IsNaN(returnvector.y) && !float.IsNaN(returnvector.z))
                return returnvector;
            else
                return Vector3.zero;
        }
        else
            return Vector3.zero;
    }

    // protected void CalculateEvilDrag()
    // {
    //     masterVelocity = Vector3.zero;

    //     if (graph.infectionManager.infectedNodes.Count > 0)
    //     {
    //         Vector3 attractPosition = graph.infectionManager.infectedNodes.First().transform.position;
    //         masterVelocity += (attractPosition - transform.position).normalized * graph.masterSpeed;
    //         // Debug.Log("ATTRACT " + name + " to " + graph.infectedNodes.First().name + " with " + masterVelocity.ToString());
    //     }
    // }

    protected void ApplyForces()
    {
        if (!float.IsNaN(forceVelocity.x) && !float.IsNaN(forceVelocity.y) && !float.IsNaN(forceVelocity.z))
        {
            //Debug.Log("Apply Forces to " + name);
            transform.position += forceVelocity * graph.damping * Time.deltaTime;
            //transform.position += throwVelocity * Time.deltaTime;
            //transform.position += masterVelocity * Time.deltaTime;

            //savedPosition = transform.localPosition;

            //throwVelocity *= 0.8f;
        }
        else
            Debug.LogError("Velocity Error: " + name + " " + forceVelocity.ToString());
    }

    #endregion physics

    #region appearance
    public override void UpdateAppearance()
    {
        base.UpdateAppearance();
        if (graph.infectionManager.evilMaster == this) // evilMaster
        {
            SetMaterial(materialEvilMaster);
            SetLight(Color.red);
        }
        else if (graph.infectionManager.evilNodes.Contains(this) && !graph.infectionManager.evilMaster == this) // evil
        {
            SetMaterial(materialEvil);
            SetLight(Color.black);
        }
        else if (infection) // infected
        {
            SetMaterial(materialInfected);
            SetLight(Color.red);
        }
        else if (endangered) // endangered
        {
            SetMaterial(materialEndangered);//Material.Lerp(materialStandard, materialEndangered, Mathf.FloorToInt(Time.time) % 2));
            SetLight(Color.Lerp(Color.yellow, Color.red, Mathf.FloorToInt(Time.time) % 2));
        }
        else if (GrabbedBy() != null) // highLighted
        {
            SetMaterial(materialHighlighted);
            SetLight(Color.white);
        }
        else // standard
        {
            SetMaterial(materialStandard);
            SetLight(Color.cyan);
        }
    }

    public void SetMaterial(Material material)
    {
        GetComponent<Renderer>().material = material;
    }

    public void SetLight(Color color)
    {
        GetComponent<Light>().color = color;
    }
    #endregion appearance

    public override void OnDestroy()
    {
        graph.nodes.Remove(this);
    }
}
