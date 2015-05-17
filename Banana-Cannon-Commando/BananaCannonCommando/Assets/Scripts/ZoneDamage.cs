using UnityEngine;
using System.Collections;

public class ZoneDamage : MonoBehaviour {

	public int playerZoneDamage = 5;
	public float playerZoneDuration = 10f;
	public int enemyZoneDamage = 10;
	public float enemyZoneDuration = 5f;

	private PlayerController playerInfo;
	private EnemyHealth enemy;
	private float destroyTimer;
	private float damageTimer;
	private CapsuleCollider cap;
	private CapsuleCollider capE;

	void Start()
	{
		cap = gameObject.GetComponent<CapsuleCollider>();
	}

	void Update()
	{
		destroyTimer += Time.deltaTime;
		damageTimer += Time.deltaTime;

		if (gameObject.tag == "Player Zone") {
			if (destroyTimer >= playerZoneDuration) {
				Destroy (gameObject);
			}
		}
		if (gameObject.tag == "Enemy Zone") {
			if (destroyTimer >= enemyZoneDuration) {
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
	//	Debug.Log (other.collider.GetType ());
	//	if (other.collider.GetType() == CapsuleCollider) {
			if (other.gameObject.tag == "Player" && gameObject.tag == "Enemy Zone") {
				// player takes enemy zone damage
				if (damageTimer >= 1f) {
					playerInfo = other.gameObject.GetComponent ("PlayerController") as PlayerController;
					playerInfo.currentPlayerHealth -= enemyZoneDamage;
					damageTimer = 0;
				}
			}
		Vector3 distance = other.gameObject.transform.position - gameObject.transform.position;
		capE = other.gameObject.GetComponent<CapsuleCollider> ();
		if (((other.gameObject.tag == "AppleE" || other.gameObject.tag == "OrangeE" ||
		      other.gameObject.tag == "TomatoE" || other.gameObject.tag == "GrapeE" ||
		      other.gameObject.tag == "WatermelonE" || other.gameObject.tag == "Orpple" ||
		      other.gameObject.tag == "MachoMato" || other.gameObject.tag == "Dr. Rotten") && 
		     gameObject.tag == "Player Zone") &&
		     distance.magnitude < (gameObject.transform.localScale.x * cap.radius + other.gameObject.transform.localScale.x * capE.radius)) {
				if (damageTimer >= 1f) {
				Debug.Log("Dealing damage");
					enemy = other.gameObject.GetComponent ("EnemyHealth") as EnemyHealth;
					enemy.currentHealth -= playerZoneDamage;
					damageTimer = 0;
				}
			}
	//	}
	}
}
