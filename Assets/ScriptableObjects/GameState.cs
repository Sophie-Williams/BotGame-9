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
	public List<BotState> BotStates = new List<BotState>();

	private void OnEnable()
	{
		ActivateCurrentQuest();
	}

	public Playable CurrentPlayable
	{
		get
		{
			foreach (Playable playable in FindObjectsOfType<Playable>()) {
				if (playable.Id == CurrentPlayableId)
					return playable;
			}

			throw new System.Exception("Playable ID '" + CurrentPlayableId + "' does not exist in Scene");
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
	/// Get bot state if present, or set it to the initial state.
	/// </summary>
	/// <param name="id">id of the bot to get</param>
	/// <param name="initial">the initial state to set if not present</param>
	/// <returns></returns>
	public BotState GetOrSetBotState(string id, Vector3 initialPosition, Quaternion initialRotation)
	{
		// NB: if no ID is specified, do not persist state.
		if (id != null && id != "")
		{
			var found = BotStates.Find(state => state.Id == id);

			if (found != null && !found.UseOriginalPosition)
			{
				return found;
			}
		}

		BotState newState = new BotState { Id = id, Position = initialPosition, Rotation = initialRotation };
		BotStates.Add(newState);
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
	public class BotState
	{
		public string Id;
		public bool UseOriginalPosition;
		public Quaternion Rotation;
		public Vector3 Position;
	}
}
