using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
	
	public void ChangeLevel(int SceneIndex)
	{
		// Changes scene to the scene associated with the SceneIndex int
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIndex);
	}

	public void ReloadScene()
	{
		// Sets currentScene to the currently open scene
		Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		// Reloads the scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
	}

	public void QuitGame()
	{
		// if the game is running in editor
	#if UNITY_EDITOR
		// stop it from running
		UnityEditor.EditorApplication.isPlaying = false;
		// if not, close the application
	#else
			Application.Quit();
	#endif
	}
}