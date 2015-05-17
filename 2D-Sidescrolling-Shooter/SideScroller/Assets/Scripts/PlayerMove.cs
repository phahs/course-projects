using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	CharacterController cc;
	public float moveSpeed = 10.0f;
	public float jumpVelocity = 7.5f;
	public float horizontalAirSpeed = 0.0f;
	float currentVelocity = 0.0f;
	private Vector3 targetAngles;

	public bool pressedQ = false;
	// Use this for initialization
	void Start () {
		cc = GetComponentInChildren<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		horizontalAirSpeed = moveSpeed / 2.0f;


		if (Input.GetKeyDown (KeyCode.Q)) {
			targetAngles = transform.eulerAngles + 180f * Vector3.up; // what the new angles should be
			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 1); // lerp to new angles
			pressedQ = !pressedQ;
				}

		if (pressedQ == false) {
			if (Input.GetKey (KeyCode.A)) {

				if(cc.isGrounded){
					if(Input.GetKey (KeyCode.Space)){
						currentVelocity = jumpVelocity;
						cc.Move (transform.right*(-moveSpeed + horizontalAirSpeed) *Time.deltaTime);
					}
					else{
						cc.Move (transform.right*-moveSpeed *Time.deltaTime);
					}
				}
				else{
					cc.Move (transform.right * -horizontalAirSpeed * Time.deltaTime);
				}
			}
			
			if (Input.GetKey (KeyCode.D)) {
				if(cc.isGrounded){
					if(Input.GetKey (KeyCode.Space)){
						currentVelocity = jumpVelocity;
						cc.Move (transform.right*(moveSpeed + horizontalAirSpeed) *Time.deltaTime);
					}
					else{
						cc.Move (transform.right*moveSpeed *Time.deltaTime);
					}
				}
				else{
					cc.Move (transform.right * horizontalAirSpeed * Time.deltaTime);
				}
			}
			
			if (cc.isGrounded && Input.GetKey (KeyCode.Space)) {  // if left/right arrows down when space is pressed, modify hAirSpeed by move speed
				currentVelocity = jumpVelocity;
				
				if(Input.GetKey (KeyCode.D)){
					// cc.Move (transform.right*(moveSpeed + horizontalAirSpeed) *Time.deltaTime);
				}
				if(Input.GetKey (KeyCode.A)){
					// cc.Move (transform.right*(-moveSpeed + horizontalAirSpeed) *Time.deltaTime);
				}
				else{
					cc.Move (transform.right * horizontalAirSpeed * Time.deltaTime);
				}
			}
			
			currentVelocity -= 9.8f * Time.deltaTime;
			cc.Move (transform.up * currentVelocity * Time.deltaTime);
		}
		//I TURNED AROUND!!!
		if (pressedQ == true) {
			if (Input.GetKey (KeyCode.A)) {
				
				if(cc.isGrounded){
					if(Input.GetKey (KeyCode.Space)){
						currentVelocity = jumpVelocity;
						cc.Move (-transform.right*(-moveSpeed + horizontalAirSpeed) *Time.deltaTime);
					}
					else{
						cc.Move (-transform.right*-moveSpeed *Time.deltaTime);
					}
				}
				else{
					cc.Move (-transform.right * -horizontalAirSpeed * Time.deltaTime);
				}
			}
			
			if (Input.GetKey (KeyCode.D)) {
				if(cc.isGrounded){
					if(Input.GetKey (KeyCode.Space)){
						currentVelocity = jumpVelocity;
						cc.Move (-transform.right*(moveSpeed + horizontalAirSpeed) *Time.deltaTime);
					}
					else{
						cc.Move (-transform.right*moveSpeed *Time.deltaTime);
					}
				}
				else{
					cc.Move (-transform.right * horizontalAirSpeed * Time.deltaTime);
				}
			}
			
			if (cc.isGrounded && Input.GetKey (KeyCode.Space)) {  // if left/right arrows down when space is pressed, modify hAirSpeed by move speed
				currentVelocity = jumpVelocity;
				
				if(Input.GetKey (KeyCode.D)){
					// cc.Move (transform.right*(moveSpeed + horizontalAirSpeed) *Time.deltaTime);
				}
				if(Input.GetKey (KeyCode.A)){
					// cc.Move (transform.right*(-moveSpeed + horizontalAirSpeed) *Time.deltaTime);
				}
				else{
					cc.Move (-transform.right * horizontalAirSpeed * Time.deltaTime);
				}
			}
			
			currentVelocity -= 9.8f * Time.deltaTime;
			cc.Move (transform.up * currentVelocity * Time.deltaTime);

		}
				}
}


		