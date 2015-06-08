using UnityEngine;
using System.Collections;

public class HulkController : MonoBehaviour {
	float speed = 2;
	[SerializeField] private float speedJumpUp = 1;

	private bool grounded = true;
	bool facingRight = false;

	private bool secondJump = true;
	public float momment=0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float direction = Input.GetAxisRaw ("Horizontal");
		if (Input.touchCount > 0) {
			if (Input.touches [0].deltaPosition.y > 0)
				direction = 1;
			if (Input.touches [0].deltaPosition.y < 0)
				direction = -1;
		}
		if (direction < 0 && facingRight)
			Flip ();
		if (direction > 0 && !facingRight)
			Flip ();
		if (direction != 0 && grounded) {
			grounded=false;
			Vector2 jumpForceUp = new Vector2 (speed * direction, speedJumpUp);
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForceUp, ForceMode2D.Impulse);
			momment = Time.time;
		}
		if (direction != 0 && !grounded && Time.time-momment>0.3 && secondJump) {
			secondJump= false;
			Vector2 jumpForceUp = new Vector2 ((speed/2) * direction, 3);
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForceUp, ForceMode2D.Impulse);
		}
	}
	void Flip()
	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "ground") {
			grounded = true;
			momment=0;
			secondJump= true;
		}
	}
}
