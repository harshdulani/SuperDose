using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupSelector : MonoBehaviour 
{
	//this is on the powerup
	/*
		0 - shots 2x powerful - 2x logo
		1 - hold to shoot - crosshair logo
		2 - slow enemies - timer logo
		3 - shield - shield logo
	*/

	public GameObject[] powerupVFX;
	public Sprite[] powerupSprite;
	public float[] effectTimer;
	public float spawnTime, onScreenTime = 4f, startTime;
	public Color myRed, myPurple;
	public GameObject shieldRing;
	public AudioClip powerup, pickup, fastBeat, fastEKG, slowBeat, slowEKG;

	private int selection = 0;
	private bool activated = false, lerpFromRedToPurple = false, lerpFromPurpleToRed = false, slowOn = false;
	private GameController gc;
	private EnemySpawner es;
	private LookAtMouse lam;
	private SpriteRenderer sr, bg;
	private GameObject powerupTimer, powerupName, pillayer;

	void Awake ()
	{
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		es = gc.gameObject.GetComponent<EnemySpawner> ();
		lam = GameObject.FindGameObjectWithTag ("Player").GetComponent<LookAtMouse> ();
		sr = GetComponent<SpriteRenderer> ();
		bg = GameObject.Find ("Background").GetComponent<SpriteRenderer> ();
		shieldRing = GameObject.FindGameObjectWithTag ("Player").transform.GetChild (2).gameObject;
		//select random powerups
		selection = (int)Random.Range (0f, powerupSprite.Length);

		sr.sprite = powerupSprite [selection];
		sr.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

		spawnTime = Time.time;
		powerupTimer = GameObject.Find ("PowerupTimer");
		powerupName = GameObject.Find ("PowerupName");
	}

	void Update()
	{
		if(Time.time - spawnTime > onScreenTime && !activated)
		{
			//destroy powerup if not activated
			gc.powerupLastActive = Time.time;
			Destroy (gameObject);
		}
		if(lerpFromRedToPurple && bg.color != myPurple)
			bg.color = Color.Lerp (bg.color, myPurple, 0.05f);
		if(lerpFromPurpleToRed && bg.color != myRed)
			bg.color = Color.Lerp (bg.color, myRed, 0.05f);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player") || (selection != 3 && other.name == "ring"))
			StartCoroutine (PickUp (other));
	}

	IEnumerator PickUp(Collider2D player)
	{
		activated = true;
		Destroy (Instantiate (powerupVFX [selection], Vector3.zero, Quaternion.identity), 4f);
		GetComponent<AudioSource> ().PlayOneShot (powerup);
		GetComponent<AudioSource> ().PlayOneShot (pickup);

		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<CircleCollider2D> ().enabled = false;

		lerpFromRedToPurple = true;
		if(selection == 0 || selection == 1 || selection == 2)
		{
			//fast bpms
			//stop music if player dies w powerup in gc
			/*gc.beat.Stop ();
			gc.beat.clip = fastBeat;
			gc.beat.volume += 0.2f;
			gc.beat.Play ();*/
			gc.ekg.clip = fastEKG;
			gc.ekg.pitch += 0.2f;
			gc.ekg.Play ();

			//increase score multiplier
			gc.powerupScoreMultiplier *= 2.5f;
		}
		if(selection == 0)
		{
			/*use if setting base shot round 2.1875f
			 * pillayer.GetComponent<LookAtMouse> ().shot.transform.localScale = new Vector3 
				(pillayer.GetComponent<LookAtMouse> ().shot.transform.localScale.x * 0.6f,
					pillayer.GetComponent<LookAtMouse> ().shot.transform.localScale.y * 2f,
				0f);*/
			pillayer = GameObject.FindGameObjectWithTag("Player");
			pillayer.GetComponent<LookAtMouse> ().shot.transform.localScale = new Vector3 (2.18f, 2.18f, 0f);
			AudioSource audioSource = pillayer.GetComponent<AudioSource> ();
			audioSource.pitch = 0.65f;
			audioSource.volume *= 1.7f;
			lam.is2x = true;
			gc.healthHit *= 2;
			es.spawnWaitMin -= 1.5f;
			es.spawnWaitMax -= 1.5f;
			StartCoroutine (TimerText(selection));
			powerupName.GetComponent<Text>().text = "Shots 2x powerful";
			yield return new WaitForSeconds (effectTimer[selection]);
			pillayer.GetComponent<LookAtMouse> ().shot.transform.localScale = new Vector3(1f, 3f, 0f);
			lam.is2x = false;
			es.spawnWaitMin += 1.5f;
			es.spawnWaitMax += 1.5f;
			gc.healthHit /= 2;
			audioSource.pitch = 1f;
			audioSource.volume /= 1.7f;
		}
		else if (selection == 1)
		{
			lam.shotWaitTime *= 0.3f;
			lam.GetComponent<AudioSource> ().pitch = 1.15f;
			lam.holdToShoot = true;
			es.spawnWaitMin -= 1f;
			es.spawnWaitMax -= 2.5f;
			powerupName.GetComponent<Text>().text = "Hold Mouse to Shoot";
			StartCoroutine (TimerText(selection));
			yield return new WaitForSeconds (effectTimer[selection]);
			es.spawnWaitMin += 1f;
			es.spawnWaitMax += 2.5f;
			if(lam) 
			{
				lam.holdToShoot = false;
				lam.shotWaitTime /= 0.3f;
				lam.GetComponent<AudioSource> ().pitch = 1f;
			}
		}
		else if(selection == 2)
		{
			InvokeRepeating ("TurnOnSlow", 0f, 1f);
			slowOn = true;

			powerupName.GetComponent<Text>().text = "Slow Enemies";
			StartCoroutine (TimerText(selection));
			es.spawnWaitMin -= 0.75f;
			es.spawnWaitMax -= 0.75f;
			yield return new WaitForSeconds (effectTimer[selection]);
			slowOn = false;
			es.spawnWaitMin += 0.75f;
			es.spawnWaitMax += 0.75f;
			GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
			foreach(GameObject activeEnemy in activeEnemies)
			{
				if(activeEnemy)
				{
					MoveTowardsPlayer mtp = activeEnemy.GetComponent<MoveTowardsPlayer> ();
					if (mtp)
					{
						//mtp.attackWait /= 2.5f;
						mtp.attackForceHigh /= 0.5f;
						mtp.attackForceLow /= 0.5f;
					}
				}
			}
		}
		else if(selection == 3)
		{
			shieldRing.SetActive (true);
			shieldRing.transform.parent.GetComponent<PolygonCollider2D> ().enabled = false;
			GameObject.Find ("PowerupCanvas").transform.GetChild (2).gameObject.SetActive (true);
			//show white shield logo on bottom left
			//Set it such that the powerupLastActiveTime is also set properly
			//add fake wait time if needed
			//make arrangements in shield.cs & destroyoncontact.cs
		}
		if(selection == 0 || selection == 1 || selection == 2)
		{
			//revert bpms
			/*gc.beat.Stop ();
			gc.beat.clip = slowBeat;
			gc.beat.volume -= 0.2f;
			gc.beat.Play ();*/
			gc.ekg.clip = slowEKG;
			gc.ekg.pitch -= 0.1f;
			gc.ekg.Play ();

			//revert score multiplier
			gc.powerupScoreMultiplier /= 2.5f;
		}
		powerupName.GetComponent<Text>().text = "";
		lerpFromRedToPurple = false;
		lerpFromPurpleToRed = true;

		gc.powerupLastActive = Time.time;
		Destroy (gameObject, 3f);
	}

	IEnumerator TimerText(int selection)
	{
		startTime = Time.time;
		while((effectTimer[selection] + (startTime - Time.time)) >= 0)
		{
			yield return new WaitForSeconds (0.1f);
			powerupTimer.GetComponent<Text> ().text = (effectTimer[selection] + (startTime - Time.time)).ToString ("n1");
		}
		powerupTimer.GetComponent<Text>().text = "";
	}

	void TurnOnSlow()
	{
		if(slowOn)
		{
			GameObject[] activeEnemies;
			activeEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
			foreach (GameObject activeEnemy in activeEnemies) 
			{
				if (activeEnemy) 
				{
					MoveTowardsPlayer mtp = activeEnemy.GetComponent<MoveTowardsPlayer> ();
					if (mtp) 
					{
						//mtp.attackWait *= 2.5f;
						mtp.attackForceHigh *= 0.5f;
						mtp.attackForceLow *= 0.5f;
					}
				}
			}
		}
		else
			CancelInvoke ();
	}
}
