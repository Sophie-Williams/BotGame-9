using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Event))]
public class EventEditor : Editor
{
	Event e;

	public void OnEnable()
	{
		e = target as Event;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Test Fire"))
		{
			var player = FindObjectOfType<PlayerController>();
			player.StartCoroutine(e.Fire());
		}
	}
}
