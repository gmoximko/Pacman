using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public const int foodCount = 240;
	public delegate void voidFunc ();
	public event voidFunc gameStart;
	public static GameManager gameManager = null;
	public BoardManager boardManager;
	
	private void Awake () {

		if (gameManager == null) {
			gameManager = this;
		} else if (gameManager != this) {
			Destroy(gameObject);
		}
		GameManager.gameManager.gameStart += boardManager.setLevel;
		DontDestroyOnLoad (gameObject);
		gameStart ();
	}

	public void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	private void OnLevelWasLoaded() {
		gameStart ();
	}
}
