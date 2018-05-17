using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
	public KeyCode restartKey;
	

	/// <summary>
	/// Small script to restart the level once R is pressed.
	/// </summary>
	void Update ()
	{
		if (Input.GetKeyDown(restartKey))
		{
			Scene currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.buildIndex);
		}
	}
}
