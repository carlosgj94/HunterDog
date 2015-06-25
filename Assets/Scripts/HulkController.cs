using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HulkController : MonoBehaviour
{

    private float duration = 0.4f;
    private float magnitude = 0.1f;

    public AudioClip birdScream;

    public Text puntuacion;
    public Text record;
    public Text Plus2;
    public Text GameOverText;

    public Button RefreshButton;

    private float kickMomment = 0;

    private int puntos = 0;


    float speed = 1.5f;
	[SerializeField] private float speedJumpUp = 1.13f;

	private bool grounded = true;
	bool facingRight = false;

	private bool secondJump = true;
	private float momment=0;
    
    private int tiempoRestante;
	// Use this for initialization

    private bool AdShowed = false;
	void Start ()
	{
        InvokeRepeating("refreshTimer", 1f, 1f);
	    tiempoRestante = 20;
        puntuacion.text = "SCORE: 0";
        record.text = "TIME: 20";
    }

    private void refreshTimer()
    {
        tiempoRestante--;
        record.text = "TIME: " + tiempoRestante;
        Color redMine;
        if (tiempoRestante < 5)
        {
            redMine = new Vector4(0.95f, 0.37f, 0.34f, 1);
            record.color = redMine;
        }
        else
        {
            record.color = Color.white;
        }
        if (tiempoRestante < 0)
        {
            GameOverText.gameObject.SetActive(true);
            RefreshButton.gameObject.SetActive(true);
            record.gameObject.SetActive(false);
            if (Random.Range(0, 5) > 2 && !AdShowed)
            {
                AdShowed = true;
                var ads = new GameController();
                ads.showAds();
            }
            AdShowed = true;
            
        }
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
           // if (!grounded && !secondJump && touch.deltaPosition.y < -5 && Time.time-mommentThird > 0.3 && !thirdJump )
            //{
             //   Vector2 jumpForceUp = new Vector2(0, -2);
              //  this.GetComponent<Rigidbody2D>().AddForce(jumpForceUp, ForceMode2D.Impulse);
                
            //}
           
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
		if (direction != 0 && !grounded && Time.time-momment>0.38 && secondJump) {
			secondJump= false;
			Vector2 jumpForceUp = new Vector2 ((speed/0.6f) * direction, 3);
			this.GetComponent<Rigidbody2D> ().AddForce (jumpForceUp, ForceMode2D.Impulse);
		}
        
	    if (kickMomment != 0)
	    {
	        if (Time.time - kickMomment > 1.5f)
	        {
	            kickMomment = 0;
                Plus2.gameObject.SetActive(false);
	        }
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
        if (coll.gameObject.tag == "Enemy")
        {
            GetComponent<AudioSource>().PlayOneShot(birdScream);
            
            if (!AdShowed)
            {
                puntos++;
                puntuacion.text = "SCORE: " + puntos;
                tiempoRestante += 2;

                Plus2.gameObject.SetActive(true);
                kickMomment = Time.time;
            }
           StartShake();
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

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "ground")
        {
            grounded = true;
            momment = 0;
            secondJump = true;
        }
    }
    public void StartShake()
    {
        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    public IEnumerator Shake()
    {
        float elapsed = 0.0f;
        Vector3 originalCamPos = Camera.main.transform.position;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.Range(0, 2) * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;
            Camera.main.transform.position = new Vector3(x, y+1, originalCamPos.z);
            yield return null;
        }
        Camera.main.transform.position = originalCamPos;
    }
}
