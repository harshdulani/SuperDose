using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make it rotate counter clockwise and grow & shrink every few seconds
public class RotateEnemy : MonoBehaviour
{
	public float rotateSpeed = 10f, growMax = 0.3f;

	private float size;

	void Start()
	{
		StartCoroutine (SizeIt ());
		size = transform.lossyScale.x;
	}

	void Update ()
	{
		GetComponent<Rigidbody2D> ().angularVelocity = rotateSpeed;
	}

	IEnumerator SizeIt()
	{
		while (true)
		{
			yield return new WaitForSeconds (1f);
			//grow
			for(float f = size; f <= (growMax + size); f += 0.1f)
			{
				transform.localScale = new Vector3 (f, f, 1f);
				yield return new WaitForSeconds (0.1f);
			}
			//shrink to normal
			for(float f = (growMax + size); f >= size; f -= 0.1f)
			{
				transform.localScale = new Vector3 (f, f, 1f);
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
}
