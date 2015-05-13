using UnityEngine;
using System.Collections;

public class GrapeAI : MonoBehaviour {
	
	public float normalSpeed = 1.5f;				// normal walking speed to get from point a to point b
	public float engageSpeed = 20f;				// how fast I move when I have an enemy in my sights
	public float patrolWait = 3f;				// time I wait at patrol points
	public float chaseFor = 2f;				// how long I chase after runaway enemies (the player) for
	public float fov = 110f;					// how much of the world I can see
	public Vector3 facing;						// the direction I am facing
	public Transform[] patrol;					// list of places I patrol
	public bool chasing;						// should I be chasing the player?
	public float maxRange = 2.75f;					// how far I can be and still be able to attack the player
	public Sprite[] spriteList;

	// For the animations
	public bool inSight;						// can I see the player?
	public bool inRange;						// is the player within my range?
	public bool atPoint;
	public bool moving;
	public float chaseTimer;
	
	private Vector3 lkf;						// the last known direction that I was facing
	private Vector3 lkp;						// the last known position of the player
	private float distance;						// the distance between myself and the player
	private NavMeshAgent nav;					// the navigation mesh that allows me to roam around
	private NavMeshPath path;					// the path to get from point a to point b
	private Facing changeFacing;
	private Animator anim;
	private SpriteRenderer currentSprite;
	private float patrolTimer;					// makes sure I keep my patrol in check
	private int patrolIndex;					// lets me know what patrol point I am headed to
	private GameObject player;					// the evil and vile player
	private bool hasFired;						// lets me know that I have attacked the player recently
	private EnemyHealth myHealth;
	private DropItem randomItem;
	private PlayerController playerHealth;
	private int spriteVal = 0;
	private Transform body;
	
	// when the game begins these things are initialised regardless of whether this script is enabled
	void Awake()
	{
		nav = GetComponent<NavMeshAgent> ();					// gets the navmeshagent
		path = new NavMeshPath ();								// creates an empty path for me to use
		myHealth = GetComponent<EnemyHealth>();
		randomItem = GetComponent<DropItem> ();
		changeFacing = GetComponent<Facing> ();
		anim = GetComponent<Animator> ();
		body = transform.GetChild(0);
		currentSprite = body.GetComponent<SpriteRenderer> ();
		currentSprite.sprite = spriteList [spriteVal];
		player = GameObject.FindGameObjectWithTag ("Player");	// finds the player
		facing = transform.position + Vector3.forward;			// my starting orientation
		inSight = false;											// I start unable to see the player
		inRange = false;										// I start with the player out of range
		atPoint = false;
		moving = false;
		chasing = false;										// I start by not chasing the player
		patrolIndex = 0;										// I am to go to the first patrol point
		lkp = Vector3.zero;										// the player's default last known position
		hasFired = false;										// I start without attacking anyone
		playerHealth = player.GetComponent ("PlayerController") as PlayerController;
	}
	
	// is called every frame
	void Update()
	{
		// if I can see the player and the player is in my range
		if (inSight && inRange) {
			Attack ();											// I attack the player
			// otherwise, if I am chasing the player
		} else if (chasing) {
			chaseTimer += Time.deltaTime;						// I need to keep track of how long I am chasing the player
			// if I have not chased the player for long enough
			if (chaseTimer <= chaseFor) {
				nav.CalculatePath (lkp, path);					// I make sure I have an up to date path to the players last known position
				nav.SetPath (path);								// I then follow that path
				facing = (nav.steeringTarget - transform.position + Vector3.up * 0.5f).normalized;	// I make sure that I am facing in the direction I am running
				// if I am not facing zero (that dark abyss)
				if (facing != Vector3.zero) {
					lkf = facing;								// I keep a record of the way I am facing, in case I do face zero, has been known to happen
				}
				// if, during my chase, I can see the player
				if (inSight) {
					chasing = false;							// I no longer need to chase the player
					chaseTimer = 0f;							// I can reset my chase timer in case the player runs away again
				}
				// if I have chased the player for long enough
			} else {
				lkp = Vector3.zero;								// I can set his last known position back to the default
				chasing = false;								// I can stop chasing him, for I have gone as far as is needed
				chaseTimer = 0f;								// I can reset my chase timer in case I come across the player again
			}
			// if I can see the player, but he is outside of my range
		} else if (inSight && !inRange) {
			chasing = false;
			Engage ();											// I need to engage the player
			// in all other cases
		} else {
			Patrol ();											// I patrol along my route
		}

		int tempVal;
		
		if (facing != Vector3.zero) {
			tempVal = changeFacing.DetermineSprite (facing);
		} else {
			tempVal = changeFacing.DetermineSprite(lkf);
		}

		if (spriteVal != tempVal) {
			spriteVal = tempVal;
			currentSprite.sprite = spriteList [spriteVal];
		}
		if (myHealth.currentHealth <= 0) {
			randomItem.ItemDrop ();
			Destroy (gameObject);
		}

		anim.SetBool ("InSight", inSight);
		anim.SetBool ("InRange", inRange);
		anim.SetBool ("AtPoint", atPoint);
		anim.SetBool ("Moving", moving);
		anim.SetFloat ("ChaseTimer", chaseTimer);
	}
	
