using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

	private PlayerController pcInfo;

	public int eAmmoAmount = 5;
	public int pAmmoAmount = 25;
	public int sAmmoAmount = 10;
	public int healthAmount = 10;

	void OnTriggerStay(Collider other)
	{
		pcInfo = other.GetComponent ("PlayerController") as PlayerController;
		int mathTempVar;
		if (other.gameObject.tag == "Player") {

			if (gameObject.tag == "Extra Life") {
				pcInfo.currentPlayerHealth += healthAmount;
				Destroy (gameObject);
				Debug.Log ("Extra Life was destroyed");
			}
			if (gameObject.tag == "Egg Splode Ammo") {
				mathTempVar = pcInfo.maxEAmmo - pcInfo.currentEAmmo;
				if(mathTempVar == 0)
				{

				}else if (pcInfo.egg && mathTempVar < eAmmoAmount) {
					pcInfo.currentEAmmo += mathTempVar;
					PlayerPrefs.SetInt ("EggAmmo", pcInfo.currentEAmmo);
					Destroy (gameObject);
					Debug.Log ("Egg Splode Ammo was destroyed");
				} else if (pcInfo.egg && mathTempVar > eAmmoAmount) {
					pcInfo.currentEAmmo += eAmmoAmount;
					PlayerPrefs.SetInt ("EggAmmo", pcInfo.currentEAmmo);
					Destroy (gameObject);
					Debug.Log ("Egg Splode Ammo was destroyed");
				}
			}
			if (gameObject.tag == "Pellet Ammo") {
				mathTempVar = pcInfo.maxPAmmo - pcInfo.currentPAmmo;
				if(mathTempVar == 0)
				{
					
				}else if (pcInfo.pellet && mathTempVar < pAmmoAmount) {
					pcInfo.currentPAmmo += mathTempVar;
					PlayerPrefs.SetInt ("PelletAmmo", pcInfo.currentPAmmo);
					Destroy (gameObject);
					Debug.Log ("Pellet Ammo was destroyed");
				} else if (pcInfo.pellet && mathTempVar > pAmmoAmount) {
					pcInfo.currentPAmmo += pAmmoAmount;
					PlayerPrefs.SetInt ("PelletAmmo", pcInfo.currentPAmmo);
					Destroy (gameObject);
					Debug.Log ("Pellet Ammo was destroyed");
				}
			}
			if (gameObject.tag == "Shotgun Ammo") {
				mathTempVar = pcInfo.maxSAmmo - pcInfo.currentSAmmo;
				if(mathTempVar == 0)
				{
					
				}else if (pcInfo.shot && mathTempVar < sAmmoAmount) {
					pcInfo.currentSAmmo += mathTempVar;
					PlayerPrefs.SetInt ("ShotAmmo", pcInfo.currentSAmmo);
					Destroy (gameObject);
					Debug.Log ("Shotgun Ammo was destroyed");
				} else if (pcInfo.shot && mathTempVar > sAmmoAmount) {
					pcInfo.currentSAmmo += sAmmoAmount;
					PlayerPrefs.SetInt ("ShotAmmo", pcInfo.currentSAmmo);
					Destroy (gameObject);
					Debug.Log ("Shotgun Ammo was destroyed");
				}
			}
			if (gameObject.tag == "Egg Splode Mod") {
				pcInfo.egg = true;
				pcInfo.currentEAmmo += 10;
				PlayerPrefs.SetInt ("EggSplodeMod", 1);
				PlayerPrefs.SetInt ("EggAmmo", pcInfo.currentEAmmo);
				Destroy (gameObject);
				Debug.Log ("Egg Splode Mod was destroyed");
			}
			if (gameObject.tag == "Pellet Nozzle") {
				pcInfo.pellet = true;
				pcInfo.currentPAmmo += 40;
				PlayerPrefs.SetInt ("PelletNozzle", 1);
				PlayerPrefs.SetInt ("PelletAmmo", pcInfo.currentPAmmo);
				Destroy (gameObject);
				Debug.Log ("Pellet Nozzle was destroyed");
			}
			if (gameObject.tag == "Shotgun Rain") {
				pcInfo.shot = true;
				pcInfo.currentSAmmo += 15;
				PlayerPrefs.SetInt ("ShotgunRain", 1);
				PlayerPrefs.SetInt ("ShotAmmo", pcInfo.currentSAmmo);
				Destroy (gameObject);
				Debug.Log ("Shotgun Rain was destroyed");
			}
		}
	}
}
