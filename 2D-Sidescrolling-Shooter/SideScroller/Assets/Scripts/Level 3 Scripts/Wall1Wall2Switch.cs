using UnityEngine;
using System.Collections;

public class Wall1Wall2Switch : MonoBehaviour {

	public GameObject wall1;
	public GameObject wall2;
	public GameObject switch3;
	public GameObject platform6;
	public bool show = true;
	// Use this for initialization
	void Start () {
		wall1.SetActive (true);
		wall2.SetActive (true);
		platform6.SetActive (false);
		switch3.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (switch3.collider) {
			show = !show;
			wall1.SetActive(show);
			wall2.SetActive(show);
			platform6.SetActive(!show);
		}
		if (show == true) {
			switch3.renderer.material.color = Color.green;
		}
		else {
			switch3.renderer.material.color = Color.magenta;
		}
	}
}
