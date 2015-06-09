using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public Text puntuacion;
    public Text record;
    private int puntos = 0;
    private int recordPuntos = 0;

    private bool movement=false;
	// Use this for initialization
	void Start () {
        puntuacion.text = "Score: 0";
        record.text = "Record: 0";
	}
	
	// Update is called once per frame
    void Update()
    {
        
        if (!movement)
        {
            movement = true;
            Vector2 force = new Vector2(0 , 0.4f);
            this.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
  
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "wallL")
        {
            Vector2 force = new Vector2(0.2f, 0);
            this.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
        if (coll.gameObject.tag == "wallR")
        {
            Vector2 force = new Vector2(-0.2f, 0);
            this.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
        if (coll.gameObject.tag == "Player")
        {
            puntos++;
            puntuacion.text = "Score: " + puntos + " ";
            if (puntos >= recordPuntos)
            {
                recordPuntos = puntos;
                record.text = "Record: " + puntos + " ";
            }
        }

        
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "ground")
        {
            movement = false;
            puntos = 0;
            puntuacion.text = "Score: " + puntos + " ";
        }
    }
}
