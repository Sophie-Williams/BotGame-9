using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A listener that will play a dialogue when fired.
/// </summary>
public class PlayDialogueListener : MonoBehaviour, Event.Listener
{
	public List<Dialogue> Dialogue = new List<Dialogue>();
	public Event Event = null;
	public Event OnDone = null;

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
		foreach (var d in Dialogue)
		{
			yield return d.RunDialogue(Source);
		}

		if (OnDone != null)
		{
			yield return OnDone.Fire();
		}
	}
}
