using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject hexaPrefab;
	public float spawnSpeed = 0.5f;

	private float timeToNextSpawn = 0f;

	void Update ()
	{
		if (Time.time >= timeToNextSpawn)
		{
			Instantiate (hexaPrefab, Vector3.zero, Quaternion.identity);
			timeToNextSpawn += Time.time * 1f / spawnSpeed;
		}
	}
}
