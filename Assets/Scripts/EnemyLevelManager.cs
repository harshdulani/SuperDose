using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevelManager : MonoBehaviour 
{
	//according to level,  add sprites
	//increase cure level/ score points for higher level enemies
	//deal with what to do with enemiesKilled
	////get rid of either enemies killed or enemies spawned in GC

	public float enemyLevel = 0f;

	[Header("Enemy Health System")]
	public Sprite newSprite;
	public float health, maxHealth;
	public Color myWhite, myRed;
	public AudioClip warning;

	private MoveTowardsPlayer mtp;
	private SpriteRenderer bg;
	private bool lerpFromRedToWhite, lerpFromWhiteToRed;
	private Color oldColor;

	void Start ()
	{
		maxHealth = health;
		bg = GameObject.Find ("Background").GetComponent<SpriteRenderer> ();
	}

	public void LevelUp (float oldLevel)
	{
		enemyLevel = ++oldLevel;
		lerpFromRedToWhite = true;
		health += 100f;
		transform.localScale = new Vector3 (enemyLevel * 0.35f, enemyLevel * 0.35f, 1f);
		mtp = GetComponent<MoveTowardsPlayer> ();
		mtp.attackWait += 0.25f;
		mtp.attackForceHigh -= 75f;
		mtp.attackForceLow -= 25f;
		GetComponent<AudioSource> ().PlayOneShot (warning, 0.4f);
	}

	public void HealthLow()
	{
		GetComponent<SpriteRenderer> ().sprite = newSprite;
	}

	void Update()
	{
		if(lerpFromRedToWhite) 
		{
			bg.color = Color.Lerp (bg.color, myWhite, 0.35f);
			if(bg.color == myWhite)
			{
				lerpFromRedToWhite = false;
				lerpFromWhiteToRed = true;
			}
		}
		if(lerpFromWhiteToRed) 
		{
			bg.color = Color.Lerp (bg.color, myRed, 0.5f);
			if (bg.color == myRed) 
				lerpFromWhiteToRed = false;
		}
	}
}
