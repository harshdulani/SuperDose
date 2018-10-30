using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public static class PlayArea
{
	public static float xMin = -6.5f, xMax = 6.5f, yMin = -4.5f, yMax = 4.5f;
}*/

public class PlayerMove : MonoBehaviour
{
	public float moveSpeed = 10f;

	public PlayArea playArea;

	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal") * moveSpeed;
		float moveVertical = Input.GetAxis ("Vertical") * moveSpeed;
		GetComponent<Rigidbody2D> ().velocity = new Vector3 (moveHorizontal, moveVertical, 0f);

		transform.position = new Vector3
		(
			Mathf.Clamp (transform.position.x, -playArea.xMax, playArea.xMax),
			Mathf.Clamp (transform.position.y, -playArea.yMax, playArea.yMax),
			0f
		);
	}
}
