using UnityEngine;
using System.Collections;

public class SwitchStairs3 : MonoBehaviour {

	public GameObject stairs3;
	public GameObject switch4;
	public bool show = false;
	// Use this for initialization
	void Start () {
		stairs3.SetActive (true	);
		switch4.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch4.collider) {
			show = !show;
			stairs3.SetActive(show);
		}
		if (show == true) {
			switch4.renderer.material.color = Color.green;
		}
		else {
			switch4.renderer.material.color = Color.magenta;
		}
	}
}
