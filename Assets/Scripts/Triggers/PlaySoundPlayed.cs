using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Fire an event when a sound has been played.
/// </summary>
public class PlaySoundPlayed : MonoBehaviour {
	/// <summary>
	/// Events to fire.
	/// </summary>
	[SerializeField] List<Event> Events = new List<Event>();

	void Start() {
		GetComponent<PlaySoundListener>().OnPlayed += this.OnPlayed;
	}

	void OnPlayed() {
		foreach (Event e in Events) {
			StartCoroutine(e.Fire());
		}
	}
}
