using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CameraController : Playable
{
	[SerializeField] GameState State;

	[SerializeField] Transform RotationAxis;
	[SerializeField] Transform CameraAttachment;
	[SerializeField] float MinHorizontalAngle;
	[SerializeField] float MaxHorizontalAngle;
	[SerializeField] float MinVerticalAngle;
	[SerializeField] float MaxVerticalAngle;
	[SerializeField] [Range(10f, 50f)] float RotationSpeed = 30f;

	public delegate void CameraEnterAction();
	public event CameraEnterAction OnEnter;

	private GameState.CameraState cameraState;
	private Vector3 initialRotation;

	void Start()
	{
		var info = GetComponent<PlayableInfo>();
		initialRotation = RotationAxis.localRotation.eulerAngles;
		cameraState = State.GetCameraState(info.Id);
		ApplyState();
	}

	/// <summary>
	/// Cameras can be rotated by using horizontal and vertical inputs.
	/// </summary>
	public override void PlayableUpdate()
	{
		Rotate(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
	}

	/// <summary>
	/// Called when entering a controller.
	/// </summary>
	public override void Enter()
	{
		// NB: disable collider to prevent self from activating UI overlay.
		GetComponent<Collider>().enabled = false;

		Transform CameraTransform = Camera.main.transform;
		CameraTransform.parent = CameraAttachment;
		CameraTransform.localPosition = Vector3.zero;
		CameraTransform.localRotation = Quaternion.identity;

		if (OnEnter != null)
			OnEnter.Invoke();
	}

	/// <summary>
	/// Called when leaving a controller.
	/// </summary>
	public override void Leave()
	{
		GetComponent<Collider>().enabled = true;
	}

	public void Rotate(float h, float v)
	{
		cameraState.HorizontalRotation = Mathf.Clamp(cameraState.HorizontalRotation + h * Time.deltaTime * RotationSpeed, MinHorizontalAngle, MaxHorizontalAngle);
		cameraState.VerticalRotation = Mathf.Clamp(cameraState.VerticalRotation + v * Time.deltaTime * RotationSpeed, MinVerticalAngle, MaxVerticalAngle);
		ApplyState();
	}

	public override void ApplyState()
	{
		RotationAxis.localRotation = Quaternion.Euler(initialRotation + Vector3.up * cameraState.HorizontalRotation + Vector3.right * cameraState.VerticalRotation);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.up * MinHorizontalAngle + Vector3.right) * Vector3.forward);
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.up * MaxHorizontalAngle + Vector3.right) * Vector3.forward);
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.right * MinVerticalAngle + Vector3.right) * Vector3.forward);
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.right * MaxVerticalAngle + Vector3.right) * Vector3.forward);
	}
}