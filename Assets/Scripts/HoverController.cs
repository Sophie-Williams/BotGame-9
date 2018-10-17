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
		if (Physics.Raycast(ray, out hit, 100f, LayerMask.value)){
			// NB: only for blocking interactions.
			if (hit.transform.gameObject.tag == "InteractionCollider")
			{
				return;
			}

			Debug.Log(hit.transform.gameObject);

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
