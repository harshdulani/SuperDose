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
	[Header("Prefab Management")]
	public Text gameOverText;
	public Text gameNameText, scoreText, startText, restartInText;
	public GameObject playerPrefab;
	public bool gameOver, gameStarted, playerAlive;

	[Header("Enemy Spawner")]
	public GameObject enemyPrefab;
	public PlayArea playArea;
	public float spawnWait = 1f, gameOverWait = 4f;
	public bool spawnerDebug = false;

	private float newX, newY;
	private float enemiesSpawned;

	[Header("Scoring System")]
	public float enemiesKilled;
	public float totalEnemies = 20, healthHit;
	public int score = 0;

	private GameObject enemyA, enemyB;

	void Awake()
	{
		gameOverText.GetComponent<Text> ().enabled = false;
		scoreText.GetComponent<Text> ().enabled = false;
		gameNameText.text = "Blood Stream";
		startText.text = "Press Left Ctrl or Left Mouse to Start.";
		restartInText.text = "";
		gameOver = true;
		gameStarted = false;
		enemiesKilled = 0f;
		enemiesSpawned = 0f;

		//Cursor.visible = false;	/filhaal no
	}

	void Update()
	{
		if(Input.GetButtonDown("Fire1") && !gameStarted)
		{
			//when NO games have started/ended and someone presses fire
			gameOverText.GetComponent<Text> ().enabled = false;
			scoreText.GetComponent<Text> ().enabled = true;
			StartCoroutine (FadeOut (gameNameText));
			StartCoroutine (FadeOut (startText));
			StartCoroutine (FadeOut (restartInText));
			StartCoroutine (FadeIn (scoreText));
			gameOver = false;
			gameStarted = true;
			playerAlive = true;
			Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
			StartCoroutine ("EnemySpawner");
		}
		else if (!playerAlive && gameStarted && !gameOver) 
		{
			//when there is no player tag obj and game is over
			WaveEnd ();
		}
		//else if(Input.GetButtonDown("Fire1") && gameOver && gameStarted) old
		else if(Input.GetButtonDown("Fire1") && gameOver && gameStarted && !GameObject.FindWithTag("Enemy"))
		{
			//when FIRST game is ended and someone presses fire
			StartCoroutine (FadeIn (restartInText));
			StartCoroutine ("WaitAndGameOver");			//restart game
		}
		if (Input.GetButtonDown ("Cancel"))
			Application.Quit();
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

	public void EnemyKilled(float enemyLevel)
	{
		if(enemiesKilled <= totalEnemies && !gameOver)
		{
			enemiesKilled += Mathf.Pow(2, enemyLevel - 1);
			score = (int)(enemiesKilled / totalEnemies * 100f);
			//score = (int)enemiesKilled;
		}
		scoreText.text = "Cured :" + score + "%";
		if (playerAlive && !gameOver && enemiesKilled >= totalEnemies)
		{
			gameOverText.text = "All Cured Now!";
			WaveEnd ();
		}
	}

	void WaveEnd()
	{
		//add wave end behavior here, like changing next wave enemy count etc
		StopCoroutine ("EnemySpawner");
		//KillRemainingEnemies ();	//too buggy will fix later
		gameOver = true;
		StartCoroutine (FadeIn (gameOverText));
		StartCoroutine (FadeIn (startText));
		StartCoroutine (FadeIn (gameNameText));
	}

	void KillRemainingEnemies ()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach(GameObject i in temp)
		{
			//Kill all remaining enemies, after shrinking them
			//try increasing their rotatespeed a lot
			//try to kill all using foreach
			//kill their movetowardsplayer components
			Destroy (i.GetComponent<MoveTowardsPlayer> ());
			i.GetComponent<RotateEnemy> ().rotateSpeed = 300f;
			StartCoroutine(Shrink (i));
		}
	}

	public IEnumerator Shrink(GameObject i)
	{
        if (i)
        {
            for (float f = i.transform.localScale.x; f >= 0.1f; f -= 0.1f)
            {
                i.transform.localScale = new Vector3(f, f, 0f);
                yield return new WaitForSeconds(0.1f);
            }
            Destroy(i);
            gameOver = true;
            print("game over is true now.");
        }
	}

	IEnumerator FadeIn(Text target)
	{
		if(target.text.CompareTo("Blood Stream") != 0)
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
		if(target.text.CompareTo("Blood Stream") != 0)
			target.GetComponent<Text>().enabled = !target.GetComponent<Text>().enabled;
	}

	IEnumerator WaitAndGameOver()
	{
		for(float f = gameOverWait; f >= 0f; f -= 0.1f) 
		{
			restartInText.text = "Restarting in " + (int)f + "...";
			yield return new WaitForSeconds (0.1f);
		}
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	IEnumerator EnemySpawner()
	{
		if(!spawnerDebug)
		{
			yield return new WaitForSeconds (3f);
			while (score <= 100)
			{
				float multiplier = 1f;
				/*
			 * old spawn system
			 * if (Random.value <= 0.5f) 
			{
				newX = -playArea.xMax + 0.5f;
				newY = -playArea.yMax + 0.5f;
				multiplier = 1f;
			}
			else 
			{
				newX = playArea.xMax - 0.5f;
				newY = playArea.yMax - 0.5f;
				multiplier = -1f;
			}*/

				/*ultra random spawn system
			newX = Random.Range (-playArea.xMax + 0.25f, playArea.xMax - 0.25f);
			newY = Random.Range (-playArea.yMax + 0.25f, playArea.yMax - 0.25f);*/

				//new spawn system

				//randomize multiplier value to randomize Min/Max locations
				if (Random.value <= 0.5f)
					multiplier = -1f;
				else
					multiplier = 1f;

				//spawn system
				if (Random.value > 0.5f) 
				{
					//randomize on X and not Y
					newY = multiplier * (playArea.yMax - 0.25f);
					newX = Random.Range (-(playArea.xMax - 0.25f), (playArea.xMax - 0.25f));
				}
				else
				{
					//randomize on Y and not X
					newX = multiplier * (playArea.xMax - 0.25f);
					newY = Random.Range (-(playArea.yMax - 0.25f), (playArea.yMax - 0.25f));
				}

				//Debug.Log ("New X = " + newX + " New Y = " + newY);
				Vector3 position = new Vector3 (newX, newY, 0f);

				Instantiate (enemyPrefab, position, Quaternion.identity);
				enemiesSpawned++;
				yield return new WaitForSeconds (spawnWait);
			}
		}
	}
}
