using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public GameObject bulletPrefab;
	public float launchForce = 1000.0f;
	public float backLaunchForce = -10000.0f;
	bool turned = false;
	public AudioClip sound;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			turned = !turned;
				}


		if (Input.GetKeyDown(KeyCode.F)) {
			GameObject newBullet = (GameObject)Instantiate (bulletPrefab, transform.position + transform.forward * 1.5f, transform.localRotation);
			audio.PlayOneShot(sound);
			if (newBullet.rigidbody != null)
				newBullet.rigidbody.AddForce (transform.forward * launchForce);
		}

		if (Input.GetKeyDown (KeyCode.W) && !turned) {
			GameObject newBullet = (GameObject) Instantiate (bulletPrefab, transform.position + transform.forward * 1.5f, transform.localRotation);
			audio.PlayOneShot(sound);
			if(newBullet.rigidbody != null)
				newBullet.rigidbody.AddForce (launchForce,launchForce,0);
		}


		if (Input.GetKeyDown (KeyCode.S) && !turned) {
			GameObject newBullet = (GameObject) Instantiate (bulletPrefab, transform.position + transform.forward * 1.5f, transform.localRotation);
			audio.PlayOneShot(sound);
			if(newBullet.rigidbody != null)
				newBullet.rigidbody.AddForce (launchForce,-launchForce,0);
		}

		if (Input.GetKeyDown (KeyCode.W) && turned) {
			GameObject newBullet = (GameObject) Instantiate (bulletPrefab, transform.position + transform.forward * 1.5f, transform.localRotation);
			audio.PlayOneShot(sound);
			if(newBullet.rigidbody != null)
				newBullet.rigidbody.AddForce (-launchForce,launchForce,0);
		}


		if (Input.GetKeyDown (KeyCode.S) && turned) {
			GameObject newBullet = (GameObject) Instantiate (bulletPrefab, transform.position + transform.forward * 1.5f, transform.localRotation);
			audio.PlayOneShot(sound);
			if(newBullet.rigidbody != null)
				newBullet.rigidbody.AddForce (-launchForce,-launchForce,0);
		}

		
	}
}

