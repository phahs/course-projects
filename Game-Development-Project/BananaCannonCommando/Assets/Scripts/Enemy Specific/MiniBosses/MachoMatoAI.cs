using UnityEngine;
using System.Collections;

public class MachoMatoAI : MonoBehaviour {

	public float normalSpeed = 5f;				// normal walking speed to get from point a to point b
	public float engageSpeed = 9f;				// how fast I move when I have an enemy in my sights
	public float patrolWait = 3f;				// time I wait at patrol points
	public float chaseFor = 9f;					// how long I chase after runaway enemies (the player) for
	public float reloadTime = 6f;				// how long it takes me to attack the player again
	public float fov = 110f;					// how much of the world I can see
	public Vector3 facing;						// the direction I am facing
	public Transform[] patrol;					// list of places I patrol
	public bool chasing;						// should I be chasing the player?
	public float chargeRange = 10f;				// how far I can be and still be able to attack the player
	public float damageRange = 4f;
	public Sprite[] spriteList;
	public bool hasAttackPath;
	
	// Variables for the Animations
	public bool inSight;
	public bool inRange;						// is the player within my range?
	public bool atPoint;
	public bool moving;
	public float chaseTimer;
	
	private Vector3 lkf;						// the last known direction that I was facing
//	private Vector3 lkp;						// the last known position of the player
	private float distance;						// the distance between myself and the player
	private NavMeshAgent nav;					// the navigation mesh that allows me to roam around
	private NavMeshPath path;					// the path to get from point a to point b
	private float patrolTimer;					// makes sure I keep my patrol in check
	private float reloading;					// lets me know that I am done reloading
	private int patrolIndex;					// lets me know what patrol point I am headed to
	private GameObject player;					// the evil and vile player
	private bool hasAttacked;						// lets me know that I have attacked the player recently
	private EnemyHealth myHealth;
	private DropItem randomItem;
	private PlayerController playerHealth;
	private NavMeshAgent playerNav;
	private Animator anim;
	private Facing changeFacing;
	private SpriteRenderer currentSprite;
	private int spriteVal = 0;
	private Transform body;
	
	// when the game begins these things are initialised regardless of whether this script is enabled
	void Awake()
	{
		body = transform.GetChild(0);
		nav = GetComponent<NavMeshAgent> ();					// gets the navmeshagent
		path = new NavMeshPath ();								// creates an empty path for me to use
		currentSprite = body.GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		changeFacing = GetComponent<Facing> ();
		myHealth = GetComponent<EnemyHealth>();
		randomItem = GetComponent<DropItem> ();
		player = GameObject.FindGameObjectWithTag ("Player");	// finds the player
		facing = transform.position + Vector3.forward;			// my starting orientation
		inSight = false;											// I start unable to see the player
		inRange = false;										// I start with the player out of range
		chasing = false;										// I start by not chasing the player
		patrolIndex = 0;										// I am to go to the first patrol point
//		lkp = Vector3.zero;										// the player's default last known position
		hasAttacked = false;										// I start without attacking anyone
		playerHealth = player.GetComponent ("PlayerController") as PlayerController;
		playerNav = player.GetComponent<NavMeshAgent> ();
		currentSprite.sprite = spriteList [spriteVal];
		chaseTimer = 12f;
	}
	
	// is called every frame
	void Update()
	{
		if (inSight && inRange && !hasAttackPath && !hasAttacked) {
			AquirePath ();
		} else if (hasAttacked) {
			reloading += Time.deltaTime;
		} else if (hasAttackPath) {
			Attack ();
		} else if (inSight) {
			Engage ();
		} else {
			Patrol ();
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
		anim.SetBool ("HasAttacked", hasAttacked);
		anim.SetBool ("AtPoint", atPoint);
		anim.SetBool ("Moving", moving);
		anim.SetFloat ("RecoverTime", reloading);
		anim.SetFloat ("ChaseTimer", chaseTimer);
		
		if (reloading > reloadTime) {
			hasAttacked = false;
			reloading = 0f;
			inSight = false;
			hasAttackPath = false;
		}
	}
	
	// this is how I attack the player
	void Attack(){
		nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		distance = (nav.pathEndPosition - gameObject.transform.position).magnitude;
		if (distance < damageRange) {
			hasAttacked = true;
			hasAttackPath = false;
		}

		distance = (player.transform.position - gameObject.transform.position).magnitude - damageRange;
		if (((nav.radius * gameObject.transform.localScale.x * 0.5f) + (playerNav.radius * player.transform.localScale.x * 0.5f) >= distance) && !hasAttacked) {
			
			hasAttacked = true;
			hasAttackPath = false;
			nav.Stop ();
			playerHealth.currentPlayerHealth -= 30;
		}
	}
	
	// This gets sets up my uppercut punch
	void AquirePath(){
		nav.Stop ();
		//	nav.speed = engageSpeed;
		Vector3 direction = player.transform.position - gameObject.transform.position;
		// I need to have a path of attack
		if (!hasAttackPath) {
			RaycastHit hit;
			Vector3 lkp = player.transform.position + direction;
			if (Physics.Raycast (player.transform.position + Vector3.up * 0.5f, direction.normalized, out hit, 2 * damageRange)) {
				
				if (nav.CalculatePath (hit.point, path)) {
					hasAttackPath = nav.SetPath (path);
				}
			} else if (nav.CalculatePath (lkp, path)) {
				hasAttackPath = nav.SetPath (path);
			}
		}
	}

	void Engage()
	{
		moving = false;
		chaseTimer = 0f;
		nav.Stop ();
		
		distance = (player.transform.position - transform.position).magnitude;	// how far away is the player from me
		
		// if the player is too far away
		if (distance > chargeRange) {
			Vector3 moveTo = Vector3.MoveTowards (transform.position, player.transform.position, distance);	// I should move towards him
			if(nav.CalculatePath (moveTo, path))					// I need to find a good path to get the player close enough
				nav.SetPath (path);									// I can start moving to that position
			// if the player is too close
		} else if (distance <= chargeRange) {
			inRange = true;										// now I can attack the player
		}
	}

	// What I am doing the rest of the time
	void Patrol(){
		moving = true;
		nav.speed = normalSpeed;								// I am moveing slowly, don't want to miss anything
		// if I am at one of the patrol points
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
		
		nav.CalculatePath(patrol[patrolIndex].position, path);	// calculate a good path to the patrol point
		nav.SetPath (path);										// start walking that path
		facing = (nav.steeringTarget - transform.position).normalized;	// make sure I am facing the way I am walking
		facing.y = 0f;
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
					if (Physics.Raycast (transform.position + Vector3.up * 0.5f, direction.normalized, out hit, 30f)) {
						if (hit.collider.gameObject == player) {
							if(!inSight)
							{
								facing = direction.normalized;
							}
							inSight = true;
						}else {
							inSight = false;
							hasAttackPath = false;
						}
					}
				} 
			} else {
				float angle = Vector3.Angle (lkf, direction);
				if (angle < (fov * 0.5f)) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position + Vector3.up * 0.5f, direction.normalized, out hit, 30f)) {
						if (hit.collider.gameObject == player) {
							if(!inSight)
							{
								facing = direction.normalized;
							}
							inSight = true;
						}else {
							inSight = false;
							hasAttackPath = false;
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
			reloading = 10f;
		}
	}
}
