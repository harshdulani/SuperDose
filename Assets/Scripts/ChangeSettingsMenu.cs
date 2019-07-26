using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSettingsMenu : MonoBehaviour 
{
	public bool toggle = false;

	private GameObject mainCamera, backCamera;

	void Start()
	{
		mainCamera = Camera.main.gameObject;
		backCamera = mainCamera.transform.GetChild (0).gameObject;
		toggle = GraphicSettings.settings.Load();
		if (toggle)
			Toggle ();
	}

	public void Toggle()
	{
		backCamera.SetActive (!backCamera.activeSelf);
		mainCamera.SetActive (!mainCamera.activeSelf);
		mainCamera.SetActive (!mainCamera.activeSelf);
		toggle = !toggle;
		GraphicSettings.settings.Save (toggle);
	}
}
