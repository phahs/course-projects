using UnityEngine;
using System.Collections;

public class AppleAttack : MonoBehaviour {
	// What happens when the apple sees the player character?
	public GameObject appleEnemy;			// This is me
	public float maxRange = 20f;			// This is as far away as I want to be from the player
	public float minRange = 15f;			// This is as close as I want to be to the player
	public float distance;					// how far away the player is
	public Vector3 moveTo;					// will keep me in my range band
	public float rof = 2f;					// my rate of fire
	public GameObject projectilePrefab;		// what I will be throwing at the player
	public float throwForce = 1000;			// how hard I thow
	public bool hasFired;					// have I thrown?
	public float timer;						// need to keep trak of time
	public Vector3 lkp;						// last known position of the player
	
	private SphereCollider col;				// To reset whether the enemy can see the player based on the Basic Enemy script
	private BasicEnemyCharacter bec;		// Need to know what is going on in the Basic Enemy Character script
	private GameObject player;				// Need to know about the player
	private NavMeshAgent nav;				// To include the NavMeshAgent component
	private NavMeshPath stayInRange;		// To set up a NavMesh path
	private NavMeshPath keepInSight;		// To set up a second NavMesh path
	
	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player");	// find the player character
		nav = GetComponent<NavMeshAgent> ();					// connect to the NavMeshAgent
		stayInRange = new NavMeshPath ();						// for keeping the player in range
		keepInSight = new NavMeshPath ();						// for keeping the player in sight
		hasFired = false;										// so we can modify the detection area of the enemy
		col = GetComponent<SphereCollider> ();
	}
	
	void Update(){
		// need the latest info from the BasicEnemyCharacter script
		bec = appleEnemy.GetComponent ("BasicEnemyCharacter") as BasicEnemyCharacter;
		// if the player is in sight
		if (bec.isPlayerInSight) {
			// increase my speed so I move fast
			nav.speed = 6f;
			// I need to keep the player in sight so I need a path to the player
			nav.CalculatePath (player.transform.position, keepInSight);
			// I need to know the distance and direction between the player and I
			moveTo = player.transform.position - gameObject.transform.position;
			// prepare for a raycast
			RaycastHit hit;
			// cast a ray from me, in the direction of the player for 20 units
			if (Physics.Raycast (gameObject.transform.position, moveTo.normalized, out hit, 30f)) {
				// if the ray hits the player
				if (hit.collider.gameObject == player) {
					// make note of where the player is, this is considered the players last known position
					lkp = hit.collider.gameObject.transform.position;
					// how far away from the player am I?
					distance = moveTo.magnitude;
					// if I am farther away from the player than my max range
					if (distance > maxRange) {
						Debug.Log("too far away");
						// get the position I should move to in order to be at my max range
						moveTo = Vector3.MoveTowards (gameObject.transform.position, player.transform.position, distance - maxRange);
						// calculate a path for that position
						nav.CalculatePath (moveTo, stayInRange);
						// and move to that position
						nav.SetPath (stayInRange);
					}
					// if I am closer to the player than my minimum range
					if (distance < minRange) {
						Debug.Log("too close");
						// get the position I should move to in order to be at my minimum range
						moveTo = Vector3.MoveTowards (gameObject.transform.position, player.transform.position, distance - minRange);
						// calculate a path for that position
						nav.CalculatePath (moveTo, stayInRange);
						// and move to that position
						nav.SetPath (stayInRange);
					}
					// if I am at or between both my max and minimum ranges
					if (distance <= maxRange && distance >= minRange) {
						// Look!!! I'm going to throw this
						GameObject appleProjectile;
						// if I have not yet thrown a projectile
						if (!hasFired) {
							// set what I am going to throw
							appleProjectile = (GameObject)Instantiate (projectilePrefab, gameObject.transform.position
							                                           + moveTo.normalized * 2.0f, transform.localRotation);
							// if what I am throwing does have a rigidbody
							if (appleProjectile.rigidbody != null) {
								// add force to what I am throwing
								appleProjectile.rigidbody.AddForce (moveTo.normalized * throwForce);
							}
							// I have now thrown my projectile
							hasFired = !hasFired;
							// time to wait until I can throw it again
							timer = Time.time + rof;
						}
					}
				}
				// if the ray did not hit the player
				else{
					// I should make a path to the last known position of the player
					nav.CalculatePath(lkp, keepInSight);
					// Move there now!!!!
					nav.SetPath(keepInSight);
					// if I reach the last known position of the player
					if(gameObject.transform.position == nav.pathEndPosition + gameObject.transform.up)
					{
						col.radius = 0.5f;		// reduce the size of my sphere collider so that my knowledge of the player will reset
						nav.speed = 3f;			// return my speed to normal so that I can patrol at my normal slow speed
					}
				}
			}
			// if I have thrown my projectile and I have waited long enough
			if (hasFired && Time.time >= timer) {
				// I will be able to fire again!
				hasFired = !hasFired;
			}
		}
	}

}
