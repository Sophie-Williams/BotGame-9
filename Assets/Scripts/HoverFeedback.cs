using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HoverFeedback : MonoBehaviour {
	[SerializeField] RectTransform BoundsBox;
	[SerializeField] TMPro.TextMeshProUGUI Label;

	private GameObject currentTarget;

	public void OnHoverUpdate(GameObject obj) {
		currentTarget = obj;
	}

	void Start () {
		FindObjectOfType<HoverController>().OnHoverUpdate += this.OnHoverUpdate;	
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTarget) {
			Collider c = currentTarget.GetComponent<Collider>();
			UIBounds uibounds = UIBounds.fromBounds(c.bounds);

			BoundsBox.gameObject.SetActive(true);
			BoundsBox.anchoredPosition = uibounds.ScreenPointBounds.BottomLeft;
			BoundsBox.sizeDelta = uibounds.ScreenPointBounds.Size;
			BoundsBox.ForceUpdateRectTransforms();

			Label.gameObject.SetActive(true);
			Label.SetText(currentTarget.name);
		
			if (uibounds.ViewportBounds.center.y > 0.5f) {
				Label.alignment = TMPro.TextAlignmentOptions.Top;
				Label.rectTransform.anchoredPosition = uibounds.ScreenPointBounds.Bottom;
				Label.rectTransform.pivot = new Vector2(0.5f, 1f);
			} else {
				Label.alignment = TMPro.TextAlignmentOptions.Bottom;
				Label.rectTransform.anchoredPosition = uibounds.ScreenPointBounds.Top;
				Label.rectTransform.pivot = new Vector2(0.5f, 0f);
			}
			Label.rectTransform.ForceUpdateRectTransforms();
		} else {
			BoundsBox.gameObject.SetActive(false);
			Label.gameObject.SetActive(false);
		}
	}
}
