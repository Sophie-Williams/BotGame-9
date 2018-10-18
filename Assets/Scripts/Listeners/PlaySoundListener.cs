using System.Collections;
using UnityEngine;

/// <summary>
/// A listeners that listens for the specified event and plays a sound.
/// </summary>
public class PlaySoundListener : MonoBehaviour, Event.Listener
{
	public AudioClip Audio;
	public Event Event = null;

	[System.NonSerialized]
	private AudioSource Source;

	public delegate void AudioPlayedAction();
	public event AudioPlayedAction OnPlayed;

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
		yield return new WaitWhile(() => Source.isPlaying);

		if (OnPlayed != null)
			OnPlayed.Invoke();

		yield break;
	}
}
