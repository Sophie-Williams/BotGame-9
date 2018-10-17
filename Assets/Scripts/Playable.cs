using UnityEngine;

public abstract class Playable : MonoBehaviour
{
	/// <summary>
	/// Identifier into the game state for recording (and retrieving) horizontal rotation.
	/// </summary>
	[SerializeField] public string Id;

	/// <summary>
	/// Deferred update called by the PlayerController.
	/// 
	/// This is used since we only want to handle update events for entities which are currently being played.
	/// </summary>
	public abstract void PlayableUpdate();

	/// <summary>
	/// Enter the current playable.
	/// </summary>
	public abstract void Enter();

	/// <summary>
	/// Leave the playable.
	/// 
	/// Used for cleaning up state related to playable behaviours.
	/// </summary>
	public abstract void Leave();
}
