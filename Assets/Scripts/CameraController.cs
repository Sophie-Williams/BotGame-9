using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField] Transform CameraRotation;
	[SerializeField] Transform CameraAttachment;
	[SerializeField] float MinHorizontalAngle; 
	[SerializeField] float MaxHorizontalAngle;
	[SerializeField] float MinVerticalAngle;
	[SerializeField] float MaxVerticalAngle;

	[SerializeField][Range(10f, 50f)] float RotationSpeed = 30f;

	private float currentHorizonalRotation;
	private float currentVerticalRotation;

	void Start() {
		currentHorizonalRotation = CameraRotation.localEulerAngles.y;
		currentVerticalRotation = CameraRotation.localEulerAngles.x;
	}

	public void MakeActive(){
		Transform CameraTransform = Camera.main.transform;
		CameraTransform.parent = CameraAttachment;
		CameraTransform.localPosition = Vector3.zero;
		CameraTransform.localRotation = Quaternion.identity;
	}

	public void Rotate(float h, float v) {
		currentHorizonalRotation = Mathf.Clamp(currentHorizonalRotation + h*Time.deltaTime*RotationSpeed, MinHorizontalAngle, MaxHorizontalAngle);
		currentVerticalRotation = Mathf.Clamp(currentVerticalRotation + v*Time.deltaTime*RotationSpeed, MinVerticalAngle, MaxVerticalAngle);

		CameraRotation.localRotation = Quaternion.Euler(currentVerticalRotation, currentHorizonalRotation, 0f);
	}


}
