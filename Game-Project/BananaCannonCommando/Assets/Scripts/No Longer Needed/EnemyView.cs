using UnityEngine;
using System.Collections;
// this script is only meant to determine if the player is within sight of the enemy
public class EnemyView : MonoBehaviour {

	public float fov = 110f;
	public bool isPlayerInSight;
	public Vector3 facing;

	private GameObject player;

	void Start()
	{
		isPlayerInSight = false;
	}

	void OnTriggerStay(Collider other)
	{

	}
}		