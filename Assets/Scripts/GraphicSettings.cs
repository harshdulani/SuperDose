using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GraphicSettings : MonoBehaviour 
{
	public static GraphicSettings settings;

	void Awake () 
	{
		if (!settings)
		{
			DontDestroyOnLoad (gameObject);
			settings = this;
			try
			{
				Load();
			}
			catch
			{
				Save (false);
			}
		}
		else if(settings != this)
		{
			Destroy (gameObject);
		}
	}

	public void Save(bool toggleValue)
	{
		FileStream file;
		BinaryFormatter bf = new BinaryFormatter ();

		file = File.Create (Application.dataPath + "/graphics.dat");

		SettingData data = new SettingData ();
		data.lowSettings = toggleValue;

		bf.Serialize (file, data);
		file.Close ();
	}

	public bool Load()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.dataPath + "/graphics.dat", FileMode.Open);
	
		SettingData data = (SettingData)bf.Deserialize (file);
		file.Close ();

		return data.lowSettings;
	}
}

[Serializable]
class SettingData
{
	public bool lowSettings;
}