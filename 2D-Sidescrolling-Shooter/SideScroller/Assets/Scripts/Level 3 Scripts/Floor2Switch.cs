using UnityEngine;
using System.Collections;

public class Floor2Switch : MonoBehaviour {

	public GameObject floor2;
	public GameObject switch7;
	public bool show = false;
	// Use this for initialization
	void Start () {
		floor2.SetActive (true);
		switch7.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch7.collider) {
			show = !show;
			floor2.SetActive(show);
		}
		if (show == true) {
			switch7.renderer.material.color = Color.green;
		}
		else {
			switch7.renderer.material.color = Color.magenta;
		}
	}
}
