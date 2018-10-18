using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] GameState State;

	private Playable currentPlayable;
	private GameObject currentTarget = null;

	void Start()
	{
		FindObjectOfType<HoverController>().OnHoverUpdate += this.OnHoverUpdate;
		Setup();
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
					MoveTo(nextPlayable);
				}
			}
		}
	}

	/// <summary>
	/// Set up the state for this PlayerController.
	/// </summary>
	public void Setup()
	{
		MoveTo(State.CurrentPlayable);
	}

	/// <summary>
	/// Move into the given playable.
	/// </summary>
	/// <param name="next"></param>
	public void MoveTo(Playable next)
	{
		next.Enter();

		if (currentPlayable != null)
		{
			currentPlayable.Leave();
		}

		currentPlayable = next;

		var id = currentPlayable.LookupId();

		if (id != "")
		{
			State.CurrentPlayableId = id;
		}
	}

	public void OnHoverUpdate(GameObject obj)
	{
		currentTarget = obj;
	}
}
