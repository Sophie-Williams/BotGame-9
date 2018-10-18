using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global/Subtitle System")]
public class SubtitleSystem : ScriptableObject
{
	public bool Enabled;
	public bool ClosedCaptions = true;

	public delegate void OnSnippetHandler(Dialogue.Snippet snippet);
	public delegate void OnClearTextHandler();

	public event OnSnippetHandler OnShowText;
	public event OnSnippetHandler OnShowClosedCaption;
	public event OnClearTextHandler OnClearText;

	/// <summary>
	/// Run the given snippet, showing it for the given period of time then hiding it.
	/// </summary>
	/// <param name="snippet"></param>
	/// <returns></returns>
	public IEnumerator RunDialogueSnippet(Dialogue dialogue, Dialogue.Snippet snippet)
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
		}
		else
		{
			if (snippet.ClosedCaption)
			{
				if (ClosedCaptions && OnShowClosedCaption != null)
				{
					OnShowClosedCaption.Invoke(snippet);
				}
			}
			else
			{
				if (OnShowText != null)
				{
					OnShowText.Invoke(snippet);
				}
			}
		}

		yield return new WaitForSeconds(snippet.Time);
	}

	public void Clear()
	{
		if (OnClearText != null)
		{
			OnClearText.Invoke();
		}
	}
}
