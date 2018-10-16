using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerController : MonoBehaviour, EventListener
{
	public AudioClip Audio;
	public Event Event = null;

	[System.NonSerialized]
	private AudioSource Source;

	void Start()
	{

		if (Event != null)
		{
			Source = GetComponent<AudioSource>();
			Event.Subscribe(this);
		}
	}

	private void OnDestroy()
	{
		if (Event != null)
		{
			Event.Unsubscribe(this);
		}
	}

	public IEnumerator Fire()
	{
		Source.clip = Audio;
		Source.Play();
		yield break;
	}
}
