using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global/Subtitle System")]
public class SubtitleSystem : ScriptableObject
{
	public bool Enabled;
	public bool ClosedCaptions = true;
	public DialogueSource DefaultSource;

	public delegate void OnSnippetHandler(DialogueSource source, Dialogue.Snippet snippet);
	public delegate void OnClearTextHandler();

	public event OnSnippetHandler OnShowText;
	public event OnClearTextHandler OnClearText;

	private DialogueSource lastSource = null;

	/// <summary>
	/// Run the given snippet, showing it for the given period of time then hiding it.
	/// </summary>
	/// <param name="snippet"></param>
	/// <returns></returns>
	public IEnumerator RunDialogueSnippet(Dialogue.Snippet snippet)
	{
		if (!Enabled)
		{
			// NB: anything playing audio should wait until that has completed.
			yield break;
		}

		// In case there is a delay we want to clear the text before waiting.
		if (snippet.Delay > 0)
		{
			Clear();
			yield return new WaitForSeconds(snippet.Delay);
		}

		// Empty snippet implies clear.
		if (snippet.Text == "")
		{
			Clear();
			yield return new WaitForSeconds(snippet.Time);
			yield break;
		}

		if (!ClosedCaptions && snippet.ClosedCaption || OnShowText == null)
		{
			yield return new WaitForSeconds(snippet.Time);
			yield break;
		}

		OnShowText.Invoke(SetSource(snippet.Source), snippet);
		yield return new WaitForSeconds(snippet.Time);
	}

	public void Clear()
	{
		if (OnClearText != null)
		{
			OnClearText.Invoke();
		}

		lastSource = null;
	}

	/// <summary>
	/// Update the current source and color (if applicable).
	/// </summary>
	private DialogueSource SetSource(DialogueSource source)
	{
		DialogueSource newSource = null;

		if (source == null)
		{
			source = DefaultSource;
		}

		if (lastSource != source)
		{
			lastSource = source;
			newSource = source;
		}

		return newSource;
	}
}
