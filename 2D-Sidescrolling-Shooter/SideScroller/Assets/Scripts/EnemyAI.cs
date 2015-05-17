using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	private bool inBox;
	GameObject player;
	public GameObject bulletPrefab;
	public float launchForce = 1000.0f;
	public float interval = 2f;
	private float timer;
	private float distToPlayerInX;
	private float distToPlayerInY;
	Vector3 fwd;
	public AudioClip shootsound;
	// Use this for initialization
	void Start () 
	{
		fwd = this.transform.TransformDirection(Vector3.right);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (inBox) 
		{
			lookAtPlayer ();
			if(Time.time >= timer)
			{
				shootAtPlayer();
				timer = Time.time + interval;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
						player = other.gameObject;
						inBox = true;
						timer = Time.time + interval;
				} else if (other.tag == "Bullet") {
				}
		else {
						Physics.IgnoreCollision (other.collider, collider);
				}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			inBox = false;
		}
	}

	void lookAtPlayer()
	{
		// get the distance in the x-axis from the enemy to the player once the player is inside the box.
		distToPlayerInX = player.transform.position.x - transform.position.x;
		// if player more negative in global position than enemy
		if (distToPlayerInX == -Mathf.Abs (distToPlayerInX)) 
		{
			// if the enemy is not facing the negative global direction, rotate
			if(fwd.x != -Mathf.Abs(fwd.x))
			{
				var angleToTarget = 180f;
				transform.eulerAngles = transform.eulerAngles + transform.TransformDirection(Vector3.up) * angleToTarget;
				fwd = transform.TransformDirection(Vector3.right);
				// effect: if the player is behind the enemy, the enemy turns to face the player.
			}
		} 
		// if player is more positive in global position than enemy
		else if(distToPlayerInX == Mathf.Abs(distToPlayerInX))
		{
			// if the enemy is not facing the positive global direction, rotate
			if(fwd.x != Mathf.Abs(fwd.x))
			{
				var angleToTarget = 180f;
				transform.eulerAngles = transform.eulerAngles + transform.TransformDirection(Vector3.up) * angleToTarget;
				fwd = transform.TransformDirection(Vector3.right);
				// effect: if the player is behind the enemy, the enemy turns to face the player.
			}
		}
	}

	void shootAtPlayer()
	{
		// get the distance in the y-axis from the enemy to the player once the player is inside the box.
		distToPlayerInY = player.transform.position.y - transform.position.y;
		//if player is in same y value, shoot forward
		Debug.Log (distToPlayerInY);
		if (distToPlayerInY <= 1) 
		{
			GameObject newBullet = (GameObject)Instantiate (bulletPrefab, transform.position + transform.right * 1.5f, transform.localRotation);
			audio.PlayOneShot(shootsound);
			if (newBullet.rigidbody != null)
			{
				newBullet.rigidbody.AddForce (transform.right * launchForce);
			}
		}
		//if player is above y value and in positive x, shoot up
		if (distToPlayerInY == Mathf.Abs (distToPlayerInY) && distToPlayerInX == Mathf.Abs(distToPlayerInX)) 
		{
			if(distToPlayerInY >= 1)
			{
				GameObject newBullet = (GameObject) Instantiate (bulletPrefab, transform.position + transform.right * 1.5f, transform.localRotation);
				audio.PlayOneShot(shootsound);
				if(newBullet.rigidbody != null)
				{
					newBullet.rigidbody.AddForce (launchForce,launchForce,0);
				}
			}
		}
		//if player is above y value and in negative x, shoot up
		if (distToPlayerInY == Mathf.Abs (distToPlayerInY) && distToPlayerInX == -Mathf.Abs(distToPlayerInX)) 
		{
			if(distToPlayerInY >= 1)
			{
				GameObject newBullet = (GameObject) Instantiate (bulletPrefab, transform.position + transform.right * 1.5f, transform.localRotation);
				audio.PlayOneShot(shootsound);
				if(newBullet.rigidbody != null)
				{
					newBullet.rigidbody.AddForce (-launchForce,launchForce,0);
				}
			}
		}
	}
}
