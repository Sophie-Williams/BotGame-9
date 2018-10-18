using UnityEngine;

public abstract class Playable : MonoBehaviour
{
	/// <summary>
	/// Unique identifier used to distinguish this playable.
	/// 
	/// This is used to index the playable in the game state, and must therefore be unique.
	/// </summary>
	[SerializeField] public string Id;

	/// <summary>
	/// Apply the global state to the given playable.
	/// </summary>
	public abstract void ApplyState();

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
