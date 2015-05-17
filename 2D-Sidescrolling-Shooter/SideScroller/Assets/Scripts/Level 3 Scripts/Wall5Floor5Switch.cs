using UnityEngine;
using System.Collections;

public class Wall5Floor5Switch : MonoBehaviour {

	public GameObject wall5;
	public GameObject floor5;
	public GameObject switch4;
	public GameObject wall8;
	public bool show = false;
	// Use this for initialization
	void Start () {
		wall5.SetActive (true);
		floor5.SetActive (true);
		wall8.SetActive (true);
		switch4.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch4.collider) {
			show = !show;
			wall5.SetActive(show);
			floor5.SetActive(show);
			wall8.SetActive(show);
		}
		if (show == true) {
			switch4.renderer.material.color = Color.green;
		}
		else {
			switch4.renderer.material.color = Color.magenta;
		}
	}
}
