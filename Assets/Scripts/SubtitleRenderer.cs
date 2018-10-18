using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SubtitleRenderer : MonoBehaviour
{
	[SerializeField] SubtitleSystem SubtitleSystem;
	[SerializeField] TMPro.TextMeshProUGUI Label;

	private string currentText;
	private string renderedText;

	public void OnShowText(string text)
	{
		Label.SetText(text);
		Label.gameObject.SetActive(true);
	}

	public void OnShowClosedCaption(string text)
	{
		Label.SetText("[" + text + "]");
		Label.gameObject.SetActive(true);
	}

	public void OnClearText()
	{
		Label.gameObject.SetActive(false);
		Label.SetText("");
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
}
