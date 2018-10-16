using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event", order = 0)]
public class Event : ScriptableObject
{
	[System.NonSerialized]
	private List<EventListener> listeners = new List<EventListener>();

	/// <summary>
	/// Subscribe to all future events.
	/// </summary>
	/// <param name="listener"></param>
	public void Subscribe(EventListener listener)
	{
		listeners.Add(listener);
	}

	/// <summary>
	/// Unsubscribe from all future events.
	/// </summary>
	/// <param name="listener"></param>
	public void Unsubscribe(EventListener listener)
	{
		listeners.Remove(listener);
	}

	/// <summary>
	/// Fire this event and all associated listeners.
	/// </summary>
	/// <returns></returns>
	public IEnumerator Fire()
	{
		foreach (EventListener listener in listeners)
		{
			yield return listener.Fire();
		}
	}
}

public interface EventListener
{
	IEnumerator Fire();
}
