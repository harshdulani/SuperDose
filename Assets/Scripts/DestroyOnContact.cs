using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DestroyOnContact : MonoBehaviour 
{
	public GameObject enemyExplosion, enemyExplosionCircle;
	public float shieldPushForceMultiplier = 400f;
	public AudioClip[] hitSounds;

	[Header("Shot Shake")]
	public float magnitude;
	public float roughness, fadeIn, fadeOut, freezeFrameDuration;

	private AudioSource hitSource;

	void OnCollisionEnter2D (Collision2D other)
	{
		GameController gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		EnemyLevelManager elm = GetComponent<EnemyLevelManager> ();
		hitSource = GetComponents<AudioSource> () [1];
		hitSource.pitch = 1f;
		//Debug.Log ("Enemy Collides with " + other.collider.tag);
		if (other.collider.CompareTag ("Shot")) 
		{
			CameraShaker.Instance.ShakeOnce (magnitude, roughness, fadeIn, fadeOut);
			Destroy (other.gameObject);
			hitSource.PlayOneShot (hitSounds [0], hitSource.volume + .1f);


			elm.health -= gameController.healthHit;
			if (elm.health <= (elm.maxHealth / 2)) 
			{
				elm.HealthLow ();
				hitSource.PlayOneShot (hitSounds [1], hitSource.volume + 0.1f);
			}

			if(elm.health <= 0f)
			{
				hitSource.PlayOneShot (hitSounds [2], hitSource.volume - 0.4f);
				Destroy (Instantiate (enemyExplosionCircle, transform.position, Quaternion.identity), 1f);
				Destroy (Instantiate (enemyExplosion, transform.position, transform.rotation), 3.5f);
				gameController.EnemyKilled (elm.enemyLevel);
				GetComponent<SpriteRenderer> ().enabled = false;
				GetComponent<Collider2D> ().enabled = false;
				Destroy (gameObject, 0.5f);
			}
		}
		if(other.collider.CompareTag ("Player"))
		{
			Destroy (other.gameObject);
			//send message to THIS enemy prefab's grow shrink function and make it bigger
			gameController.playerAlive = false;
			gameController.enemiesKilled++;
		}

		if(other.collider.name == "ring")
		{
			hitSource.PlayOneShot (hitSounds [3], hitSource.volume - 0.4f);
			other.gameObject.transform.GetChild (2).gameObject.SetActive (false);
			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
			Vector3 attack = transform.position - other.transform.position;
			rb.AddForce (attack * shieldPushForceMultiplier);
			other.gameObject.GetComponent<PolygonCollider2D> ().enabled = true;
			GameObject.Find ("PowerupCanvas").transform.GetChild (2).gameObject.SetActive (false);
		}


		if(other.collider.CompareTag("Enemy"))
		{
			//kill that one and make this one larger (prefereably 2x) also give this full 2x health
			if (other.gameObject.GetComponent<EnemyLevelManager> ().enemyLevel == elm.enemyLevel)
			{
				if(elm.enemyLevel <= 3)
					gameController.FuseEnemy (gameObject, other.gameObject);
			}
		}

		if(other.collider.CompareTag("Walls"))
		{
			other.gameObject.GetComponent<AudioSource> ().Play ();
		}
	}
}
