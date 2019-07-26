using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighScoreSaveFile : MonoBehaviour
{
	public static HighScoreSaveFile saveFile;

	void Awake () 
	{
		if (!saveFile)
		{
			DontDestroyOnLoad (gameObject);
			saveFile = this;
			try
			{
				Load();
			}
			catch
			{
				Save (0);
			}
		}
		else if(saveFile != this)
		{
			Destroy (gameObject);
		}
	}

	public void Save(int newScore)
	{
		FileStream file;
		SaveData data;
		BinaryFormatter bf = new BinaryFormatter ();

		try
		{
			file = File.Open (Application.dataPath + "/score.dat", FileMode.Open);
			data = (SaveData)bf.Deserialize (file);
			print ("exception not caught" + data.highScore);
		}
		catch(Exception e)
		{
			print ("exception " + e + " caught");
			file = File.Create (Application.dataPath + "/score.dat");
			data = new SaveData ();
		}

		print ("score " + newScore);
		print ("high score " + data.highScore);

		if (newScore >= data.highScore)
		{
			data.highScore = newScore;
		}

		print ("new high score " + data.highScore);

		bf.Serialize (file, data);
		file.Close ();
	}

	public int Load()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.dataPath + "/score.dat", FileMode.Open);

		SaveData data = (SaveData)bf.Deserialize (file);
		file.Close ();

		return data.highScore;
	}
}

[Serializable]
class SaveData
{
	public int highScore;
}