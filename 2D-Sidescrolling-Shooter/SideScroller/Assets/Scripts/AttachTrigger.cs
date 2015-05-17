using UnityEngine;
using System.Collections;

// When an actor touches this trigger, it becomes a child
// When an actor leaves this trigger, it is removed as a child
public class AttachTrigger : MonoBehaviour {
	// Attach the actor as a child of ourselves
	void OnTriggerEnter(Collider other) {
		// How parenting works in Unity:
		// - ONLY a transform can have a parent or child
		// - If one transform is a child of another:
		//	- During update, the parent transform (matrix) is added to the child
		//	- This makes the child transform "follow" the parent transform
		// - NOTE: the parent transform is completely unaffected by the child transform
		// - Why?
		//	- logically it should work this way
		//	- The parent doesn't even know that it has a child
		//
		// How to procedurally set parent/child relationships
		// - The Transform object has a parent property
		// - transform.parent is another transform
		// - By default, .parent is always null
		// - As soon as we assign a valid Transform to be a parent, 
		//   the matrix relationship is formed
		
		// Where's my object?
		// A Collider has a GameObject (gameObject)
		// The gameObject has the Transform
		other.gameObject.transform.parent = transform;
		Debug.Log ("shit");
	}
	
	// Detach the actor from ourselves
	void OnTriggerExit(Collider other) {
		// Removing the parent/child relationship is easy:
		// - set the transform's parent (of the child object) to null
		other.gameObject.transform.parent = null;
	}
}
