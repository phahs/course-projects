using UnityEngine;
using System.Collections;

public class SwitchPlatform1 : MonoBehaviour {
	
	public GameObject platform1;
	public GameObject switch1;
	public bool show = false;

	// Use this for initialization
	void Start () {
		platform1.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (switch1.collider) {
			show = !show;
			platform1.SetActive(show);
		}
		if (show == true) {
						switch1.renderer.material.color = Color.green;
				}
		else {
			switch1.renderer.material.color = Color.magenta;
		}
	}
}
