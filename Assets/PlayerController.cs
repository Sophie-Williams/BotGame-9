using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[SerializeField] CameraController CurrentCamera;

	private GameObject currentTarget = null;

	void Start() {
		FindObjectOfType<HoverController>().OnHoverUpdate += this.OnHoverUpdate;
	}

	void Update () {
		CurrentCamera.Rotate(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));

		if (Input.GetMouseButton(0) && currentTarget != null) {
			CameraController nextCamera = currentTarget.GetComponent<CameraController>();
			if (nextCamera != null) {
				if (Input.GetMouseButton(0)) {
					nextCamera.MakeActive();
					CurrentCamera = nextCamera;
				}
			}
		}
	}

	public void OnHoverUpdate(GameObject obj) {
		currentTarget = obj;
	}
}
