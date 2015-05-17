using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {
	
	public int doYouGetAnItem = 75;
	public GameObject health;
	public GameObject eggAmmo;
	public GameObject pelletAmmo;
	public GameObject shotAmmo;

	private GameObject player;
	private PlayerController pcInfo;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		pcInfo = player.GetComponent("PlayerController") as PlayerController;
	}

	public void ItemDrop()
	{
		int numItems = 0;
		int rand = Random.Range (1, 101);
		Debug.Log ("Do we get an item? " + rand);
		if (pcInfo.egg) {
			numItems += 1;
		}
		if (pcInfo.pellet) {
			numItems += 1;
		}
		if (pcInfo.shot) {
			numItems += 1;
		}
		numItems += 1;
		Debug.Log ("how many item do we get to choose from: " + numItems);
		if (rand <= doYouGetAnItem) {
			rand = Random.Range (1, 101);
			int itemNum = rand % numItems;
			Debug.Log("we rolled " + rand + " and have " + numItems + " items, so we get item number " + itemNum);
			if (itemNum == 0) {
				GameObject.Instantiate (health, transform.position, transform.localRotation);
			}
			if (itemNum == 1) {
				GameObject.Instantiate (eggAmmo, transform.position, transform.localRotation);
			}
			if (itemNum == 2) {
				GameObject.Instantiate (pelletAmmo, transform.position, transform.localRotation);
			}
			if (itemNum == 3) {
				GameObject.Instantiate (shotAmmo, transform.position, transform.localRotation);
			}
		}
	}
}
