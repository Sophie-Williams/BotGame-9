using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CameraController : MonoBehaviour
{
	[SerializeField] Transform RotationAxis;
	[SerializeField] Transform CameraAttachment;
	[SerializeField] float MinHorizontalAngle;
	[SerializeField] float MaxHorizontalAngle;
	[SerializeField] float MinVerticalAngle;
	[SerializeField] float MaxVerticalAngle;
	[SerializeField] [Range(10f, 50f)] float RotationSpeed = 30f;
	public List<Event> OnMakeActive = new List<Event>();

	private float horizonalRotation;
	private float verticalRotation;
	private Vector3 initialRotation;

	void Start()
	{
		initialRotation = RotationAxis.localRotation.eulerAngles;
	}

	/// <summary>
	/// Called when entering a controller.
	/// </summary>
	public void MakeActive()
	{
		// NB: disable collider so we don't accidentally raytrace into it.
		GetComponent<Collider>().enabled = false;

		Transform CameraTransform = Camera.main.transform;
		CameraTransform.parent = CameraAttachment;
		CameraTransform.localPosition = Vector3.zero;
		CameraTransform.localRotation = Quaternion.identity;

		foreach (Event e in OnMakeActive)
		{
			StartCoroutine(e.Fire());
		}
	}

	/// <summary>
	/// Called when leaving a controller.
	/// </summary>
	public void Leave()
	{
		GetComponent<Collider>().enabled = true;
	}

	public void Rotate(float h, float v)
	{
		horizonalRotation = Mathf.Clamp(horizonalRotation + h * Time.deltaTime * RotationSpeed, MinHorizontalAngle, MaxHorizontalAngle);
		verticalRotation = Mathf.Clamp(verticalRotation + v * Time.deltaTime * RotationSpeed, MinVerticalAngle, MaxVerticalAngle);
		RotationAxis.localRotation = Quaternion.Euler(initialRotation + Vector3.up * horizonalRotation + Vector3.right * verticalRotation);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.up * MinHorizontalAngle + Vector3.right) * Vector3.forward);
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.up * MaxHorizontalAngle + Vector3.right) * Vector3.forward);
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.right * MinVerticalAngle + Vector3.right) * Vector3.forward);
		Gizmos.DrawRay(RotationAxis.position, RotationAxis.parent.rotation * Quaternion.Euler(initialRotation + Vector3.right * MaxVerticalAngle + Vector3.right) * Vector3.forward);
	}
}