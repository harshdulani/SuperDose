using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayArea
{
	public float xMax = 6.5f, yMax = 4.5f;
}

public class GameController : MonoBehaviour 
{
	[Header("General")]
	public Text gameOverText;
	public string[] gameOverTextOptions;
	public Text gameNameText, startText, introText, instrText, scoreText, highScoreText;
	public Image progressBarParent, progressBarChild, newHigh;
	public Canvas pauseCanvas;
	public SpriteRenderer cureGlow;
	public GameObject playerPrefab;
	public bool gameOver, gameStarted, playerAlive, restartable;
	public float gameOverWait = 4f;
	public AudioSource menuMusic, gameMusic, beat, ekg, death;
	public AudioClip flatline, endgame;

	[Header("Enemy Spawner")]
	public PlayArea playArea;
	public GameObject enemyPrefab;

	private EnemySpawner es;

	[Header("Powerup Spawner")]
	public float powerupLastActive = 0f;

	[Header("Scoring System")]
	public float enemiesKilled;
	public float totalEnemies = 20, healthHit;
	public int score = 0;
	public float powerupScoreMultiplier = 1f;

	private GameObject enemyA, enemyB, player;
	private PutAwayWalls pawL, pawR, pawT, pawB;

	void Awake()
	{
		print (SceneManager.sceneCount);

		pauseCanvas.enabled = false;
		menuMusic = GameObject.Find("Menu Music").GetComponent<AudioSource> ();
		gameMusic = GameObject.Find("Game Music").GetComponent<AudioSource> ();
		beat = GameObject.Find("Ambience").GetComponents<AudioSource> ()[1];
		ekg = GameObject.Find("Ambience").GetComponents<AudioSource> ()[0];
		gameOverText.text = gameOverTextOptions [Random.Range (0, gameOverTextOptions.Length - 1)];
		gameOverText.GetComponent<Text> ().enabled = false;
		progressBarParent.GetComponent<Image> ().enabled = false;
		scoreText.GetComponent<Text> ().enabled = false;
		//cureGlow.enabled = false;
		highScoreText.text = "★" + PlayerPrefs.GetInt ("HighScore");
		newHigh.GetComponent<Image> ().enabled = false;
		newHigh.transform.GetComponentInChildren<Text> ().enabled = false;
		scoreText.text = "0";
		gameNameText.text = "Super     \n Dose";
		startText.text = "Click to Start!";
		gameOver = true;
		gameStarted = false;
		enemiesKilled = 0f;
		es = GetComponent<EnemySpawner> ();
		es.enemiesSpawned = 0f;
		restartable = true;
		pawL = GameObject.Find ("Left").GetComponent<PutAwayWalls> ();
		pawR = GameObject.Find ("Right").GetComponent<PutAwayWalls> ();
		pawT = GameObject.Find ("Top").GetComponent<PutAwayWalls> ();
		pawB = GameObject.Find ("Bottom").GetComponent<PutAwayWalls> ();
		cureGlow.transform.localScale = new Vector3 (3f, 3f, 1f);
		GameObject.Find ("PowerupTimer").GetComponent<Text> ().text = "";
		GameObject.Find ("PowerupName").GetComponent<Text> ().text = "";
		GameObject.Find ("PowerupCanvas").transform.GetChild (2).gameObject.SetActive (false);
		death = transform.GetChild (0).GetComponents<AudioSource> () [0];
	}

	void Update()
	{
		if(Input.GetButtonDown("Fire1") && !gameStarted && !pauseCanvas.enabled)
		{
			//when NO games have started/ended and someone presses fire
			menuMusic.Stop ();
			gameMusic.Play ();
			beat.Play ();
			ekg.Play ();
			gameOverText.GetComponent<Text> ().enabled = false;
			scoreText.GetComponent<Text> ().enabled = true;
			progressBarParent.GetComponent<Image> ().enabled = true;
			//cureGlow.enabled = true;
			StartCoroutine (FadeOut (gameNameText));
			StartCoroutine (FadeOut (startText));
			StartCoroutine (FadeOut (introText));
			StartCoroutine (FadeOut (instrText));
			gameOver = false;
			gameStarted = true;
			playerAlive = true;
			player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
			es.StartCoroutine ("Routine");
			pawL.WallManager (true);
			pawR.WallManager (true);
			pawT.WallManager (true);
			pawB.WallManager (true);
			powerupLastActive = Time.time;	//so that a powerup doesnt spawn as soon as you start a game if youve waited for a bit
		}
		else if (!playerAlive && gameStarted && !gameOver) 
		{
			//when there is no player tag obj and game is over
			WaveEnd ();
		}
		else if(Input.GetButtonDown("Fire1") && gameOver && gameStarted && isRestartable() && restartable)
		{
			//when FIRST game is ended and someone presses fire
			restartable = false;
			pawL.WallManager (false);
			pawR.WallManager (false);
			pawT.WallManager (false);
			pawB.WallManager (false);
			gameMusic.Stop ();
			menuMusic.Play ();
			beat.Stop ();
			ekg.Stop ();
			SceneManager.LoadScene ("Menu");			//restart game
		}
		if (Input.GetButtonDown ("Cancel")) 
		{
			//SceneManager.LoadScene ("Menu");
			pauseCanvas.enabled = !pauseCanvas.enabled;
			if(player)
				player.GetComponent<LookAtMouse> ().enabled = !player.GetComponent<LookAtMouse> ().enabled;
			if(pauseCanvas.enabled == true)
				Time.timeScale = 0f;
			else
				Time.timeScale = 1f;
		}
	}

