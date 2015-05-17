using UnityEngine;
using System.Collections;

public class EnemyActor : MonoBehaviour {
	public float health = 60.0f;

	void OnCollisionEnter(Collision other) {
		Debug.Log("enemy was hit");
		if (other.gameObject.tag == "Bullet") {
			// TODO: replace damage value with variable, probably within the projectile somewhere
			// Placing the damage value within the projectile would allow greater customization/projectile variations
			health -= 20.0f;
			
			// Simple kill:
			if (health <= 0) {
				Destroy(gameObject);
			}
		}
	}
}