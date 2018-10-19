using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
	Dialogue dialogue;
	GameObject gameObject;
	AudioSource source;
	EditorCoroutine editorCoroutine;

	public void OnEnable()
	{
		dialogue = target as Dialogue;

		gameObject = EditorUtility.CreateGameObjectWithHideFlags(
			"Dialogue Preview",
			HideFlags.HideAndDontSave,
			typeof(AudioSource)
		);

		source = gameObject.GetComponent<AudioSource>();
		editorCoroutine = EditorCoroutine.Setup();

		dialogue.SubtitleSystem.OnShowText += this.OnShowText;
		dialogue.SubtitleSystem.OnClearText += this.OnClearText;
	}

	private void OnDisable()
	{
		DestroyImmediate(gameObject);
		editorCoroutine.Destroy();

		dialogue.SubtitleSystem.OnShowText -= this.OnShowText;
		dialogue.SubtitleSystem.OnClearText -= this.OnClearText;
	}

	private void OnShowText(DialogueSource source, Dialogue.Snippet snippet)
	{
		Debug.Log(snippet.Render(source));
	}

	private void OnShowClosedCaption(DialogueSource source, Dialogue.Snippet snippet)
	{
		if (source != null)
		{
			Debug.Log("Source: " + source.Name);
		}

		Debug.Log("Closed Caption: " + snippet.Text);
	}

	private void OnClearText()
	{
		Debug.Log("*clear*");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Test in Console"))
		{
			editorCoroutine.RunCoroutine(dialogue.RunDialogue(source));
		}
	}
}
