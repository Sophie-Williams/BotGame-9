using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameState State;

	private Playable currentPlayable;
	private GameObject currentTarget = null;

	void Start()
	{
		currentPlayable = State.CurrentPlayable;
		currentPlayable.Enter();
		FindObjectOfType<HoverController>().OnHoverUpdate += this.OnHoverUpdate;
	}

	void Update()
	{
		currentPlayable.PlayableUpdate();

		if (Input.GetMouseButton(0) && currentTarget != null)
		{
			Playable nextPlayable = currentTarget.GetComponent<Playable>();

			if (nextPlayable != null)
			{
				if (Input.GetMouseButton(0))
				{
					nextPlayable.Enter();
					currentPlayable.Leave();
					currentPlayable = nextPlayable;

					if (currentPlayable.Id != null && currentPlayable.Id != "")
					{
						State.CurrentPlayableId = currentPlayable.Id;
					}
				}
			}
		}
	}

	public void OnHoverUpdate(GameObject obj)
	{
		currentTarget = obj;
	}
}
