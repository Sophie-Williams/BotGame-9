using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
	public GameState State;
	public string Id;
	public string Description;

	/// <summary>
	/// Events to fire when this quest is done.
	/// </summary>
	public List<Event> OnDone = new List<Event>();

	/// <summary>
	/// Events that should have fired for this quest to be done.
	/// </summary>
	public List<QuestGoal> Goals = new List<QuestGoal>();

	public void Enable()
	{
		foreach (QuestGoal goal in Goals)
		{
			// NB: nothing to set up.
			if (goal.Event == null) {
				continue;
			}

			goal.Listener = new QuestGoalListener
			{
				Quest = this,
				Goal = goal,
			};
			goal.Event.Subscribe(goal.Listener);
		}
	}

	public void Disable()
	{
		foreach (QuestGoal goal in Goals)
		{
			goal.Event.Unsubscribe(goal.Listener);
			goal.Listener = null;
		}
	}

	/// <summary>
	/// Mark a quest goal as completed.
	/// </summary>
	/// <param name="goal"></param>
	public IEnumerator Mark(QuestGoal goal)
	{
		var goalId = Id + "/" + goal.Id;

		if (State.Completed.Contains(Id))
		{
			yield break;
		}

		if (State.Completed.Contains(goalId))
		{
			yield break;
		}

		State.Completed.Add(goalId);

		// NB: check that all goals are completed.
		if (!Goals.TrueForAll(g => State.Completed.Contains(Id + "/" + g.Id)))
		{
			yield break;
		}

		State.Completed.Add(Id);

		foreach (Event e in OnDone)
		{
			yield return e.Fire();
		}
	}

	public class QuestGoalListener : Event.Listener
	{
		public Quest Quest;
		public QuestGoal Goal;

		public IEnumerator Fire()
		{
			yield return Quest.Mark(Goal);
		}
	}

	[System.Serializable]
	public class QuestGoal
	{
		public string Id;

		/// <summary>
		/// Description of quest goal.
		/// </summary>
		public string Description;

		/// <summary>
		/// Event to subscribe to for when to consider this quest goal as completed.
		/// </summary>
		public Event Event;

		/// <summary>
		/// Events to fire when this goal is completed.
		/// </summary>
		public List<Event> OnDone = new List<Event>();

		[System.NonSerialized]
		public QuestGoalListener Listener = null;
	}
}