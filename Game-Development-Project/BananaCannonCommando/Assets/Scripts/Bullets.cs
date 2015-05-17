using UnityEngine;
using System.Collections;

public class Bullets : MonoBehaviour {
	public int cannonDamage = 10;
	public int eggDamage = 20;
	public int pelletDamage = 15;
	public int shotDamage = 40;
	public int appleDamage = 8;
	public int orangeDamage = 12;
	public GameObject enemyAcidZone;
	public GameObject playerAcidZone;

	private PlayerController playerInfo;
	private EnemyHealth enemy;


	void OnCollisionEnter(Collision collision){
		//always destroy the bullet once it hits something
		playerInfo = collision.gameObject.GetComponent ("PlayerController") as PlayerController;
		if (collision.gameObject.tag == "Player") {
			if (gameObject.tag == "Apple Seed") {
				playerInfo.currentPlayerHealth -= appleDamage;
				Destroy (gameObject);
			}
		}
		if (gameObject.tag == "Orange Juice") {
			if (collision.gameObject.tag == "Player") {
				playerInfo.currentPlayerHealth -= orangeDamage;
			}
			Vector3 spawnPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
			Quaternion spawnRot = new Quaternion(0,0,0,1);
			GameObject.Instantiate(enemyAcidZone, spawnPos, spawnRot);
			Destroy (gameObject);
		}
		if (collision.gameObject.tag == "AppleE" || collision.gameObject.tag == "OrangeE" ||
		    collision.gameObject.tag == "TomatoE" || collision.gameObject.tag == "GrapeE" ||
		    collision.gameObject.tag == "WatermelonE" || collision.gameObject.tag == "Orpple" ||
		    collision.gameObject.tag == "MachoMato" || collision.gameObject.tag == "Dr. Rotten") {
			enemy = collision.gameObject.GetComponent("EnemyHealth") as EnemyHealth;
			if (gameObject.tag == "Base Cannon") {
				enemy.currentHealth -= cannonDamage;
				Destroy (gameObject);
				Debug.Log("hit the enemy");
			}
			if (gameObject.tag == "Pellet") {
				enemy.currentHealth -= pelletDamage;
				Destroy (gameObject);
			}
			if (gameObject.tag == "Shotgun") {
				enemy.currentHealth -= shotDamage;
				Destroy (gameObject);
			}
		}
		if (gameObject.tag == "Egg Splode") {
			if(collision.gameObject.tag == "AppleE" || collision.gameObject.tag == "OrangeE" ||
			   collision.gameObject.tag == "TomatoE" || collision.gameObject.tag == "GrapeE" ||
			   collision.gameObject.tag == "WatermelonE" || collision.gameObject.tag == "Orpple" ||
			   collision.gameObject.tag == "MachoMato" || collision.gameObject.tag == "Dr. Rotten")
			{
				enemy.currentHealth -= eggDamage;
			}
			Vector3 spawnPos = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
			Quaternion spawnRot = new Quaternion(0,0,0,1);
			GameObject.Instantiate(playerAcidZone, spawnPos, spawnRot);
			Destroy (gameObject);
		}
		Destroy (gameObject);
	}
}
