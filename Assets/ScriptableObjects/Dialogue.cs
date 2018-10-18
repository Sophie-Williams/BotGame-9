using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
	public AudioClip Audio;
	public SubtitleSystem SubtitleSystem;
	public List<DialogueSnippet> Snippets;

	[System.Serializable]
	public class DialogueSnippet
	{
		/// <summary>
		/// Time to delay until snippet should be played.
		/// </summary>
		public float Delay;

		/// <summary>
		/// Time that this snippet will stay on screen.
		/// </summary>
		public float Time;

		/// <summary>
		/// Text that will stay on screen.
		/// </summary>
		public string Text;

		/// <summary>
		/// Closed subtitles are audio hints, and are encapsulated in brackets.
		/// 
		/// They indicate things that you normally can't hear.
		/// </summary>
		public bool ClosedCaption = false;
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
