using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SubtitleRenderer : MonoBehaviour
{
	[SerializeField] SubtitleSystem SubtitleSystem;
	[SerializeField] TMPro.TextMeshProUGUI Label;

	private string currentText;
	private string renderedText;
	private DialogueSource currentSource = null;
	private Color currentColor = Color.white;

	public void OnShowText(Dialogue.Snippet snippet)
	{
		var t = "\"" + snippet.Text + "\"";

		if (SetSource(snippet.Source) && snippet.DisplaySource)
			t = snippet.Source.Name + ": " + t;

		Label.SetText(t);
		Label.gameObject.SetActive(true);
	}

	public void OnShowClosedCaption(Dialogue.Snippet snippet)
	{
		SetSource(snippet.Source);
		Label.color = Color.white;
		Label.SetText("[" + snippet.Text + "]");
		Label.gameObject.SetActive(true);
	}

	public void OnClearText()
	{
		Label.gameObject.SetActive(false);
		Label.SetText("");
		currentSource = null;
	}

	void Start()
	{
		Label.gameObject.SetActive(false);

		if (SubtitleSystem != null)
		{
			SubtitleSystem.OnShowText += this.OnShowText;
			SubtitleSystem.OnShowClosedCaption += this.OnShowClosedCaption;
			SubtitleSystem.OnClearText += this.OnClearText;
		}
	}

	// Update is called once per frame
	void Update()
	{
		Label.alignment = TMPro.TextAlignmentOptions.Top;
	}

	/// <summary>
	/// Sets the color of the text according to the source.
	/// </summary>
	private bool SetSource(DialogueSource source)
	{
		var changed = false;

		if (currentSource != source)
		{
			currentSource = source;
			changed = true;
		}

		if (currentSource != null)
		{
			if (Label.color != currentSource.SubtitleColor)
			{
				Label.color = currentSource.SubtitleColor;
			}
		}
		else
		{
			if (Label.color != Color.white)
			{
				Label.color = Color.white;
			}
		}

		return changed;
	}
}
