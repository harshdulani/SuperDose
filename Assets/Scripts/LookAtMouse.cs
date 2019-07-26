using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour 
{
	public GameObject shot;
	public Transform spawnSpace;
	public float shotSpeed = 10f, shotWaitTime = 0.3f, oldShotTime = 0f;
	public bool holdToShoot = false, is2x = false;
	public AudioClip x2shoot;

	private Vector3 direction;
	private float angle;

	void Start()
	{
		shot.transform.localScale = new Vector3(1f, 3f, 0f);
	}

	void Update ()
	{
		direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
		angle = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		if (Input.GetButtonDown ("Fire1") && (Time.time - oldShotTime) > shotWaitTime && !holdToShoot)
		{
			oldShotTime = Time.time;
			Shoot ();
		}
		if (Input.GetButton ("Fire1") && (Time.time - oldShotTime) > shotWaitTime && holdToShoot) 
		{
			oldShotTime = Time.time;
			Shoot ();
		}
	}

	void Shoot()
	{
		GameObject shotInstance = Instantiate (shot, spawnSpace.position, spawnSpace.rotation);

		//knockback
		Vector3 knockbackVector = new Vector3 (transform.position.x - direction.x * 0.01f * 0.01f, transform.position.y - direction.y * 0.01f * 0.01f, transform.position.z);
		transform.position = knockbackVector;
		//GetComponent<Rigidbody2D> ().AddForce (new Vector2 (transform.position.x - direction.x * 0.01f * knockbackForce, transform.position.y - direction.y * 0.01f * knockbackForce));
		AudioSource shootSound = GetComponent<AudioSource> ();
		shootSound.Play ();
		if (is2x)
			shootSound.PlayOneShot (x2shoot, shootSound.volume);

		shotInstance.GetComponent<Rigidbody2D> ().AddForce (transform.up * shotSpeed);
		Destroy (shotInstance, 1f);
	}
}
