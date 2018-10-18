using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Global/GameState")]
public class GameState : ScriptableObject
{
	public const string DEFAULT_PLAYABLE_ID = "SteveAI";

	public string CurrentPlayableId = DEFAULT_PLAYABLE_ID;
	public Quest CurrentQuest;
	public List<string> Completed = new List<string>();
	public List<CameraState> CameraStates = new List<CameraState>();
	public List<EntityTransform> EntityTransforms = new List<EntityTransform>();

	private void OnEnable()
	{
		ActivateCurrentQuest();
	}

	public Playable CurrentPlayable
	{
		get
		{
			foreach (Playable playable in FindObjectsOfType<Playable>()) {
				if (playable.LookupId() == CurrentPlayableId)
					return playable;
			}

			throw new System.Exception("Playable ID '" + CurrentPlayableId + "' does not exist in Scene");
		}
	}

	/// <summary>
	/// Reset the game state to the initial state.
	/// </summary>
	public void Clear()
	{
		CurrentPlayableId = GameState.DEFAULT_PLAYABLE_ID;
		Completed.Clear();
		CameraStates.Clear();
		EntityTransforms.Clear();
	}

	/// <summary>
	/// Reset the game state to the initial state.
	/// </summary>
	public void Reset()
	{
		CurrentPlayableId = GameState.DEFAULT_PLAYABLE_ID;
		Completed.Clear();

		foreach (var cameraState in CameraStates)
		{
			cameraState.HorizontalRotation = 0;
			cameraState.VerticalRotation = 0;
		}

		foreach (var entityTransform in EntityTransforms)
		{
			entityTransform.ResetToInitial();
		}
	}

	/// <summary>
	/// Activate all triggers on the current quest.
	/// </summary>
	public void ActivateCurrentQuest()
	{
		CurrentQuest.Enable();
	}

	/// <summary>
	/// Get the persisted camera state.
	/// 
	/// Mutating the reference will cause the state to eventually be serialized in the global state.
	/// </summary>
	/// <param name="id">id of the camera to get</param>
	/// <returns>persisted camera state</returns>
	public CameraState GetCameraState(string id)
	{
		// NB: if no ID is specified, do not persist state.
		if (id != null && id != "")
		{
			var found = CameraStates.Find(state => state.Id == id);

			if (found != null)
			{
				return found;
			}
		}

		CameraState newState = new CameraState { Id = id, HorizontalRotation = 0f, VerticalRotation = 0f };
		CameraStates.Add(newState);
		return newState;
	}

	/// <summary>
	/// Get the entity transformation for the entity with the given ID.
	/// </summary>
	/// <returns></returns>
	public EntityTransform GetEntityTransform(string id, Transform initial)
	{
		// NB: if no ID is specified, do not persist state.
		if (id != null && id != "")
		{
			var found = EntityTransforms.Find(state => state.Id == id);

			if (found != null && !found.UseOriginalPosition)
			{
				return found;
			}
		}

		EntityTransform newState = new EntityTransform {
			Id = id,
			InitialPosition = initial.position,
			Position = initial.position,
			InitialRotation = initial.rotation,
			Rotation = initial.rotation,
		};

		EntityTransforms.Add(newState);
		return newState;
	}

	[System.Serializable]
	public class CameraState
	{
		public string Id;
		public float HorizontalRotation;
		public float VerticalRotation;
	}

	[System.Serializable]
	public class EntityTransform
	{
		public string Id;
		public bool UseOriginalPosition;
		public Quaternion InitialRotation;
		public Quaternion Rotation;
		public Vector3 InitialPosition;
		public Vector3 Position;

		/// <summary>
		/// Reset current state to initial.
		/// </summary>
		public void ResetToInitial()
		{
			Position = InitialPosition;
			Rotation = InitialRotation;
		}

		/// <summary>
		/// Apply the current transform to the given argument.
		/// </summary>
		/// <param name="transform">transform to apply to</param>
		public void ApplyTo(Transform transform)
		{
			transform.position = Position;
			transform.rotation = Rotation;
		}

		/// <summary>
		/// Update the current transform from the given argument.
		/// </summary>
		/// <param name="transform">transform to update from</param>
		public void UpdateFrom(Transform transform)
		{
			Position = transform.position;
			Rotation = transform.rotation;
		}
	}
}
