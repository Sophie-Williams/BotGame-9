using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour {
	public delegate void HoverUpdateAction(GameObject obj);
	public event HoverUpdateAction OnHoverUpdate;


	[SerializeField] LayerMask LayerMask;
	private GameObject currentHover;

    public GameObject CurrentHover { get { return currentHover; } }

    void Start () {
		currentHover = null;
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.value)){
			if (hit.transform.gameObject != currentHover) {
				currentHover = hit.transform.gameObject;
				OnHoverUpdate.Invoke(currentHover);
			}
		} else {
			if (currentHover != null) {
				currentHover = null;
				OnHoverUpdate.Invoke(null);
			}
		}	
	}
}
