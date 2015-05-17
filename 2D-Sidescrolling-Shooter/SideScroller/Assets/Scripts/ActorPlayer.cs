using UnityEngine;
using System.Collections;

public class ActorPlayer : MonoBehaviour {
	public float health = 100.0f;
	public bool alive;
	public AudioClip pitfall;
	void Start(){
		alive = true;
		Time.timeScale = 1;
	}

	void Update() {

		// Simple kill:
		if (health <= 0) {
			alive = false;
		}


		if (alive == false) {
			gameObject.SetActive(false);
			Time.timeScale = 0;
			Application.LoadLevel("Game Over");
		}

	}

	void OnCollisionEnter(Collision other) {
		Debug.Log("actor was hit");
		if (other.gameObject.tag == "Bullet") {
			health -= 20.0f;
		}

		if (other.gameObject.tag == "DeathZone") {
			health -= 100.0f;
			audio.PlayOneShot(pitfall);
			Debug.Log ("fell into a pit");
		}

		// Simple kill:
		if (health <= 0) {
			alive = false;
		}

	}

	void OnTriggerEnter(Collider other) {
				if (other.gameObject.tag == "DeathZone") {
						health -= 100.0f;
					Debug.Log ("fell into death pit");
					// Simple kill:
					if (health <= 0) {
						alive = false;
					}
				}
		}
}
