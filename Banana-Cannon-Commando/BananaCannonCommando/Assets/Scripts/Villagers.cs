	using UnityEngine;
using System.Collections;

public class Villagers : MonoBehaviour {
	
	private float timer;
	private float waitTime;
	private float radius;
	private int spriteNumber;
	private Vector3 startPos;
	private bool isMoving;
	private bool atPoint;
	private NavMeshAgent nav;
	private NavMeshPath path;
	private SpriteRenderer currentSprite;
	private Vector3 lkf;
	private Vector3 thisDest;
	private Animator anim;
	private Facing setSprite;
	private Transform body_sprite;
	public Sprite [] spriteList; // 0 is front, 8 is frontleft

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		path = new NavMeshPath ();
		body_sprite = transform.GetChild (0);
		currentSprite = body_sprite.GetComponent<SpriteRenderer> ();
		setSprite = GetComponent<Facing> ();
		anim = GetComponent<Animator> ();
		startPos = gameObject.transform.position;
		isMoving = false;
		atPoint = true;
		timer = 0f;
		radius = 5f;
		waitTime = Random.Range(0.5f, radius);
		spriteNumber = spriteList.Length;
		currentSprite.sprite = spriteList [Random.Range(0, spriteNumber)];
		thisDest = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		if (atPoint) {
			timer += Time.deltaTime;
			if (timer >= waitTime) {
				Roam ();
				waitTime = Random.Range(0.5f, radius);
				thisDest = nav.destination;
				atPoint = false;
				isMoving = true;
				timer = 0f;
			}
		}
		else if (isMoving) {
			if(transform.position.x == thisDest.x && transform.position.z == thisDest.z)
			{
				atPoint = true;
				isMoving = false;
				timer = 0f;
			}
		}
		
		int tempNum = 0;

		Vector3 direction = (nav.steeringTarget - transform.position).normalized;
		direction.y = 0f;

		if (direction != Vector3.zero) {
			lkf = direction;
			tempNum = setSprite.DetermineSprite (lkf);
		} else {
			tempNum = setSprite.DetermineSprite (direction);
		}

		if (tempNum != spriteNumber) {
			currentSprite.sprite = spriteList [tempNum];
			spriteNumber = tempNum;
		}


		anim.SetBool ("IsMoving", isMoving);
		anim.SetBool ("AtPoint", atPoint);
	}

	void Roam()
	{
		Vector3 randomDirection = Random.insideUnitSphere * radius;
		randomDirection += startPos;
		NavMeshHit hit;
		NavMesh.SamplePosition (randomDirection, out hit, radius, 1);
		Vector3 finalPosition = hit.position;
		bool haspath;
		haspath = nav.CalculatePath (finalPosition, path);
		if (haspath) {
			haspath = nav.SetPath (path);
		}
	}
}
