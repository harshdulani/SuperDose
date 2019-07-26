using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAndRush : MonoBehaviour
{
	public float attackHorizontalForce = 700f;

	private GameObject player;
	private Collider2D l, r, t, b;

	void Start ()
	{
		l = GameObject.Find ("Left").GetComponent<BoxCollider2D> ();
		r = GameObject.Find ("Right").GetComponent<BoxCollider2D> ();
		t = GameObject.Find ("Top").GetComponent<BoxCollider2D> ();
		b = GameObject.Find ("Bottom").GetComponent<BoxCollider2D> ();
		Physics2D.IgnoreCollision (GetComponent <PolygonCollider2D> (), l, true);
		Physics2D.IgnoreCollision (GetComponent <PolygonCollider2D> (), r, true);
		Physics2D.IgnoreCollision (GetComponent <PolygonCollider2D> (), t, true);
		Physics2D.IgnoreCollision (GetComponent <PolygonCollider2D> (), b, true);
		print (GetComponent <PolygonCollider2D> ());
	}
	void Update()
	{
		if(!player)
			player = GameObject.FindWithTag ("Player");
		if (Input.GetKeyDown ("q") && player)
		{
			Attack ();
			print ("attack");
		}
	}
	void Attack()
	{
		float bossPosXMultiplier = 1f;
		if (transform.position.x > 0)
			bossPosXMultiplier = 1f;
		else
			bossPosXMultiplier = -1f;
		
		if (player.transform.position.y > 0) 
		{
			transform.position = new Vector3 (17.5f * bossPosXMultiplier, 2.75f, 1f);
		}
		else
		{
			transform.position = new Vector3 (17.5f * bossPosXMultiplier, -2.75f, 1f);
		}
		GetComponent<Rigidbody2D> ().AddForce (new Vector2(-attackHorizontalForce * bossPosXMultiplier, 0f));
	}
}
