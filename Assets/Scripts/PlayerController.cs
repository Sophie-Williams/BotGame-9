using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Playable CurrentPlayable;

	private GameObject currentTarget = null;

	void Start()
	{
		CurrentPlayable.Enter();
		FindObjectOfType<HoverController>().OnHoverUpdate += this.OnHoverUpdate;
	}

	void Update()
	{
		CurrentPlayable.PlayableUpdate();

		if (Input.GetMouseButton(0) && currentTarget != null)
		{
			Playable nextPlayable = currentTarget.GetComponent<Playable>();

			if (nextPlayable != null)
			{
				if (Input.GetMouseButton(0))
				{
					nextPlayable.Enter();
					CurrentPlayable.Leave();
					CurrentPlayable = nextPlayable;
				}
			}
		}
	}

	public void OnHoverUpdate(GameObject obj)
	{
		currentTarget = obj;
	}
}