	private bool isRestartable()
	{
		if(playerAlive)
		{
			if (GameObject.FindGameObjectWithTag ("Enemy"))
				return false;
			else
				return true;
		}
		else
			return true;
	}

	public void FuseEnemy(GameObject a, GameObject b)
	{
		if(a != enemyA && a != enemyB && b != enemyA && b != enemyB)	//if the enemies havent already sent request for this too
		{
			enemyA = a;
			enemyB = b;
			Vector3 oldPosition = a.transform.position;
			Quaternion oldRotation = a.transform.rotation;
			GameObject newEnemy = Instantiate (enemyPrefab, oldPosition, Quaternion.identity);
			newEnemy.GetComponent<EnemyLevelManager> ().LevelUp (a.GetComponent<EnemyLevelManager> ().enemyLevel);
			Destroy (a);
			Destroy (b);
		}
	}

	public void EnemyKilledOld(float enemyLevel)
	{
		if(enemiesKilled <= totalEnemies && !gameOver)
		{
			enemiesKilled += Mathf.Pow(2, enemyLevel - 1);
			score = (int)(enemiesKilled / totalEnemies * 100f);
			progressBarChild.fillAmount = score / 100f;
			//min size of circle is 0.1f, max is 4f
			float glowStep = 4f / totalEnemies;
			cureGlow.transform.localScale = new Vector3 (cureGlow.transform.localScale.x + glowStep, cureGlow.transform.localScale.y + glowStep, 1f);
		}
		if (playerAlive && !gameOver && enemiesKilled >= totalEnemies)
		{
			gameOverText.text = "All Cured Now!";
			WaveEnd ();
		}
	}

	public void EnemyKilled(float enemyLevel)
	{
		if(playerAlive && !gameOver)
		{
			enemiesKilled += Mathf.Pow(2, enemyLevel - 1) * powerupScoreMultiplier;
			score = (int)(enemiesKilled * 10f * PlayerPrefs.GetFloat ("Difficulty"));
			scoreText.text = score.ToString ();
		}
	}

	void WaveEnd()
	{
		//add wave end behavior here, like changing next wave enemy count etc
		es.StopCoroutine ("Routine");
		death.Play ();
		death.PlayOneShot (flatline);
		beat.Stop ();
		ekg.Stop ();
		gameMusic.Stop ();
		gameMusic.clip = endgame;
		gameMusic.Play ();
		gameOver = true;
		StartCoroutine (FadeIn (gameOverText));
		StartCoroutine (FadeIn (startText));
		StartCoroutine (FadeIn (gameNameText));
		highScoreText.GetComponent<Text> ().enabled = true;
		if (score > PlayerPrefs.GetInt ("HighScore"))
		{
			PlayerPrefs.SetInt ("HighScore", score);
			newHigh.GetComponent<Image> ().enabled = true;
			newHigh.transform.GetComponentInChildren<Text> ().enabled = true;
		}
		highScoreText.text = "★" + PlayerPrefs.GetInt ("HighScore");
	}

	IEnumerator FadeIn(Text target)
	{
		if(target.text.CompareTo(gameNameText.text) != 0)
			target.GetComponent<Text>().enabled = true;
		for (float f = 0; f <= 1f; f += 0.1f) 
		{
			Color c = target.color;
			c.a = f;
			target.color = c;
			yield return new WaitForSeconds(.1f);
		}

	}

	IEnumerator FadeOut(Text target)
	{
		for (float f = 1f; f >= 0; f -= 0.1f) 
		{
			Color c = target.color;
			c.a = f;
			target.color = c;
			yield return new WaitForSeconds(.1f);
		}
		if(target.text.CompareTo(gameNameText.text) != 0)
			target.GetComponent<Text>().enabled = !target.GetComponent<Text>().enabled;
	}
}