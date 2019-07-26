using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvasManager : MonoBehaviour
{
	public GameObject original, howTo, about;
	public Text highScore;

	private bool howToFlag = false, aboutFlag = false;
	private GameObject powerupCanvas, creditCanvas, perfCanvas, menuCanvas;

	void Awake()
	{
		print (SceneManager.sceneCount);
	}


	void Start()
	{
		AudioSource menuMusic = GameObject.Find("Menu Music").GetComponent<AudioSource> ();
		if (!menuMusic.isPlaying) 
		{
			print (menuMusic.isPlaying);
			menuMusic.Play ();
		}
		powerupCanvas = GameObject.Find ("PowerupCanvas");
		powerupCanvas.SetActive (howToFlag);
		creditCanvas = GameObject.Find ("CreditCanvas");
		creditCanvas.transform.GetChild (1).gameObject.SetActive (aboutFlag);
		perfCanvas = GameObject.Find ("PerfCanvas");
		perfCanvas.SetActive (true);
		menuCanvas = GameObject.Find ("MenuCanvas");
		menuCanvas.SetActive (true);
		highScore.text = "★" + PlayerPrefs.GetInt ("HighScore");
	}

	public void PlayGame()
	{
		SceneManager.LoadScene ("Main");
	}
	public void HowTo()
	{
		howToFlag = !howToFlag;
	}
	public void About()
	{
		if(!aboutFlag)
		{
			creditCanvas.transform.GetChild (1).gameObject.SetActive (true);
			menuCanvas.SetActive (false);
			perfCanvas.SetActive (false);
		}
		else
		{
			creditCanvas.transform.GetChild (1).gameObject.SetActive (false);
			menuCanvas.SetActive (true);
			perfCanvas.SetActive (true);
		}
		aboutFlag = !aboutFlag;
	}

	void Update()
	{
		if(Camera.main)
		{
			if (howToFlag && Camera.main.transform.rotation != howTo.transform.rotation)
				Camera.main.transform.Rotate (new Vector3 (0f, Mathf.Lerp (Camera.main.transform.rotation.y, -3f, 0.1f), 0f));
			else if (!howToFlag && Camera.main.transform.rotation != original.transform.rotation)
				Camera.main.transform.Rotate (new Vector3 (0f, Mathf.InverseLerp (Camera.main.transform.rotation.y, 0f, 0.01f), 0f));
			else if (Camera.main.transform.rotation == howTo.transform.rotation
			       || Camera.main.transform.rotation == original.transform.rotation)
				powerupCanvas.SetActive (howToFlag);
		}
	}

	public void ExitGame()
	{
		Debug.Log ("quit triggered.");
		Application.Quit ();
	}
}