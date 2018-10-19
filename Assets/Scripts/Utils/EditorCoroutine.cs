using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Utility class for running coroutines from an Editor window.
///
/// Binds up to the EditorApplication's update loop to poll every coroutine for completion at that point.
/// </summary>
public class EditorCoroutine
{
	private LinkedList<Routine> routines = new LinkedList<Routine>();
	private long previousTicks;

	/// <summary>
	/// Setup a new helper and register it to the editor update loop.
	/// </summary>
	public static EditorCoroutine Setup()
	{
		var editorCoroutine = new EditorCoroutine();
		EditorApplication.update += editorCoroutine.update;
		return editorCoroutine;
	}

	/// <summary>
	/// Close the helper, unregistering the update loop.
	/// </summary>
	public void Destroy()
	{
		EditorApplication.update -= this.update;
	}

	/// <summary>
	/// Try to advance all routines.
	/// </summary>
	private void update()
	{
		var node = routines.First;

		var currentTicks = System.DateTime.Now.Ticks;
		var span = new System.TimeSpan((currentTicks - previousTicks));
		var delta = (float)span.TotalMilliseconds / 1000f;

		while (node != null)
		{
			var routine = node.Value;

			if (routine.Test(delta) && !routine.Move())
				routines.Remove(node);

			node = node.Next;
		}

		previousTicks = currentTicks;
	}

	/// <summary>
	/// Flatten the given enumerator, causing all nested IEnumerators which are generated to be flattened into one long enumerator.
	/// </summary>
	private IEnumerator flatten(IEnumerator enumerator)
	{
		while (enumerator.MoveNext())
		{
			var value = enumerator.Current;

			if (value is IEnumerator)
			{
				var nested = flatten(value as IEnumerator);

				while (nested.MoveNext())
					yield return nested.Current;

				continue;
			}

			yield return value;
		}
	}

	public void RunCoroutine(IEnumerator enumerator)
	{
		this.routines.AddLast(new Routine
		{
			Current = null,
			Enumerator = flatten(enumerator),
		});
	}

	/// <summary>
	/// The state of a single coroutine.
	/// </summary>
	class Routine
	{
		public Testable Current;
		public IEnumerator Enumerator;

		public bool Test(float delta)
		{
			if (Current == null)
			{
				return true;
			}

			return Current.Test(delta);
		}

		/// <summary>
		/// Move to the next value in enumerator.
		/// </summary>
		public bool Move()
		{
			if (!Enumerator.MoveNext())
			{
				return false;
			}

			var value = Enumerator.Current;

			if (value == null)
			{
				return true;
			}

			if (value is WaitForSeconds)
			{
				var wait = value as WaitForSeconds;

				var field = wait.GetType().GetField("m_Seconds", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

				if (field == null)
				{
					throw new System.Exception("WaitForSeconds: Could not find m_Seconds field");
				}

				var time = (float)field.GetValue(wait);
				Current = new WaitForSecondsTest { Left = time };
				return true;
			}

			if (value is WaitUntil)
			{
				var wait = value as WaitUntil;
				Current = new WaitUntilTest { Wait = wait };
				return true;
			}

			throw new System.Exception("Unhandled: " + value.GetType());
		}
	}

	interface Testable
	{
		/// <summary>
		/// Is this enumerator done?
		/// </summary>
		bool Test(float delta);
	}

	class WaitForSecondsTest : Testable
	{
		public float Left;

		public bool Test(float delta)
		{
			Left -= delta;
			return Left <= 0f;
		}
	}

	class WaitUntilTest : Testable
	{
		public WaitUntil Wait;

		public bool Test(float delta)
		{
			return !Wait.keepWaiting;
		}
	}
}
