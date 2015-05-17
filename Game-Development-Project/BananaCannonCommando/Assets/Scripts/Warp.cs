using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour {

	public bool jump;
	public GameObject target;

	private GameObject player;
	private Warp destination;
	private NavMeshAgent playerNav;

	void Awake() {
		player = GameObject.FindGameObjectWithTag ("Player");
		destination = target.GetComponent<Warp>();
		// target.GetComponent (Warp);
		playerNav = player.GetComponent <NavMeshAgent>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (!jump) {
			if (other.tag == "Player") {
				destination.jump = true;
				playerNav.Warp(target.transform.position);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") {
			jump = false;
		}
	}
}
