using UnityEngine;
using System.Collections;


public class HulkController : MonoBehaviour {
	float speed = 1.5f;
	[SerializeField] private float speedJumpUp = 1;

	private bool grounded = true;
	bool facingRight = false;

	private bool secondJump = true;
	private float momment=0;

    private bool thirdJump = false;
    private float mommentThird = 0;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		float direction = Input.GetAxisRaw ("Horizontal");
		if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width / 2)
				direction = 1;
            if (touch.position.x < Screen.width / 2)
				direction = -1;
		}
		if (direction < 0 && facingRight)
			Flip ();
		if (direction > 0 && !facingRight)
			Flip ();
		if (direction != 0 && grounded) {
			grounded=false;
            thirdJump = false;
			Vector2 jumpForceUp = new Vector2 (speed * direction, speedJumpUp);
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForceUp, ForceMode2D.Impulse);
			momment = Time.time;
		}
		if (direction != 0 && !grounded && Time.time-momment>0.38 && secondJump) {
			secondJump= false;
			Vector2 jumpForceUp = new Vector2 ((speed/0.6f) * direction, 3);
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForceUp, ForceMode2D.Impulse);
		}
        if (direction != 0 && !grounded && !secondJump && thirdJump && Time.time - mommentThird > 0.45 && mommentThird-momment>0.3)
        {
            thirdJump = false;
            Vector2 jumpForceUp = new Vector2((speed / 1) * direction, 3);
            this.GetComponent<Rigidbody2D>().AddForce(jumpForceUp, ForceMode2D.Impulse);
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
        if (coll.gameObject.tag == "Enemy")
        {
            thirdJump = true;
            mommentThird = Time.time;
        }
        if (coll.gameObject.tag == "wallL")
        {
            Vector2 force = new Vector2(0.3f, 0);
            this.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
        if (coll.gameObject.tag == "wallR")
        {
            Vector2 force = new Vector2(-0.3f, 0);
            this.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
	}
}
