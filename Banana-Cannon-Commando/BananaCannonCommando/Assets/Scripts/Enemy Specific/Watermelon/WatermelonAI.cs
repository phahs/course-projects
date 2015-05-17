using UnityEngine;
using System.Collections;

public class WatermelonAI : MonoBehaviour {
	
	public float normalSpeed;				// normal walking speed to get from point a to point b
	public float engageSpeed;				// how fast I move when I have an enemy in my sights
	public float patrolWait = 3f;				// time I wait at patrol points
	public float range = 4f;					// how close I need to be to damage the player
	public float fov = 110f;					// how much of the world I can see
	public Vector3 facing;						// the direction I am facing
	public Transform[] patrol;					// list of places I patrol
	public Sprite[] spriteList;

	// For Animations
	public bool inSight;						// can I see the player?
	public bool hasAttacked;					// have I successfully attacked?
	public bool atPoint;
	public bool moving;
	public float reloadTime;					// how long it takes me to attack the player again

	private Vector3 lkf;						// the last known direction that I was facing
	private float distance;						// the distance between myself and the player
	private NavMeshAgent nav;					// the navigation mesh that allows me to roam around
	private NavMeshPath path;					// the path to get from point a to point b
	private Facing changeFacing;
	private Animator anim;
	private SpriteRenderer currentSprite;
	private float patrolTimer;					// makes sure I keep my patrol in check
	private float reloading;					// lets me know that I am done reloading
	private int patrolIndex;					// lets me know what patrol point I am headed to
	public bool hasAttackPath;
	private float timer;
	private GameObject player;					// the evil and vile player
	private EnemyHealth myHealth;
	private DropItem randomItem;
	private PlayerController playerHealth;
	private NavMeshAgent playerNav;
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
		hasAttackPath = false;									// I start without an attack path
		hasAttacked = false;									// I have not attacked
		atPoint = false;
		moving = false;
		reloadTime = 4f;
		patrolIndex = 0;										// I am to go to the first patrol point
		normalSpeed = 3f;
		engageSpeed = 15f;
		playerHealth = player.GetComponent ("PlayerController") as PlayerController;
		playerNav = player.GetComponent<NavMeshAgent> ();
	}
	
	// is called every frame
	void Update()
	{
		// if I can see the player I have not yet attacked the player
		if (inSight && !hasAttackPath && !hasAttacked) {
			AquirePath ();
			// if I can see the player and I have attacked
		} else if(hasAttacked)
		{
			reloading += Time.deltaTime;						// just waiting to "reload" or uncross my eyes rather
			//if I have aquired an attack path
		} else if (hasAttackPath) {
			Attack();
			// in all other cases
		}else{
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
		anim.SetBool ("HasAttacked", hasAttacked);
		anim.SetBool ("AtPoint", atPoint);
		anim.SetBool ("Moving", moving);
		anim.SetFloat ("RecoverWait", reloading);

		if (reloading >= reloadTime) {
			hasAttacked = false;
			reloading = 0f;
			inSight = false;
			hasAttackPath = false;
		}
	}

	// I am preparing to roll towards the player
	void AquirePath()
	{
		nav.Stop ();
	//	nav.speed = engageSpeed;
		Vector3 direction = player.transform.position - gameObject.transform.position;
		// I need to have a path of attack
		if (!hasAttackPath) {
			RaycastHit hit;
			Vector3 lkp = player.transform.position + direction;
			if (Physics.Raycast (player.transform.position + Vector3.up * 0.5f, direction.normalized, out hit, direction.magnitude)) {

				if (nav.CalculatePath (hit.point, path)) {
					hasAttackPath = nav.SetPath (path);
				}
			}else if (nav.CalculatePath (lkp, path)) {
				hasAttackPath = nav.SetPath (path);
			}
		}
	}

	// this is how I attack the player
	void Attack()
	{
		nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		distance = (nav.pathEndPosition - gameObject.transform.position).magnitude;
		if (distance < range) {
			hasAttacked = true;
		}

		distance = (player.transform.position - gameObject.transform.position).magnitude - range;
		if (((nav.radius * gameObject.transform.localScale.x * 0.5f) + (playerNav.radius * player.transform.localScale.x * 0.5f) >= distance) && !hasAttacked) {

			hasAttacked = true;
			nav.Stop ();
			playerHealth.currentPlayerHealth -= 40;
		}
	}
	
	// What I am doing the rest of the time
	void Patrol()
	{
		moving = true;
		nav.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
		nav.speed = normalSpeed;								// I am moveing slowly, don't want to miss anything
		// if I am at one of the patrol points
		if (transform.position.x == patrol [patrolIndex].position.x &&
		    transform.position.z == patrol [patrolIndex].position.z) {
			atPoint = true;
			moving = false;
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
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Base Cannon" || collision.gameObject.tag == "Egg Splode"
		    || collision.gameObject.tag == "Pellet" || collision.gameObject.tag == "Shotgun") {
	//		nav.Stop ();
			Vector3 direction = player.transform.position - transform.position;
			facing = direction.normalized;
			inSight = true;
			reloading = reloadTime;
		}
	}
}
