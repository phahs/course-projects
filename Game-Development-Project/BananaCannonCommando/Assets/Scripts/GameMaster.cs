using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	private GameObject player;
	private Transform playerPos;
	private Vector3 position;

	void Awake(){
		player = GameObject.FindGameObjectWithTag ("Player");
		playerPos = player.GetComponent<Transform> ();
	}

	void Start(){
		position.x = PlayerPrefs.GetFloat ("FruitopiaPosX");
		position.y = PlayerPrefs.GetFloat ("FruitopiaPosY");
		position.z = PlayerPrefs.GetFloat ("FruitopiaPosZ");

		playerPos.position = position;
		player.transform.position = playerPos.position;
	}

	void Update()
	{
	//	Debug.Log("what is being saved: " + player.transform.position);
		PlayerPrefs.SetFloat ("FruitopiaPosX", player.transform.position.x);
		PlayerPrefs.SetFloat ("FruitopiaPosZ", player.transform.position.z);
	}
}
