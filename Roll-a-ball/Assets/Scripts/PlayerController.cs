using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed;
	public float jump;
	private bool inAir = false;
	public float playerYPos = 0.50f;

	void Update() {
		if(Input.GetKeyDown (KeyCode.Space)) { // ADDS A JUMP ON SPACEBAR KEYDOWN
			if (inAir == false) {
				this.rigidbody.AddForce(new Vector3(0, jump, 0));
			}
		}
		if(this.rigidbody.position.y > playerYPos) { // CHECKS IF PLAYER IS IN THE AIR
			inAir = true;	
			Debug.Log(this.rigidbody.position.y);
		}
		if (this.rigidbody.position.y <= playerYPos) 
			inAir = false;
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigidbody.AddForce (movement * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Pickup") 
		{
			other.gameObject.SetActive(false);
		}
	}
}
