using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Fire an event when we enter a camera.
/// </summary>
public class CameraEnter : MonoBehaviour {
	/// <summary>
	/// Events to fire.
	/// </summary>
	[SerializeField] List<Event> Events = new List<Event>();

	void Start() {
		GetComponent<CameraController>().OnEnter += this.OnEnter;
	}

	void OnEnter() {
		foreach (Event e in Events) {
			StartCoroutine(e.Fire());
		}
	}
}
