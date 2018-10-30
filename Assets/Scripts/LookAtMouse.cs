using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour 
{
	public GameObject shot;
	public Transform spawnSpace;
	public float shotSpeed = 10f;

	void Update ()
	{
		var direction = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
		var angle = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		if (Input.GetButtonDown ("Fire1")) {
			GameObject shotInstance =
				Instantiate (shot, spawnSpace.position, spawnSpace.rotation);
			shotInstance.GetComponent<Rigidbody2D> ().AddForce (transform.up * shotSpeed);
			Destroy (shotInstance, 1f);
		}
	}
}
