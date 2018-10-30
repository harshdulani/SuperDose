using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
	public float attackForceLow = 300f, attackForceHigh = 600f, closeRadius = 3f, attackWait = 1f;

	private Transform player;
	private Vector3 attack;

	void Start()
	{
		if (GameObject.FindWithTag ("Player")) 
		{
			player = GameObject.FindWithTag ("Player").transform;
			StartCoroutine ("Attack");
		}
	}

	IEnumerator Attack()
	{
		//this wass initially designed to attack after everytime it completes a shrink/grow cycle.
		//so change the while loop wait for method parameter to the one in the RotateEnemy script.

		while (GetComponent<Rigidbody2D> () != null && player != null)	//till the enemy rigidbody & player exist , a safeguard.
		{
			yield return new WaitForSeconds (attackWait);	//.75f
			if (player)
			{
				attack = -(transform.position - player.position);
				if ((attack.x >= -closeRadius && attack.x <= closeRadius) && (attack.y >= -closeRadius && attack.y <= closeRadius))
				{
					//if enemy is within close range, it seems to slow down, hence more aggresive attack
					GetComponent<Rigidbody2D> ().AddForce (attack * attackForceHigh);
					//Debug.Log ("Enemy Attack! at force " + attack + " at " + attackForceHigh);
				}
				else
				{
					GetComponent<Rigidbody2D> ().AddForce (attack * attackForceLow);
					//Debug.Log ("Enemy Attack! at force " + attack + " at " + attackForceLow);
				}
			}
		}
	}
}
