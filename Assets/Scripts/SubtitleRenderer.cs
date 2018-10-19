using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SubtitleRenderer : MonoBehaviour
{
	[SerializeField] SubtitleSystem SubtitleSystem;
	[SerializeField] TMPro.TextMeshProUGUI Label;

	public void OnShowText(DialogueSource source, Dialogue.Snippet snippet)
	{
		if (source != null)
		{
			Label.color = source.SubtitleColor;
		}

		Label.SetText(snippet.Render(source));
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
			SubtitleSystem.OnClearText += this.OnClearText;
		}
	}

	// Update is called once per frame
	void Update()
	{
		Label.alignment = TMPro.TextAlignmentOptions.Top;
	}
}
