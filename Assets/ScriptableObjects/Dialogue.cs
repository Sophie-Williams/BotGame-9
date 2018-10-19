using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
	public AudioClip Audio;
	public SubtitleSystem SubtitleSystem;
	public List<Snippet> Snippets;

	[System.Serializable]
	public class Snippet
	{
		/// <summary>
		/// What is the source of this sound.
		/// 
		/// Can be used if the source is not visible.
		/// </summary>
		public DialogueSource Source;

		/// <summary>
		/// Text that will stay on screen.
		/// </summary>
		public string Text;

		/// <summary>
		/// Time to delay until snippet should be played.
		/// </summary>
		public float Delay;

		/// <summary>
		/// Time that this snippet will stay on screen.
		/// </summary>
		public float Time;

		/// <summary>
		/// Closed subtitles are audio hints, and are encapsulated in brackets.
		/// 
		/// They indicate things that you normally can't hear.
		/// </summary>
		public bool ClosedCaption = false;

		/// <summary>
		/// Render the snippet with the given source if present.
		/// </summary>
		public string Render(DialogueSource source)
		{
			var t = Text;

			if (ClosedCaption)
			{
				t = "[" + t + "]";
			}
			else
			{
				t = "\"" + t + "\"";
			}

			if (source != null && source.Name != "")
			{
				t = source.Name + ": " + t;
			}

			return t;
		}
	}

	/// <summary>
	/// Runs the given dialogue and blocks until it is done.
	/// </summary>
	/// <returns>enumerator that blocks until the given dialogue has run</returns>
	public IEnumerator RunDialogue(AudioSource Source)
	{
		Source.clip = Audio;
		Source.Play();

		foreach (var snippet in Snippets)
		{
			yield return SubtitleSystem.RunDialogueSnippet(snippet);
		}

		SubtitleSystem.Clear();
		yield return new WaitUntil(() => !Source.isPlaying);
	}
}
