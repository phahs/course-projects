using UnityEngine;
using System.Collections;

public class oscillator : MonoBehaviour {
	public float speed = 1.0f; 					// Sine period advancement per second
	public Vector3 amount = new Vector3 (0.0f, 1.0f, 0.0f);	// Per-axis movement distance scalar
	Vector3 originalPosition = Vector3.zero;	// Where we started
	float lastCycleTime = 0.0f;					// Cummulative time count
	float lastSine = 0.0f;						// Sine value of cummulative time count
	
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		// There's no point leaving this component enabled if it's not going to do anything
		if (amount == Vector3.zero)
			enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Refresh our control values
		lastCycleTime += speed * Time.deltaTime;
		lastSine = Mathf.Sin(lastCycleTime);
		
		// Adjust the game object's position
		// Because we used originalPosition, the object will always translate back and forth across the initial position it started at
		transform.position = originalPosition + (amount*lastSine);
	}
}
