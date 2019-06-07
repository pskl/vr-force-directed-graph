using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class GraphObject : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Graph graph;
    public bool hidden;
    public bool highlighted;
    
    public virtual void Hide()
    {
		hidden = true;
    }

    public virtual void Show()
    {
        hidden = false;
    }

    public virtual void Update()
    {
        UpdatePosition();
        UpdateAppearance();
    }

    public virtual void UpdatePosition()
    {

    }

    public virtual void UpdateAppearance()
    {

    }
    
	#region events


	public virtual void OnPointerEnter (PointerEventData eventData)
	{
		Debug.Log ("Enter" + name);
	}

	public virtual void OnPointerExit (PointerEventData eventData)
	{
		Debug.Log ("Exit" + name);
	}

	public virtual void OnPointerClick (PointerEventData eventData)
	{
		
	}

	public virtual void OnBeginDrag (PointerEventData eventData)
	{
	}

	public virtual void OnDrag (PointerEventData eventData)
	{
		//transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
	}

	public virtual void OnEndDrag (PointerEventData eventData)
	{
		
	}

	#endregion events
}
