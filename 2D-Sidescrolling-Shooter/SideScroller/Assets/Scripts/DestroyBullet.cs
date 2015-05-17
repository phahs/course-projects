using UnityEngine;
using System.Collections;

public class DestroyBullet : MonoBehaviour {
	
	void OnCollisionEnter(){
		Destroy(gameObject);
	}
}
