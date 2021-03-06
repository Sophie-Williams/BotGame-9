﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameState))]
public class GameStateEditor : Editor
{
	GameState state;

	public void OnEnable()
	{
		state = target as GameState;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Clear"))
		{
			state.Clear();
			EditorUtility.SetDirty(state);
		}

		if (GUILayout.Button("Reset All Quests"))
		{

			state.Completed.Clear();
			EditorUtility.SetDirty(state);
		}

		if (GUILayout.Button("Reset Current Quest"))
		{
			state.Completed.Remove(state.CurrentQuest.Id);

			foreach (Quest.QuestGoal goal in state.CurrentQuest.Goals)
			{
				state.Completed.Remove(state.CurrentQuest.Id + "/" + goal.Id);
			}

			EditorUtility.SetDirty(state);
		}
	}
}
