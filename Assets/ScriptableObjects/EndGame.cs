using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global/End Game", order = 4)]
public class EndGame : ScriptableObject, Event.Listener
{
	public GameState State;

	/// <summary>
	/// Events that will end the game.
	/// </summary>
	public List<Event> Events = new List<Event>();

	private void OnEnable()
	{
		foreach (var e in Events)
		{
			e.Subscribe(this);
		}
	}

	public IEnumerator Fire()
	{
		State.Reset();

		var player = FindObjectOfType<PlayerController>();

		if (player != null)
		{
			player.Setup();
		}

		yield break;
	}
}
