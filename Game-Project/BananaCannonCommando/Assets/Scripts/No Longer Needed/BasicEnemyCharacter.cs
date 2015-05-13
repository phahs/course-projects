using UnityEngine;
using System.Collections;

public class BasicEnemyCharacter : MonoBehaviour 
{

	public bool isPlayerInSight;	// can the player be seen?
	public bool pause = false;		// pause on the patrol route
	public float pauseTime = 5f;	// how long to pause on the patrol route
	public Vector3[] patrolPoints;	// the patrol route I keep mentioning
	public float fov = 110f;		// the enemies' field of view
	public int health = 100;		// how much health enemies have
	public bool wasHit = false;		// was I hit

	private NavMeshAgent nav;		// To include the NavMeshAgent component
	private NavMeshPath patrol;		// To set up a NavMesh path
	private bool hasFoundPath;		// To determine if a path has been found
	private GameObject player;		// To not need to do more than one string compare for the Player tag
	private float timer;			// To keep track of how long we have been paused
	private int currentPoint;		// To iterate through the patrol locations
	private bool hasSavedPath;		// To determine if we have saved our last path
	private Vector3 savePath;		// To store the last path before seeing the player
	private SphereCollider col;		// To reset whether the enemy can see thr player based on the Enemy Specific scripts
//	private DropItem drop;			// To drop items when killed

	void Start()
	{
		// initiate some values at the start of the scene
		nav = GetComponent<NavMeshAgent> ();					// connect to the NavMeshAgent
		player = GameObject.FindGameObjectWithTag ("Player");	// find the player character
		isPlayerInSight = false;								// start state for this is that the player is not in sight
		patrol = new NavMeshPath ();							// for all future paths
		currentPoint = 0;										// iterator for the patrol route array
		hasFoundPath = false;									// so we know when a viable path has been found
		hasSavedPath = false;									// so we know if we have saved a previous path
		col = GetComponent<SphereCollider> ();					// so we can modify the detection area of the enemy
//		drop = gameObject.GetComponent<DropItem> ();
	}

	void Update()
	{
		// what we do every frame
			// is check if we can see the player
			if (!isPlayerInSight) {
				// if we are returning from a stop (in otherwords from seeing the player)
				if (hasSavedPath) {
					// then we need to find a path again
					hasFoundPath = false;
					// we also don't want to return to this section right away
					hasSavedPath = false;
					// time that we will follow to the players last known position... i think
					timer = Time.time + pauseTime;
					// I should wait for a little while before returning to my patrol
					pause = !pause;
				}
				// if we are not pausing at a patrol point on the patrol route
				if (!pause) {
					// if we have not already found a path
					if (!hasFoundPath) {
						// we find a path and we set that path
						hasFoundPath = nav.CalculatePath (patrolPoints [currentPoint], patrol);
						// I'm going on patrol!!
						nav.SetPath (patrol);
					}
					// if we are at one of the patrol points
					if (gameObject.transform.position == patrolPoints [currentPoint]) {
						currentPoint++;						// look at the next patrol point
						nav.ResetPath ();					// clear our path
						hasFoundPath = false;				// prepare for finding a new path
						timer = Time.time + pauseTime;		// prepare to pause for a little while
						pause = !pause;						// and make sure we pause
					}
				}
				// if we are pausing and we have paused for the requisite time
				if (pause && Time.time >= timer) {
					// we should stop pausing
					pause = !pause;
					// if we don't have a path yet, we should be moving at a speed of 3
					if (!hasFoundPath) {
						// we patrol slowly
						nav.speed = 3f;
					}
				}
				// if we do see the player
			} else {
				// quit following the path, we need to stare at the player
			//	nav.Stop ();
				// we need to "remember where we were headed before staring at the player
				hasSavedPath = true;
			}
			// if our iterator has exceeded the length of our patrol route
			if (currentPoint >= patrolPoints.Length) {
				// reset our iterator
				currentPoint = 0;
			}
		if (health <= 0) {
		//	drop.randomItem();
			Destroy (gameObject);
		}
	}

	void OnTriggerStay(Collider other)
	{
		// if the player has entered our sphere collider trigger
		if (other.gameObject == player) {
			// get the vector from me to the player
			Vector3 toPlayer = other.transform.position - gameObject.transform.position;
			// determine where the player is in relation to the direction I am facing
				float angleToPlayer = Vector3.Angle (nav.steeringTarget.normalized, toPlayer.normalized);
	//		float angleToPlayer2 = Vector3.Angle (toPlayer.normalized, -nav.steeringTarget.normalized);
	//		Debug.Log("what is the angle to the player: " + angleToPlayer + ", " + angleToPlayer2);
			// if the player is in 1/2 of my field of view
			if (angleToPlayer <= fov * 0.5f) {
				// prepare for a raycast
				RaycastHit hit;
				// cast a ray from me, in the direction of the player for 20 units
					if (Physics.Raycast (gameObject.transform.position, toPlayer.normalized, out hit, 30f)) {
					// if the ray hits the player
					if (hit.collider.gameObject == player) {
						// then I can see the player
						isPlayerInSight = true;
						}
					}
				}
			}
		}
	
	

	void OnTriggerExit(Collider other)
	{
		// if the player can leave the sphere collider trigger
		if (other.gameObject == player) {
			// I can't see the player
			isPlayerInSight = false;
			// I have knowledge of my surroundings out to 3... whatevers
			col.radius = 3f;
			
		}
	}
}