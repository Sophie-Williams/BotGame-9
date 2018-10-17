using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Global/GameState")]
public class GameState : ScriptableObject
{
	public string CurrentPlayableId = "SteveAI";
	public Quest CurrentQuest;
	public List<string> Completed = new List<string>();
	public List<CameraState> CameraStates = new List<CameraState>();

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

	[System.Serializable]
	public class CameraState
	{
		public string Id;
		public float HorizontalRotation;
		public float VerticalRotation;
	}
}
