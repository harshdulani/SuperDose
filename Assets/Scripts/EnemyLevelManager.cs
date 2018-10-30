using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevelManager : MonoBehaviour 
{
	//according to level,  add sprites
	//increase cure level/ score points for higher level enemies
	//deal with what to do with enemiesKilled
	//get rid of either enemies killed or enemies spawned in GC

	public float enemyLevel = 0f;

	[Header("Enemy Health System")]
	public Sprite newSprite;
	public float health, maxHealth;

	MoveTowardsPlayer mtp;
	//private GameObject enemyA, enemyB;

	void Start ()
	{
		maxHealth = health;
	}

	public void LevelUp (float oldLevel)
	{
		enemyLevel = ++oldLevel;
		health += 100f;
		transform.localScale = new Vector3 (enemyLevel, enemyLevel, 1f);
		mtp = GetComponent<MoveTowardsPlayer> ();
		mtp.attackWait += 0.25f;
		mtp.attackForceHigh -= 75f;
		mtp.attackForceLow -= 25f;
	}

	/*public void FuseEnemy(GameObject a, GameObject b)
	{
		if(a != enemyA && a != enemyB && b != enemyA && b != enemyB)	//if the enemies havent already sent request for this too
		{
			enemyA = a;
			enemyB = b;
			Vector3 oldPosition = a.transform.position;
			Quaternion oldRotation = a.transform.rotation;
			GameObject newEnemy = Instantiate (
				GameObject.FindWithTag ("GameController").GetComponent<GameController> ().enemyPrefab,
				oldPosition, Quaternion.identity) as GameObject;
			Destroy (b);
			newEnemy.GetComponent<EnemyLevelManager> ().LevelUp ();
			Destroy (a);
		}
	}*/

	public void HealthLow()
	{
		GetComponent<SpriteRenderer> ().sprite = newSprite;
	}
}
