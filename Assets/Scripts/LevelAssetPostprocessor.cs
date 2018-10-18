using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelAssetPostprocessor : AssetPostprocessor
{
	void OnPostprocessModel(GameObject g)
	{
		Apply(g.transform);
	}

	void Apply(Transform t)
	{
		if (t.name.Equals("StaticGeometry"))
		{
			Debug.Log("Applying static geometry");
			ApplyStaticGeometry(t);
			return;
		}

		if (t.name.Equals("InteractionColliders"))
		{
			Debug.Log("Adding interaction colliders");
			ApplyInteractionColliders(t);
			return;
		}

		if (t.name.Equals("Colliders"))
		{
			Debug.Log("Adding colliders");
			ApplyColliders(t);
			return;
		}

		// Recurse
		foreach (Transform child in t)
		{
			Apply(child);
		}
	}

	/// <summary>
	/// Mark geometry as static to allow for baked lighting.
	/// </summary>
	/// <param name="t"></param>
	void ApplyStaticGeometry(Transform t)
	{
		foreach (Transform child in t)
		{
			child.gameObject.isStatic = true;
			ApplyStaticGeometry(child);
		}
	}

	/// <summary>
	/// Add interaction colliders which prevent interaction rays from reaching through.
	/// 
	/// This is used to create geometry that prevents interacting from going through things, like walls.
	/// </summary>
	/// <param name="t"></param>
	void ApplyInteractionColliders(Transform t)
	{
		foreach (Transform child in t)
		{
			// NB: block interaction rays
			child.gameObject.layer = LayerMask.NameToLayer("Interactable");
			child.gameObject.tag = "InteractionCollider";

			var collider = child.gameObject.AddComponent<MeshCollider>();
			child.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	/// <summary>
	/// Add interaction colliders which prevent interaction rays from reaching through.
	/// 
	/// This is used to create geometry that prevents interacting from going through things, like walls.
	/// </summary>
	/// <param name="t"></param>
	void ApplyColliders(Transform t)
	{
		foreach (Transform child in t)
		{
			// NB: block interaction rays
			child.gameObject.layer = LayerMask.NameToLayer("Default");

			var collider = child.gameObject.AddComponent<MeshCollider>();
			child.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