	// this is how I attack the player
	void Attack()
	{
		if (!hasFired) {
			playerHealth.currentPlayerHealth -= 50;
			myHealth.currentHealth -= 200;
		}
	}
	// this is how I engage the player
	void Engage()
	{
		nav.speed = engageSpeed;							// prepare myself to move really fast
		
		distance = (player.transform.position - transform.position).magnitude;	// how far away is the player from me
		// if the player is too far away
		if (distance > maxRange) {
			Vector3 moveTo = Vector3.MoveTowards (transform.position, player.transform.position, distance - maxRange + 1f);	// I should move towards him
			if(nav.CalculatePath (moveTo, path))					// I need to find a good path to get the player close enough
				nav.SetPath (path);									// I can start moving to that position
			// if the player is too close
		}

		else if (distance <= maxRange) {
			inRange = true;										// now I can attack the player
		}
	}
	
	// What I am doing the rest of the time
	void Patrol()
	{
		moving = true;
		bool hasPath;
		if (transform.position.x == patrol [patrolIndex].position.x &&
			transform.position.z == patrol [patrolIndex].position.z) {
			moving = false;
			atPoint = true;
			patrolTimer += Time.deltaTime;						// I should wait here and look in front of me
			// if I have waited long enough
			if (patrolTimer >= patrolWait) {
				// if I am at the last patrol point
				if (patrolIndex == patrol.Length - 1) {
					patrolIndex = 0;							// I should go back to the first patrol point
					patrolTimer = 0;							// I should restart my patrol timer
					//otherwise
				} else {
					patrolIndex++;								// I go to the thext patrol point
					patrolTimer = 0;							// I should restart my patrol timer
				}
				atPoint = false;
			}
		}
		
		hasPath = nav.CalculatePath (patrol [patrolIndex].position, path);	// calculate a good path to the patrol point
		if (hasPath) {
			hasPath = nav.SetPath (path);
			facing = (nav.steeringTarget - gameObject.transform.position).normalized;	// make sure I am facing the way I am walking
			facing.y = 0f;
		}
		// as long as I'm not looking at zero
		if (facing != Vector3.zero) {
			lkf = facing;										// I keep a backup of where I'm looking
		}
	}
	
	// when something enters and stays in my personal bubble
	void OnTriggerStay(Collider other)
	{
		
		if (other.gameObject == player) {
			Vector3 direction = other.transform.position - transform.position;
			if (facing != Vector3.zero) {
				float angle = Vector3.Angle (facing, direction);
				if (angle < (fov * 0.5f)) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position + Vector3.up * 0.5f, direction.normalized, out hit, 60f)) {
						if (hit.collider.gameObject == player) {
							inSight = true;
							facing = direction.normalized;
							lkp = player.transform.position;
						}else {
							inSight = false;
							inRange = false;
							if(lkp != Vector3.zero)
							{
								chasing = true;
							}
						}
					}
				} 
			} else {
				float angle = Vector3.Angle (lkf, direction);
				if (angle < (fov * 0.5f)) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position + Vector3.up * 0.5f, direction.normalized, out hit, 60f)) {
						if (hit.collider.gameObject == player) {
							inSight = true;
							facing = direction.normalized;
							lkp = player.transform.position;
						}else {
							inSight = false;
							inRange = false;
							if(lkp != Vector3.zero)
							{
								chasing = true;
							}
						}
					}
				} 
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player) {
			inSight = false;
			inRange = false;
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Base Cannon" || collision.gameObject.tag == "Egg Splode"
		    || collision.gameObject.tag == "Pellet" || collision.gameObject.tag == "Shotgun") {
			nav.Stop ();
			Vector3 direction = player.transform.position - transform.position;
			facing = direction.normalized;
			inSight = true;
		}
	}
}
