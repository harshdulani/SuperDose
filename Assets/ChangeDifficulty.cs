using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDifficulty : MonoBehaviour
{
	public void ChangeDifficultySetting()
	{
		float newValue = (int) transform.GetChild(1).GetComponent<Slider> ().value;

		if(newValue == 1f)
		{
			newValue = 1f;
		}
		else if(newValue == 2f)
		{
			newValue = 1.5f;
		}
		else if(newValue == 3f)
		{
			newValue = 2f;
		}

		PlayerPrefs.SetFloat ("Difficulty", newValue);
	}
}
