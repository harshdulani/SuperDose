using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvasManager : MonoBehaviour
{
	public void ExitGame()
	{
		Debug.Log ("quit triggered.");
		Application.Quit ();
	}

	public void ResumeGame()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ().pauseCanvas.enabled = !GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController> ().pauseCanvas.enabled;
		if(player)
			player.GetComponent<LookAtMouse> ().enabled = !player.GetComponent<LookAtMouse> ().enabled;
		Time.timeScale = 1f;
	}

	public void GotoMenu()
	{
		SceneManager.LoadScene ("Menu");
	}
}
