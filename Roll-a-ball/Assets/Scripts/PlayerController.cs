using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speedForce = 5;
	public float jumpForce = 2;
	public float playerYPos = 0.50f;
	private float maxMarbleRotate = 25;
	public bool jumpPressed;
	private bool needsTorque = true;
	private bool inAir = false;
	private const float DistFromGround = 1f;
	private Rigidbody rigidMarble;
	private Transform cameraPos;
	private Vector3 camerasDirection;
	private Vector3 action;

	private void Awake() {
		if (Camera.main != null) {
			cameraPos = Camera.main.transform;
		}
		else {
			Debug.LogWarning("No main camera found for player's camera-relative controls");
		}
	}

	private void Start() {
		rigidMarble = GetComponent<Rigidbody>();
		GetComponent<Rigidbody>().maxAngularVelocity = maxMarbleRotate;
	}

	void Update() {
		jumpPressed = Input.GetButton("Jump");
		if (cameraPos != null) { // find cameras direction
			camerasDirection = Vector3.Scale(camerasPosition.forward, new Vector3(1, 0, 1)).normalized;
			action = (moveVertical * cameraPos + moveHorizontal * cameraPos.right).normalized;
		}
		if(Input.GetKeyDown (KeyCode.Space)) { // ADDS A JUMP ON SPACEBAR KEYDOWN
			if (inAir == false) {
				this.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0));
			}
		}
		if(this.GetComponent<Rigidbody>().position.y > playerYPos) { // CHECKS IF PLAYER IS IN THE AIR
			inAir = true;	
			Debug.Log(this.GetComponent<Rigidbody>().position.y);
		}
		if (this.GetComponent<Rigidbody>().position.y <= playerYPos) 
			inAir = false;
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		GetComponent<Rigidbody>().AddForce (movement * speedForce * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Pickup") 
		{
			other.gameObject.SetActive(false);
		}
	}

	public void GetActions(Vector3 direction, bool jumpPressed) {
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
