using UnityEngine;
using System.Collections;

public class StuffCamera : MonoBehaviour {

		public Transform player;
		void Update ()
		{
			transform.position = new Vector3 (player.position.x,player.position.y, player.position.z -10);
		}
	}
