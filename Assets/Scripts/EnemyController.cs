using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour {
    
    
    private Animator anim;

    private bool movement=false;

    private float jump = 0;

    private bool muerto = false;

    private int higherPos;
    
    private float forceYMicro;

    private float velocityX;
	// Use this for initialization
	void Start ()
	{
	    velocityX = Random.Range(0.5f, 1.5f);
        higherPos = Random.Range(7, 9);

        jump = 0;

        anim = this.gameObject.GetComponent<Animator>();
        anim.SetBool("Died", false);//Walking animation is deactivated
    }
	
	// Update is called once per frame
    void Update()
    {
        if (jump == 0)
        {
            jump = Time.time;
        }
        if (Time.time - jump > 1/70f)
        {
            movement = false;
            jump = 0;
        }
        if (!movement)
        {
            Vector2 forceX = new Vector2(velocityX, 0);
            this.GetComponent<Rigidbody2D>().AddForce(forceX);
            movement = true;
            if (muerto)
            {
                Vector2 force = new Vector2(0, 5f);
                this.GetComponent<Rigidbody2D>().AddForce(force);
            }
            else
            {
                if (gameObject.transform.position.y < higherPos)
                {
                    float dist = 8 - gameObject.transform.position.y;
                    Vector2 force = new Vector2(0, 0.3f*(dist*dist));
                    this.GetComponent<Rigidbody2D>().AddForce(force);
                }
                
            }
             
            
        }
        if (gameObject.transform.position.x < -13 || gameObject.transform.position.x > 13)
        {
            Destroy(gameObject);
        }
        if (gameObject.transform.position.y > 9 && muerto)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            muerto = true;
            Vector2 force = new Vector2(0, 100);
            this.GetComponent<Rigidbody2D>().AddForce(force);
            this.GetComponent<PolygonCollider2D>().isTrigger = true;
            anim.SetBool("Died", true);//Walking animation is deactivated
        }
    }
    
}
