using UnityEngine;
using System.Collections;
// this is how all enemy apples will behave
public class AppleAI : MonoBehaviour {

	public GameObject projectile;				// what I will throw at the player
	public Vector3 facing;						// the direction I am facing
	public Transform[] patrol;
	public int patrolIndex;
	public bool hasAttacked;
	public float reloadTimer;
	public float patrolWait = 3f;				// time I wait at patrol points
	public float chaseFor = 6f;					// how long I chase after runaway enemies (the player) for
	public float reloadTime = 3f;				// how long it takes me to attack the player again
	public float fov = 110f;					// how much of the world I can see
	public float minRange = 15f;				// how close I can be and still be able to attack the player
	public float maxRange = 20f;				// how far I can be and still be able to attack the player
	public float throwForce = 1000f;			// how hard I throw
	public Sprite[] spriteList;

	// Variables for the Animations
	public bool inSight;
	public bool inRange;
	public bool atPoint;
	public bool moving;
	public float chaseTimer;
	
	private GameObject player;					// the evil and vile player
	private NavMeshAgent nav;
	private NavMeshPath path;
	private Animator anim;
	private EnemyHealth myHealth;
	private DropItem randomItem;
	private Facing changeFacing;
	private Vector3 lkf;						// the last known direction that I was facing
	private Vector3 lkp;						// the last known position of the player
	private float patrolTimer = 0f;
	private float distance;						// the distance between myself and the player
	private SpriteRenderer currentSprite;
	private int spriteVal = 0;
	private Transform body;
	
	// Use this for initialization
	void Start () {
		body = transform.GetChild(0);
		nav = gameObject.GetComponent<NavMeshAgent> ();
		path = new NavMeshPath ();
		anim = gameObject.GetComponent<Animator> ();
		myHealth = GetComponent<EnemyHealth> ();
		randomItem = GetComponent<DropItem> ();
		currentSprite = body.GetComponent<SpriteRenderer> ();
		changeFacing = GetComponent<Facing> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		atPoint = false;
		inSight = false;
		inRange = false;
		moving = false;
		hasAttacked = false;
		chaseTimer = 8f;
		reloadTimer = 0f;
		facing = transform.position + Vector3.forward;
		lkp = Vector3.zero;
		currentSprite.sprite = spriteList [spriteVal];
	}
	
	// Update is called once per frame
	void Update () {
		if (inSight && inRange) {
			Attack ();
		} else if (inSight) {
			Engage ();
		} else if (chaseFor >= chaseTimer) {
			Chase();
		} else {
			Patrol ();
		}

		int tempVal;
		if (facing != Vector3.zero) {
			tempVal = changeFacing.DetermineSprite (facing);
		} else {
			tempVal = changeFacing.DetermineSprite(lkf);
		}
		if (tempVal != spriteVal) {
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
	
	void Patrol()
	{
		moving = true;
		bool hasPath;
		if(transform.position.x == patrol [patrolIndex].position.x &&
		   transform.position.z == patrol [patrolIndex].position.z){
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
		
		hasPath = nav.CalculatePath(patrol[patrolIndex].position, path);	// calculate a good path to the patrol point
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
	
	void Engage()
	{
		moving = false;
		chaseTimer = 0f;
		nav.Stop ();
		
		distance = (player.transform.position - transform.position).magnitude;	// how far away is the player from me
		
		// if the player is too far away
		if (distance > maxRange) {
			Vector3 moveTo = Vector3.MoveTowards (transform.position, player.transform.position, distance - maxRange + 1f);	// I should move towards him
			if(nav.CalculatePath (moveTo, path))					// I need to find a good path to get the player close enough
				nav.SetPath (path);									// I can start moving to that position
			// if the player is too close
		}
		if (minRange > distance) {
			Vector3 moveTo = Vector3.MoveTowards (transform.position, player.transform.position, distance - minRange - 1f);	// I should move away from him
			if(nav.CalculatePath (moveTo, path))					// I need to find a good path to get farther from the player
				nav.SetPath (path);									// I can start moving to that position
			// if the player is in my range's sweet spot
		} else if (minRange <= distance && distance <= maxRange) {
			inRange = true;										// now I can attack the player
		}
	}
	
	void Chase()
	{
		chaseTimer += Time.deltaTime;
		nav.CalculatePath (lkp, path);					// I make sure I have an up to date path to the players last known position
		nav.SetPath (path);								// I then follow that path
		facing = (nav.steeringTarget - transform.position).normalized;	// I make sure that I am facing in the direction I am running
		facing.y = 0f;
		// if I am not facing zero (that dark abyss)
		if (facing != Vector3.zero) {
			lkf = facing;								// I keep a record of the way I am facing, in case I do face zero, has been known to happen
		}
		// if, during my chase, I can see the player
		if (inSight) { 	
			chaseTimer = 9f;							// I can reset my chase timer in case the player runs away again
		}
		// if I have chased the player for long enough
		else if( chaseFor < chaseTimer){
			lkp = Vector3.zero;								// I can set his last known position back to the default
		}
	}
	
	
	void Attack()
	{
		// if I have thrown a projectile
		if (hasAttacked) {
			reloadTimer += Time.deltaTime;						// I need to take the time to properly reload
			// if I have taken the necessary time to reload
			if (reloadTimer >= reloadTime) {
				hasAttacked = !hasAttacked;							// I am able to fire again
				reloadTimer = 0;									// I will need to start the reloading process from scratch
			}
		}else{
			Throw();
		}
		distance = (player.transform.position - transform.position).magnitude;	// how far away is the player while I am aiming
		// if the player gets too close or goes to far
		if (distance <= minRange || distance >= maxRange) {
			inRange = false;									// he's out of my range
		}
	}
	
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
							inSight = true;
							facing = direction.normalized;
							facing.y = 0f;
							lkp = player.transform.position;
						}else {
							inSight = false;
							inRange = false;
							if(lkp != Vector3.zero)
							{
								chaseTimer = 0f;
							}
						}
					}
				} 
			} else {
				float angle = Vector3.Angle (lkf, direction);
				if (angle < (fov * 0.5f)) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position, direction.normalized, out hit, 30f)) {
						if (hit.collider.gameObject == player) {
							inSight = true;
							facing = direction.normalized;
							facing.y = 0f;
							lkp = player.transform.position;
						}else {
							inSight = false;
							inRange = false;
							if(lkp != Vector3.zero)
							{
								chaseTimer = 0f;
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
	
	void Throw ()
	{
		GameObject newProjectile;								// my thrown weapon
		// set what I am going to throw
		newProjectile = (GameObject)Instantiate (projectile, gameObject.transform.position
		                                         + Vector3.up * 0.5f + facing * 2.0f, transform.localRotation);
		// if what I am throwing does have a rigidbody
		if (newProjectile.rigidbody != null) {
			newProjectile.rigidbody.AddForce (facing * throwForce);	// add force to what I am throwing
		}
		hasAttacked = !hasAttacked;								// I have now fired and must reload
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Base Cannon" || collision.gameObject.tag == "Egg Splode"
			|| collision.gameObject.tag == "Pellet" || collision.gameObject.tag == "Shotgun") {
			nav.Stop ();
			Vector3 direction = player.transform.position - transform.position;
			facing = direction.normalized;
			facing.y = 0f;
			inSight = true;
		}
	}
}