using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[SerializeField] CameraController CurrentCamera;

	// Update is called once per frame
	void Update () {
		CurrentCamera.Rotate(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			CameraController nextCamera = hit.transform.GetComponent<CameraController>();
			if (nextCamera != null) {
				if (Input.GetMouseButton(0)) {
					nextCamera.MakeActive();
					CurrentCamera = nextCamera;
				}
			}
		}
	}
}
