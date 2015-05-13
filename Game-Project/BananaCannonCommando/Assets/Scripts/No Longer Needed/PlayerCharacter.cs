using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	// Base start for health and ammo
	public int health;
	public int cAmmo;
	public int eAmmo;
	public int pAmmo;
	public int sAmmo;
	// ammo and health capacities
	public int cAmmoTotal = 60;
	public int eAmmoTotal = 40;
	public int pAmmoTotal = 100;
	public int sAmmoTotal = 50;
	public int healthTotal = 100;
	// item values
/*	public int cAmmoAmount = 15;
	public int eAmmoAmount = 5;
	public int pAmmoAmount = 25;
	public int sAmmoAmount = 10;
	public int healthAmount = 10;
*/	// weapon attachment status
	public static bool cannon = true;
	public bool egg = false;
	public bool pellet = false;
	public bool shot = false;
	// cooldown time for the weapon attachments
	public static float cannonInterval = 0.5f;
	public static float eggInterval = 3f;
	public static float pelletInterval = 0.5f;
	public static float shotInterval = 2f;
	public float timer;
	private bool hasFired = false;
	// shooting and bullets
	public GameObject cannonPrefab;
	public GameObject eggPrefab;
	public GameObject pelletPrefab;
	public GameObject shotPrefab;
	public float cannonForce = 600f;
	public float eggForce = 600f;
	public float pelletForce = 600f;
	public float shotForce =  600f;
	private enum weaponAttachment {cannon, egg, pellet, shot};
	public int attachment = (int)weaponAttachment.cannon;

	public GameObject player;
	private PlayerMovement pm;
	private ItemPickup ip;

	void Start()
	{
		health = 50;
		cAmmo = 15;
		eAmmo = 0; 
		pAmmo = 0;
		sAmmo = 0;
	}

	void OnTriggerStay(Collider other)
	{
		ip = other.gameObject.GetComponent ("ItemPickup") as ItemPickup;

		int ammoTempVar;
		// pick objects up
/*		if (other.gameObject.tag == "Extra Life") {
			// pick up an extra life
			if( health < healthTotal){
				Debug.Log ("actor picked up more health");
				health += ip.healthAmount;
			}
		}

		if (other.gameObject.tag == "Base Cannon Ammo") {
			// gain cannon ammo
			ammoTempVar = cAmmoTotal - cAmmo;
			if(ammoTempVar < ip.cAmmoAmount){
				Debug.Log ("Actor picked up base cannon ammo");
				cAmmo += ammoTempVar;
			}
			else if(ammoTempVar > ip.cAmmoAmount)
			{
				Debug.Log ("Actor picked up base cannon ammo");
				cAmmo += ip.cAmmoAmount;
			}
		}
*/		
		if (other.gameObject.tag == "Egg Splode Ammo") {
			// gain ammo
			ammoTempVar = eAmmoTotal - eAmmo;
			if(ammoTempVar < ip.eAmmoAmount){
				Debug.Log ("Actor picked up base cannon ammo");
				eAmmo += ammoTempVar;
			}
			else if(ammoTempVar > ip.eAmmoAmount)
			{
				Debug.Log ("Actor picked up base cannon ammo");
				eAmmo += ip.eAmmoAmount;
			}
		}
		
		if (other.gameObject.tag == "Pellet Ammo") {
			// gain ammo
			ammoTempVar = pAmmoTotal - pAmmo;
			if(ammoTempVar < ip.pAmmoAmount){
				Debug.Log ("Actor picked up base cannon ammo");
				pAmmo += ammoTempVar;
			}
			else if(ammoTempVar > ip.pAmmoAmount)
			{
				Debug.Log ("Actor picked up base cannon ammo");
				pAmmo += ip.pAmmoAmount;
			}
		}
		
		if (other.gameObject.tag == "Shotgun Ammo") {
			// gain ammo
			ammoTempVar = sAmmoTotal - sAmmo;
			if(ammoTempVar < ip.sAmmoAmount){
				Debug.Log ("Actor picked up base cannon ammo");
				sAmmo += ammoTempVar;
			}
			else if(ammoTempVar > ip.sAmmoAmount)
			{
				Debug.Log ("Actor picked up base cannon ammo");
				sAmmo += ip.sAmmoAmount;
			}
		}
		
		if (other.gameObject.tag == "Egg Splode Mod") {
			// gain weapon attachment usability
			Debug.Log ("actor picked up the egg splode mod");
			egg = true;
			eAmmo = 10;
		}
		
		if (other.gameObject.tag == "Pellet Nozzle") {
			// gain weapon attachment usability
			Debug.Log ("actor picked up the pellet nozzle");
			pellet = true;
			pAmmo = 25;
		}
		
		if (other.gameObject.tag == "Shotgun Rain") {
			// gain weapon attachment usability
			Debug.Log ("actor picked up the shotgun rain");
			shot = true;
			sAmmo = 20;
		}
	}

	// Update is called once per frame
	void Update () {
		pm = player.GetComponent("PlayerMovement") as PlayerMovement;

	// switch weapon attachments
		if (Input.GetKey (KeyCode.Alpha1)) {
			attachment = (int)weaponAttachment.cannon;
			timer = cannonInterval;
		}
		if (Input.GetKey (KeyCode.Alpha2)) {
			attachment = (int)weaponAttachment.egg;
			timer = eggInterval;
		}
		if (Input.GetKey (KeyCode.Alpha3)) {
			attachment = (int)weaponAttachment.pellet;
			timer = pelletInterval;
		}
		if (Input.GetKey (KeyCode.Alpha4)) {
			attachment = (int)weaponAttachment.shot;
			timer = shotInterval;
		}

	// fire weapon

		if (Input.GetKey (KeyCode.Space)) {
			//determine which bullet prefab will be used based on active weapon attachment
			Debug.Log ("firing weapon");
			GameObject newBullet;

			switch (attachment) {
			case (int)weaponAttachment.cannon:
				if (!hasFired) {
					if (cAmmo > 0) {
						newBullet = (GameObject)Instantiate (cannonPrefab, transform.position + pm.facing * 1f, transform.localRotation);
						if (newBullet.rigidbody != null) {
							newBullet.rigidbody.AddForce (pm.facing * cannonForce);
						}
						hasFired = !hasFired;
						timer = Time.time + cannonInterval;
						cAmmo--;
					}
				}
				break;
			case (int)weaponAttachment.egg:
				if (egg) {
					if (!hasFired) {
						if (eAmmo > 0) {
							newBullet = (GameObject)Instantiate (eggPrefab, transform.position + pm.facing * 1f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (pm.facing * eggForce);
							}
							hasFired = !hasFired;
							timer = Time.time + eggInterval;
							eAmmo--;
						}
					}
				}
				break;
			case (int)weaponAttachment.pellet:
				if (pellet) {
					if (!hasFired) {
						if (pAmmo > 0) {
							newBullet = (GameObject)Instantiate (pelletPrefab, transform.position + pm.facing * 1f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (pm.facing * pelletForce);
							}
							hasFired = !hasFired;
							timer = Time.time + pelletInterval;
							pAmmo--;
						}
					}
				}
				break;
			case (int)weaponAttachment.shot:
				if (shot) {
					if (!hasFired) {
						if (sAmmo > 0) {
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + pm.facing * 1f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (pm.facing * shotForce);
							}
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + pm.facing * 1f
								+ transform.up * 0.2f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (pm.facing * shotForce);
							}
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + pm.facing * 1f
								+ transform.right * 0.2f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (pm.facing * shotForce);
							}
							newBullet = (GameObject)Instantiate (shotPrefab, transform.position + pm.facing * 1f
								- transform.right * 0.2f, transform.localRotation);
							if (newBullet.rigidbody != null) {
								newBullet.rigidbody.AddForce (pm.facing * shotForce);
							}
							hasFired = !hasFired;
							timer = Time.time + shotInterval;
							sAmmo--;
						}
					}
				}
				break;
			}

			if (Time.time >= timer) {
				hasFired = !hasFired;
			}
		}
		/*if (health <= 0) {
			Application.Quit();
			Destroy (gameObject);
		}*/
	}
}
