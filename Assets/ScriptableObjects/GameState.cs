using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Global/GameState")]
public class GameState : ScriptableObject
{
	public Quest CurrentQuest;
	public List<string> Completed = new List<string>();

	private void OnEnable()
	{
		ActivateCurrentQuest();
	}

	/// <summary>
	/// Activate all triggers on the current quest.
	/// </summary>
	public void ActivateCurrentQuest()
	{
		CurrentQuest.Enable();
	}
}
