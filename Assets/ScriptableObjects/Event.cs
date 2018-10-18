using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event", order = 0)]
public class Event : ScriptableObject
{
	[System.NonSerialized]
	private List<Listener> listeners = new List<Listener>();

	/// <summary>
	/// Subscribe to all future events.
	/// </summary>
	/// <param name="listener"></param>
	public void Subscribe(Listener listener)
	{
		listeners.Add(listener);
	}

	/// <summary>
	/// Unsubscribe from all future events.
	/// </summary>
	/// <param name="listener"></param>
	public void Unsubscribe(Listener listener)
	{
		listeners.Remove(listener);
	}

	/// <summary>
	/// Fire this event and all associated listeners.
	/// </summary>
	/// <returns></returns>
	public IEnumerator Fire()
	{
		Debug.Log("Firing Event: " + this);

		foreach (Listener listener in listeners)
		{
			yield return listener.Fire();
		}
	}

	public interface Listener
	{
		IEnumerator Fire();
	}
}
