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

    public bool hideconnections;

    public bool dontRepel;
    public bool dontBeRepelled;
    public bool dontAttract;
    public bool dontBeAttracted;

    //public Vector3 savedPosition;
    //public Vector3 throwVelocity;

    public List<Edge> attractionlist = new List<Edge>();

    public Material materialStandard;
    public Material materialHighlighted;
    public Material materialInfected;
    public Material materialEndangered;
    public Material materialImmune;
    public Material materialEvil;
    public Material materialEvilMaster;

    public bool IsEndangered()
    {
        return attractionlist.Any(x=>x.Other(this).infection != null);
    }

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


    public override void Start()
    {
        base.Start();
        namefield.text = name;
        RandomPosition();
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
        graph.infectionManager.InstantiateInfection(this);
    }
    #endregion


    public void CheckForBreak()
    {
        Node otherNode = null;
        if (graph.grabLeft == this && graph.grabRight != null)
            otherNode = graph.grabRight;
        else if (graph.grabRight == this && graph.grabLeft != null)
            otherNode = graph.grabLeft;

        Edge connectingEdge = attractionlist.Find(x => x.Other(this) == otherNode);
        if (connectingEdge != null)
        {
            float currentDistance = Vector3.Distance(
                transform.position, 
                otherNode.transform.position
            );

            float strain = (float)(currentDistance / (graph.initialGrabDistance * graph.ripFactor));
            connectingEdge.Strain(strain);

            if (currentDistance > graph.initialGrabDistance * graph.ripFactor)
            {
                Debug.Log("BREAK!");
                Destroy(connectingEdge.gameObject);

                if (attractionlist.Count == 0)
                    Separate();
            }
        }
    }
    
    protected void Separate()
    {
        graph.nodes.Remove(this);
        graph.infectionManager.AddToEvilNodes(this);
    }

    public void RandomPosition()
    {
        transform.localPosition = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
    }

    // public void Accelerate(Vector3 force)
    // {
    //     throwVelocity = force;
    // }


    #region physics
    public override void UpdatePosition()
    {
        // if (GrabbedBy() != null)
        // {
        //     transform.position = GrabbedBy().transform.position;
        // }
        // else
        // {
            CalculateForces();
        // }
    }
    
    protected void CalculateForces()
    {
        // Rigidbody rb = GetComponent<Rigidbody> ();
        // rb.velocity = Vector3.zero;

        // REPULSION
        if(!dontBeRepelled)
            foreach (Node rn in graph.nodes.Where(x=>x != this && !x.dontRepel))
                transform.localPosition += CalcRepulsion(rn) * graph.speed;

        //ATTRACTION
        if(!dontBeAttracted)
            foreach (Edge e in attractionlist.Where(x=>!x.Other(this).dontAttract))
                transform.localPosition += CalcAttraction (e.Other (this), 1) * graph.speed; // unweighted

        //ATTRACTION TO CENTER
        transform.localPosition += CalcAttractionToCenter(graph.gravity) * graph.speed;

        //Debug.Log(name + " velocity set to " + rb.velocity.ToString());
    }

    protected Vector3 CalcAttraction(Node otherNode, float weight)
    {
        return CalcAttraction(
            transform.localPosition,
            otherNode.transform.localPosition,
            weight
        );
    }

    protected Vector3 CalcAttraction(Vector3 thisPosition, Vector3 otherPosition, float weight)
    {
        // weighted LinLog
        return otherPosition-thisPosition.normalized * // direction
            weight * Mathf.Log(1+Vector3.Distance(thisPosition, otherPosition)) * // force
            graph.attractionFactor *
            0.5f; //halved

    }

    protected Vector3 CalcRepulsion(Node otherNode)
    {
		// Vector3 a = transform.position;
		// Vector3 b = otherNode.transform.position;

        // float distance = Vector3.Distance(a, b);

        return CalcRepulsion(
            transform.localPosition,
            otherNode.transform.localPosition,
            graph.scale
        );
    }
    public Vector3 CalcRepulsion(Vector3 thisPosition, Vector3 otherPosition, float scale)
    {
		// // Coulomb's Law: F = k(Qq/r^2)
        // float force = -(graph.scale * graph.scale) / Vector3.Distance (a, b);

		// //float force = (graph.repulsionNodes / (distance * Vector3.Distance (a, b)));

		// return ((b - a).normalized) * force * 0.5f;

        return
            (otherPosition-thisPosition).normalized * // direction
             -(scale * scale) / Vector3.Distance(thisPosition, otherPosition) // force
             *graph.repulsionFactor
             * 0.5f; //halved;
    }

    protected Vector3 CalcAttractionToCenter(float gravity)
    {
        //Debug.Log(((-transform.localPosition.normalized)*gravity).ToString());
        return  
            -transform.localPosition.normalized // direction
            * gravity;// * Mathf.Log (transform.localPosition.magnitude); // force;

		// if (float.IsNaN (returnvector.x) | float.IsNaN (returnvector.y) | float.IsNaN (returnvector.z))
		// 	return Vector3.zero;
		// else
		//	return returnvector;
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
        else if (IsEndangered()) // endangered
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
