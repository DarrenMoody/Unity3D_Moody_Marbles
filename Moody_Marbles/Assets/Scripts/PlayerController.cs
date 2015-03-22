using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speedForce = 5;
	public float jumpForce = 2;
	private float maxMarbleRotate = 25;
	public bool jumpPressed;
	private bool needsTorque = true;
	private const float DistFromGround = 1f;
	private Rigidbody rigidMarble;
	private Transform cameraPos;
	private Vector3 camerasDirection;
	private Vector3 action;

	private void Awake() {
		if (Camera.main != null) { // If there is a main camera, get it's position
			cameraPos = Camera.main.transform;
		}
		else { // Else warn if there is no main camera
			Debug.LogWarning("No main camera found for camera-relative controls");
		}
	}

	private void Start() {
		rigidMarble = GetComponent<Rigidbody>();
		GetComponent<Rigidbody>().maxAngularVelocity = maxMarbleRotate;	
	}
	
	void Update() { // This functions somewhat like an actionlistener
		
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		jumpPressed = Input.GetButton("Jump");

		if (cameraPos != null) { // Find cameras movement direction
			camerasDirection = Vector3.Scale(cameraPos.forward, new Vector3(1, 0, 1)).normalized;
			action = (moveVertical * camerasDirection + moveHorizontal * cameraPos.right).normalized;
		}
		else {
			action = (moveVertical * Vector3.forward + moveHorizontal * Vector3.right).normalized;
		}
		/*if(Input.GetKeyDown (KeyCode.Space)) { // ADDS A JUMP ON SPACEBAR KEYDOWN
			if (inAir == false) {
				this.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0));
			}
		}
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().AddForce (movement * speedForce * Time.deltaTime);
		if(this.GetComponent<Rigidbody>().position.y > playerYPos) { // CHECKS IF PLAYER IS IN THE AIR
			inAir = true;											 // Only works on 0 Y - ground level 
			Debug.Log(this.GetComponent<Rigidbody>().position.y);
		}
		if (this.GetComponent<Rigidbody>().position.y <= playerYPos) 
			inAir = false;*/
	}

	void FixedUpdate()
	{
		GetActions(action, jumpPressed);
		jumpPressed = false;
	}

	void OnTriggerEnter(Collider other) { // Function to handle collider objects
		if(other.gameObject.tag == "Pickup") 
		{
			other.gameObject.SetActive(false);
		}
	}

	public void GetActions(Vector3 direction, bool jumpPressed) { // Function to handle actions
		if (needsTorque) {
			rigidMarble.AddTorque(new Vector3(direction.z, 0, -direction.x) * speedForce);
		}
		else {
			rigidMarble.AddForce(direction * speedForce);
		}

		// if the marble is on the ground and jump button is pressed
		if (Physics.Raycast(transform.position, -Vector3.up, DistFromGround) && jumpPressed) {
			rigidMarble.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
	}
}
