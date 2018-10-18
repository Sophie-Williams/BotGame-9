using System.Collections;
using UnityEngine;

/// <summary>
/// A listener that will play a dialogue when fired.
/// </summary>
public class PlayDialogueListener : MonoBehaviour, Event.Listener
{
	public Dialogue Dialogue;
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
		if (Dialogue != null)
		{
			yield return Dialogue.RunDialogue(Source);
		}

		if (OnDone != null)
		{
			yield return OnDone.Fire();
		}
	}
}
