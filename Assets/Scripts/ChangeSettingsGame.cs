using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ChangeSettingsGame : MonoBehaviour
{
	public bool toggle = false;

	private GameObject mainCamera, backCamera;

	void Start () 
	{
		mainCamera = Camera.main.gameObject;
		backCamera = mainCamera.transform.GetChild (0).gameObject;
		toggle = GraphicSettings.settings.Load();
		if (toggle)
			Toggle ();
	}

	void Toggle () 
	{
		backCamera.SetActive (!backCamera.activeSelf);
		mainCamera.SetActive (!mainCamera.activeSelf);
		mainCamera.SetActive (!mainCamera.activeSelf);
		mainCamera.GetComponent<PostProcessingBehaviour> ().enabled = !mainCamera.GetComponent<PostProcessingBehaviour> ().enabled;
		toggle = !toggle;
		GraphicSettings.settings.Save (toggle);
	}
}
