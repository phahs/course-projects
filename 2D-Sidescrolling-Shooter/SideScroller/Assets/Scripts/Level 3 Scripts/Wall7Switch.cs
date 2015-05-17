using UnityEngine;
using System.Collections;

public class Wall7Switch : MonoBehaviour {

	public GameObject wall7;
	public GameObject switch9;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall7.SetActive (true);
		switch9.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch9.collider) {
			show = !show;
			wall7.SetActive(show);
		}
		if (show == true) {
			switch9.renderer.material.color = Color.green;
		}
		else {
			switch9.renderer.material.color = Color.magenta;
		}
	}
}
