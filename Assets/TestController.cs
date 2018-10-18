using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {
	public float Speed = 10f;
	public float RotationSpeed = 30f;
	private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		var h = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(0, 0, y) * Speed * Time.deltaTime;
		Quaternion deltaRotation = Quaternion.Euler(0, h * RotationSpeed, 0);

		rigidBody.MovePosition(transform.position + (transform.rotation * movement));
		rigidBody.MoveRotation(transform.rotation * deltaRotation);
	}
}
